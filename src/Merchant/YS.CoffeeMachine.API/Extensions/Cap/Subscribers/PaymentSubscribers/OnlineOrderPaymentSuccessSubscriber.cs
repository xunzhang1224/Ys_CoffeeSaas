using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Services;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.Pay.SDK.ServicePlatform.Request;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers.PaymentSubscribers
{
    /// <summary>
    /// 在线支付成功回调订阅
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="_iotBaseService"></param>
    public class OnlineOrderPaymentSuccessSubscriber(CoffeeMachineDbContext context, ILogger<OnlineOrderPaymentSuccessSubscriber> logger, IotBaseService _iotBaseService) : ICapSubscribe
    {
        /// <summary>
        /// 在线支付成功回调订阅
        /// </summary>
        [CapSubscribe(CapConst.OnlineOrderPaymentSuccess)]
        public async Task Handle(OnlineOrderPaymentSuccessfulDto input)
        {
            // 获取订单信息
            var orderInfo = await context.OrderInfo.AsQueryable().AsNoTracking()
                .Include(i => i.OrderDetails)
                .FirstOrDefaultAsync(x => x.ThirdOrderId == input.PayOrderId);
            if (orderInfo == null)
            {
                logger.LogError(JsonConvert.SerializeObject("支付成功回调：未找到对应的订单信息"));
                return;
            }

            // 检查订单状态
            if (orderInfo.SaleResult == OrderSaleResult.Success || orderInfo.SaleResult == OrderSaleResult.Refund || orderInfo.SaleResult == OrderSaleResult.Cancel)
            {
                logger.LogError(JsonConvert.SerializeObject($"订单已完结，当前状态: {orderInfo.SaleResult}无需操作"));
                return;
            }

            // 检查设备基础信息
            var deviceBaseInfo = await context.DeviceBaseInfo.AsQueryable()
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderInfo.DeviceBaseId);
            if (deviceBaseInfo == null)
            {
                logger.LogError(JsonConvert.SerializeObject($"支付成功回调：未找到 {orderInfo.DeviceBaseId} 设备信息"));
                return;
            }

            // 组装饮品信息
            var parmeters = new List<Ito6216ParmeterDto>();
            orderInfo.OrderDetails.ForEach(item =>
            {
                parmeters.Add(new Ito6216ParmeterDto
                {
                    ServerId = item.ItemCode // 饮品SKU
                });
            });

            // 检查设备是否在线并下发出货指令
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

            // 更新订单信息
            orderInfo.SetSaleResult(OrderSaleResult.Success);
            context.OrderInfo.Update(orderInfo);
        }
    }

    /// <summary>
    /// 下发制作饮品指令 DTO
    /// </summary>
    public class Ito6216SentDto
    {
        /// <summary>
        /// 能力Id
        /// </summary>
        public int CapabilityId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 服务/命令 唯一标识
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<Ito6216ParmeterDto> Parameters { get; set; } = new List<Ito6216ParmeterDto>();
    }

    /// <summary>
    /// 参数 DTO
    /// </summary>
    public class Ito6216ParmeterDto
    {
        /// <summary>
        /// 饮品SKU
        /// </summary>
        public string ServerId { get; set; }
    }
}