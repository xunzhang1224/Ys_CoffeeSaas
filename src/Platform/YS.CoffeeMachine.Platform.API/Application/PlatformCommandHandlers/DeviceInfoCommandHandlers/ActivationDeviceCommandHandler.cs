using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// ActivationDevice
    /// </summary>
    /// <param name="context"></param>
    public class ActivationDeviceCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<ActivationDeviceCommand>
    {
        /// <summary>
        /// ActivationDevice
        /// </summary>
        public async Task Handle(ActivationDeviceCommand request, CancellationToken cancellationToken)
        {
            var deviceBaseInfo = await context.DeviceBaseInfo.AsQueryable().Where(a => a.Id == request.deviceBaseInfoId).FirstOrDefaultAsync();
            if (deviceBaseInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);

            var deviceInfo = new DeviceInfo(request.enterpriseinfoId, request.deviceBaseInfoId, request.deviceName, deviceBaseInfo.MachineStickerCode);
            await context.DeviceInfo.AddAsync(deviceInfo);
        }
    }
}
