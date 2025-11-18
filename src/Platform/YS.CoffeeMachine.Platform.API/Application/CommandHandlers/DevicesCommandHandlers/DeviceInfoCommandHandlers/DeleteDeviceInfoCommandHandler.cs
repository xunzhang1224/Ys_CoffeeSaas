using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 批量删除设备
    /// </summary>
    /// <param name="context"></param>
    public class DeleteDeviceInfoCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<DeleteDeviceInfoCommand, bool>
    {
        /// <summary>
        /// 批量删除设备
        /// </summary>
        public async Task<bool> Handle(DeleteDeviceInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.ids.Count <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var devices = await context.DeviceInfo.AsNoTracking().Where(w => request.ids.Contains(w.Id)).ToListAsync();
            devices.ForEach(e => e.IsDelete = true);
            context.UpdateRange(devices);
            return true;
        }
    }
}
