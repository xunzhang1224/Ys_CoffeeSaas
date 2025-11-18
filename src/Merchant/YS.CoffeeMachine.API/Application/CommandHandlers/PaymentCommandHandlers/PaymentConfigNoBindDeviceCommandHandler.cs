using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.PaymentCommands;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.PaymentCommandHandlers
{
    /// <summary>
    /// 支付配置没有绑定设置
    /// </summary>
    /// <param name="context"></param>
    public class PaymentConfigNoBindDeviceCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<PaymentConfigNoBindDeviceCommand>
    {
        /// <summary>
        /// 支付配置没有绑定设置
        /// </summary>
        public async Task Handle(PaymentConfigNoBindDeviceCommand request, CancellationToken cancellationToken)
        {
            await context.DevicePaymentConfig.Where(a=>a.PaymentConfigId==request.id && request.deviceIds.Contains(a.DeviceInfoId)).ExecuteDeleteAsync();
        }
    }
}
