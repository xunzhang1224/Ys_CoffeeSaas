using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos.PaymentEventSubscribe;
using YS.CoffeeMachine.API.Services;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayPaymentDtos;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.Util.Core;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers.DomesticPaymentSubscribers
{
    /// <summary>
    /// 支付宝支付成功异步回调接口
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_logger"></param>
    /// <param name="_iotBaseService"></param>
    /// <param name="_publish"></param>
    /// <param name="_orderPaymentMethodUnit"></param>
    public class AlipaySuccessCallbackSubscriber(CoffeeMachineDbContext context, ILogger<AlipaySuccessCallbackSubscriber> _logger,
        IotBaseService _iotBaseService, IPublishService _publish, OrderPaymentMethodUnit _orderPaymentMethodUnit)
        : ICapSubscribe
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.AlipaySuccessCallback)]
        public async Task Handle(PaymentMessageDto requestDto)
        {
            _logger.LogInformation("接收到支付宝异步回调消息，订单号：{OrderId}", requestDto.OrderId);

            try
            {
                if (requestDto == null || string.IsNullOrWhiteSpace(requestDto.OrderId) || string.IsNullOrWhiteSpace(requestDto.OrderNo))
                {
                    _logger.LogError("[订单支付成功]消息订阅处理失败,参数错误");
                    throw ExceptionHelper.AppFriendly("参数不合法");
                }

                // 主订单状态
                var orderStatus = OrderStatusEnum.Success;

                // 获取订单
                var orderInfo = await context.OrderInfo.AsNoTracking().IgnoreQueryFilters().Include(i => i.OrderDetails).FirstOrDefaultAsync(w => w.Code == requestDto.OrderId);
                if (orderInfo == null)
                {
                    _logger.LogWarning("未找到对应的订单信息，订单号：{OrderId}", requestDto.OrderId);
                    return;
                }

                #region 状态检查

                if (orderInfo.OrderStatus != OrderStatusEnum.PaymentInProgress)
                {
                    _logger.LogInformation("订单状态非支付中，订单号：{OrderId}，当前状态：{OrderStatus}", requestDto.OrderId, orderInfo.OrderStatus);
                    return;
                }
                if (orderStatus == orderInfo.OrderStatus)
                {
                    _logger.LogInformation("订单已支付成功，订单号：{OrderId}", requestDto.OrderId);
                    return;
                }
                #endregion

                #region 更新订单状态信息

                // 更新订单状态信息
                orderInfo.SetSaleResult(OrderSaleResult.Success);
                orderInfo.SetOrderStatus(OrderStatusEnum.Success);
                if (requestDto.PayTime.HasValue)
                    orderInfo.SetPayTimeSp(requestDto.PayTime.Value.ToUnixTimeMilliseconds(), requestDto.PayTime);
                orderInfo.SetThirdOrderId(requestDto.OrderNo!);
                context.OrderInfo.Update(orderInfo);
                await context.SaveChangesAsync();

                // 检查是否支付成功回调超时
                DateTime currUtcTime = DateTime.UtcNow;
                double payTotalSeconds = (currUtcTime - requestDto.PayTime!.Value).TotalSeconds; // 当前时间跟支付成功时间相差的秒数（安卓等待时间时90秒，这里暂时限制不超过90秒）
                double createTotalSeconds = (currUtcTime - orderInfo.CreateTime).TotalSeconds; // 当前时间跟订单创建时间相差的秒数
                if (payTotalSeconds > 90 || createTotalSeconds > 90)
                {
                    _logger.LogInformation(@$"订单支付成功消息通知(在线支付）,执行具体的操作---支付成功回调超时，
                    当前时间跟支付成功时间相差的秒数:{payTotalSeconds}，当前时间跟订单创建时间相差的秒数：{createTotalSeconds},
                    content:{Newtonsoft.Json.JsonConvert.SerializeObject(requestDto)}");

                    PaymentSuccessCallbackTimeoutDto dto = new PaymentSuccessCallbackTimeoutDto()
                    {
                        OrderCode = orderInfo.Code,
                        OrderNo = requestDto.OrderNo
                    };

                    // 处理超时的支付成功回调(退款操作)
                    await _orderPaymentMethodUnit.PaymentSuccessCallbackTimeout(dto);

                    return;
                }
                #endregion

                // 在线支付成功，远程出货并修改订单状态
                bool flowControl = await CommandSender(orderInfo);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"订单支付成功消息通知(在线支付）,执行具体的操作---发生异常，content：" + JsonConvert.SerializeObject(requestDto));
                throw ExceptionHelper.AppFriendly("订单支付成功消息通知(在线支付）,执行具体的操作,发生异常");
            }
        }

        /// <summary>
        /// 下发远程出货指定(TODO：这里后续做成后台任务或队列去操作)
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        private async Task<bool> CommandSender(OrderInfo orderInfo)
        {
            #region 远程出货

            // 检查设备基础信息
            var deviceBaseInfo = await context.DeviceBaseInfo
                .AsQueryable()
                .AsNoTracking().IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == orderInfo.DeviceBaseId);
            if (deviceBaseInfo == null)
            {
                _logger.LogError(JsonConvert.SerializeObject($"支付成功回调：未找到 {orderInfo.DeviceBaseId} 设备信息"));
                return false;
            }

            Iot3201SentDto iot3201SentDto = new Iot3201SentDto();
            iot3201SentDto.Mid = deviceBaseInfo.Mid;

            iot3201SentDto.Orders = new List<Order>();
            foreach (var item in orderInfo.OrderDetails)
            {
                Order subOrder = new Order();
                subOrder.Slot = item.SlotNo;
                subOrder.SKU = item.ItemCode;
                subOrder.Price = item.Price;
                subOrder.PayType = orderInfo.Provider;
                subOrder.SubOrderNo = item.Id.ToString();
                iot3201SentDto.Orders.Add(subOrder);
            }

            iot3201SentDto.OrderNo = orderInfo.Code;

            //// 发送远程出货指令
            //// 检查设备是否在线并下发出货指令
            var sendDto = new CommandDownSend()
            {
                IsRecordLog = true,
                Mid = deviceBaseInfo.Mid,
                Method = "3201",
                Params = JsonConvert.SerializeObject(iot3201SentDto)
            };
            await _iotBaseService.PaymentSuccessSend(sendDto);

            #endregion
            return true;
        }
    }
}