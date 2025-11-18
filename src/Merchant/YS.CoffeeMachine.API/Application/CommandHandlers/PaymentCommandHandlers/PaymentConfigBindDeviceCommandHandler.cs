using YS.CoffeeMachine.Application.Commands.PaymentCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Payment;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.PaymentCommandHandlers
{
    /// <summary>
    /// 支付配置绑定设备
    /// </summary>
    /// <param name="context"></param>
    public class PaymentConfigBindDeviceCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<PaymentConfigBindDeviceCommand>
    {
        /// <summary>
        /// 支付配置绑定设备
        /// </summary>
        public async Task Handle(PaymentConfigBindDeviceCommand request, CancellationToken cancellationToken)
        {
            var infos = new List<DevicePaymentConfig>();
            foreach (var item in request.deviceIds)
            {
                infos.Add(new DevicePaymentConfig(request.id, item));
            }
            await context.AddRangeAsync(infos);
        }
    }
}
