using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.BindDeviceServiceProvidersCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.BindDeviceServiceProvidersCommandHandlers
{
    /// <summary>
    /// 设备绑定服务商
    /// </summary>
    /// <param name="context"></param>
    public class BindDeviceServiceProvidersCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<BindDeviceServiceProvidersCommand, bool>
    {
        /// <summary>
        /// 设备绑定服务商
        /// </summary>
        public async Task<bool> Handle(BindDeviceServiceProvidersCommand request, CancellationToken cancellationToken)
        {
            var deviceInfo = await context.DeviceInfo.Include(i => i.DeviceServiceProviders).FirstAsync(w => w.Id == request.deviceId);
            if (deviceInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            deviceInfo.BindServiceProviders(request.spIds);
            var res = context.Update(deviceInfo);
            return res != null;
        }
    }
}
