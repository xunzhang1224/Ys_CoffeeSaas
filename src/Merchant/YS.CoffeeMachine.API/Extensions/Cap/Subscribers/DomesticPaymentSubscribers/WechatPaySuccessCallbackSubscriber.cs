using DotNetCore.CAP;
using MessagePack;
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

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers.DomesticPaymentSubscribers
{
    /// <summary>
    /// 微信支付成功异步回调接口
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="_iotBaseService"></param>
    /// <param name="_publish"></param>
    /// <param name="_orderPaymentMethodUnit"></param>
    public class WechatPaySuccessCallbackSubscriber(CoffeeMachineDbContext context, ILogger<WechatPaySuccessCallbackSubscriber> logger, IotBaseService _iotBaseService, IPublishService _publish, OrderPaymentMethodUnit _orderPaymentMethodUnit) : ICapSubscribe
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.WechatPaySuccessCallback)]
        public async Task Handle(PaymentMessageDto input)
        {
            logger.LogInformation("接收到微信异步回调消息，订单号：{OrderId}", input.OrderId);

            // 获取订单信息
            var orderInfo = await context.OrderInfo.AsNoTracking().IgnoreQueryFilters().Include(i => i.OrderDetails).FirstOrDefaultAsync(w => w.Code == input.OrderId);
            if (orderInfo == null)
            {
                logger.LogWarning("未找到对应的订单信息，订单号：{OrderId}", input.OrderId);
                return;
            }

            #region 状态检查

            if (orderInfo.OrderStatus != OrderStatusEnum.PaymentInProgress)
            {
                logger.LogInformation("订单状态非支付中，订单号：{OrderId}，当前状态：{OrderStatus}", input.OrderId, orderInfo.OrderStatus);
                return;
            }

            if (orderInfo.OrderStatus == OrderStatusEnum.Success)
            {
                logger.LogInformation("订单已支付成功，订单号：{OrderId}", input.OrderId);
                return;
            }

            // 更新订单状态信息
            orderInfo.SetSaleResult(OrderSaleResult.Success);
            orderInfo.SetOrderStatus(OrderStatusEnum.Success);
            if (input.PayTime.HasValue)
                orderInfo.SetPayTimeSp(input.PayTime.Value.ToUnixTimeMilliseconds(), input.PayTime);
            orderInfo.SetThirdOrderId(input.OrderNo!);
            context.OrderInfo.Update(orderInfo);
            await context.SaveChangesAsync();

            // 检查是否支付成功回调超时
            DateTime currUtcTime = DateTime.UtcNow;
            double payTotalSeconds = (currUtcTime - input.PayTime!.Value).TotalSeconds; // 当前时间跟支付成功时间相差的秒数（安卓等待时间时90秒，这里暂时限制不超过90秒）
            double createTotalSeconds = (currUtcTime - orderInfo.CreateTime).TotalSeconds; // 当前时间跟订单创建时间相差的秒数
            if (payTotalSeconds > 90 || createTotalSeconds > 90)
            {
                logger.LogInformation(@$"订单支付成功消息通知(在线支付）,执行具体的操作---支付成功回调超时，
                    当前时间跟支付成功时间相差的秒数:{payTotalSeconds}，当前时间跟订单创建时间相差的秒数：{createTotalSeconds},
                    content:{JsonConvert.SerializeObject(input)}");

                PaymentSuccessCallbackTimeoutDto dto = new PaymentSuccessCallbackTimeoutDto()
                {
                    OrderCode = orderInfo.Code,
                    OrderNo = input.OrderNo
                };

                // 处理超时的支付成功回调(退款操作)
                await _orderPaymentMethodUnit.PaymentSuccessCallbackTimeout(dto);

                return;
            }

            #endregion

            #region 更新订单状态信息

            // 更新订单状态信息
            orderInfo.SetSaleResult(OrderSaleResult.Success);
            orderInfo.SetOrderStatus(OrderStatusEnum.Success);
            if (input.PayTime.HasValue)
                orderInfo.SetPayTimeSp(input.PayTime.Value.ToUnixTimeMilliseconds(), input.PayTime);
            orderInfo.SetThirdOrderId(input.OrderNo!);
            context.OrderInfo.Update(orderInfo);
            await context.SaveChangesAsync();
            #endregion

            bool flowControl = await CommandSender(orderInfo);
        }

        /// <summary>
        /// 下发远程出货指定
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
                logger.LogError(JsonConvert.SerializeObject($"支付成功回调：未找到 {orderInfo.DeviceBaseId} 设备信息"));
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

    /// <summary>
    /// 3201指令发送参数
    /// </summary>
    public class Iot3201SentDto
    {
        /// <summary>
        /// mid
        /// </summary>
        public string Mid { get; set; } = "0000000000";

        /// <summary>
        /// TimeSp
        /// </summary>
        public long TimeSp { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        /// <summary>
        /// 指令号
        /// </summary>
        public static readonly int KEY = 3201;

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单出货明细
        /// </summary>
        public List<Order> Orders { get; set; }
    }

    /// <summary>
    /// 数据结构
    /// </summary>
    [MessagePackObject(true)]
    public class Order
    {
        /// <summary>
        /// 子订单号
        /// </summary>
        public string SubOrderNo { get; set; }

        /// <summary>
        /// 出货编号
        /// </summary>
        public string DeliveryId { get; set; }

        /// <summary>
        /// 货道号
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; } = string.Empty;

        /// <summary>
        /// 附件参数
        /// </summary>
        public string Extra { get; set; } = string.Empty;

        /// <summary>
        /// 货柜编号
        /// </summary>
        public int CounterNo { get; set; }
    }
}