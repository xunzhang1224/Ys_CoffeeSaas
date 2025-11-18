using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceUserAssociationCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceUserAssociationCommandHanders
{
    /// <summary>
    /// 设备用户多对多绑定
    /// </summary>
    /// <param name="context"></param>
    public class BindDeviceUserAssociationCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<BindDeviceUserAssociationCommand, bool>
    {
        /// <summary>
        /// 设备用户多对多绑定
        /// </summary>
        public async Task<bool> Handle(BindDeviceUserAssociationCommand request, CancellationToken cancellationToken)
        {
            var deviceInfos = await context.DeviceInfo.Include(i => i.DeviceUserAssociations).Where(w => request.deviceIds.Contains(w.Id)).ToListAsync();
            if (deviceInfos.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            deviceInfos.ForEach(e => { e.BindUsers(request.userIds); });
            return true;
        }
    }
}
