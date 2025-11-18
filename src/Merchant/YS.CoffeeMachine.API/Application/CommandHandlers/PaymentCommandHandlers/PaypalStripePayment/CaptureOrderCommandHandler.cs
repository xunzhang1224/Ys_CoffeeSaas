using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Extensions.Cap.Subscribers.PaymentSubscribers;
using YS.CoffeeMachine.API.Services;
using YS.CoffeeMachine.Application.Commands.PaymentCommands.PaypalStripePayment;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.Pay.SDK;
using YS.Pay.SDK.Pay.Request;
using YS.Pay.SDK.Response;
using YS.Pay.SDK.Top;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.PaymentCommandHandlers.PaypalStripePayment
{
    /// <summary>
    /// 订单捕获
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <param name="_iotBaseService"></param>
    public class CaptureOrderCommandHandler(CoffeeMachineDbContext context, IConfiguration configuration, IotBaseService _iotBaseService) : ICommandHandler<CaptureOrderCommand, CaptureOrderResponse>
    {
        /// <summary>
        /// 执行订单捕获
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CaptureOrderResponse> Handle(CaptureOrderCommand input, CancellationToken cancellationToken)
        {
            // 获取并验证订单信息
            var orderInfo = await context.OrderInfo.AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync(x => x.ThirdOrderId == input.payOrderId, cancellationToken);
            if (orderInfo == null)
                throw ExceptionHelper.AppFriendly($"未找到对应的订单信息，PayOrderId: {input.payOrderId}");
            if (orderInfo.SaleResult == OrderSaleResult.Success || orderInfo.SaleResult == OrderSaleResult.Cancel || orderInfo.SaleResult == OrderSaleResult.Refund)
                throw ExceptionHelper.AppFriendly($"订单状态异常，当前状态: {orderInfo.SaleResult}不允许操作, PayOrderId: {input.payOrderId}");

            // 检查设备基础信息
            var deviceBaseInfo = await context.DeviceBaseInfo.AsQueryable()
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderInfo.DeviceBaseId);
            if (deviceBaseInfo == null)
                throw ExceptionHelper.AppFriendly(JsonConvert.SerializeObject($"支付成功回调：未找到 {orderInfo.DeviceBaseId} 设备信息"));

            // 组装捕获请求
            CaptureOrderRequest request = new CaptureOrderRequest()
            {
                PayOrderId = input.payOrderId
            };
            DefaultTopApiClient topClient = new DefaultTopApiClient(long.Parse(configuration["YsPaymentPlatform:AppId"] ?? "0"), configuration["YsPaymentPlatform:AppKey"] ?? string.Empty, configuration["YsPaymentPlatform:TestUrl"] ?? string.Empty);
            var result = await topClient.Execute(request);

            // 更新订单状态
            switch (result.PayOrderStatus)
            {
                case PayOrderStatusEnum.Obligation:
                    orderInfo.SetSaleResult(OrderSaleResult.NotPay);
                    break;
                case PayOrderStatusEnum.Success:
                    orderInfo.SetSaleResult(OrderSaleResult.Success);
                    orderInfo.SetPayTimeSp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), DateTime.UtcNow);

                    #region 制作饮品/出货逻辑
                    // 组装饮品信息
                    var parmeters = new List<Ito6216ParmeterDto>();
                    orderInfo.OrderDetails.ForEach(item =>
                    {
                        parmeters.Add(new Ito6216ParmeterDto
                        {
                            ServerId = item.ItemCode // 饮品SKU
                        });
                    });

                    // 检查设备是否在线并下发出货指令（TODO: 需要改成对接3201购物车出货，带上订单信息）
                    var sendDto = new CommandDownSend()
                    {
                        IsRecordLog = true,
                        Mid = deviceBaseInfo.Mid,
                        Method = "6216",
                        Params = JsonConvert.SerializeObject(new Ito6216SentDto
                        {
                            CapabilityId = 67, // 67是出货能力ID
                            Mid = deviceBaseInfo.Mid,
                            Parameters = parmeters
                        }),
                    };
                    await _iotBaseService.PaymentSuccessSend(sendDto);
                    #endregion

                    break;
                case PayOrderStatusEnum.Fail:
                    orderInfo.SetSaleResult(OrderSaleResult.Fail);
                    orderInfo.SetPayErrorInfo(result.ErrCode, result.ErrMsg);
                    break;
                case PayOrderStatusEnum.CancelPayment:
                    orderInfo.SetSaleResult(OrderSaleResult.Cancel);
                    break;
            }

            // 返回结果
            return result;
        }
    }
}