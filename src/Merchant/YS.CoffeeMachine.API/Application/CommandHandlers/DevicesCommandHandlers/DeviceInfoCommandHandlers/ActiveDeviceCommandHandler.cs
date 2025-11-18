using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 设备激活
    /// </summary>
    /// <param name="_context"></param>
    public class ActiveDeviceCommandHandler(CoffeeMachineDbContext _context) : ICommandHandler<ActiveDeviceCommand>
    {
        /// <summary>
        /// 设备激活
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(ActiveDeviceCommand request, CancellationToken cancellationToken)
        {
            var info = await _context.DeviceInfo.AsQueryable().Where(a => a.Id == request.deviceId).FirstOrDefaultAsync();
            if (info.ActiveTime != null)
            {
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0090)]);
            }

            info.ActiveDevice();
        }
    }
}
