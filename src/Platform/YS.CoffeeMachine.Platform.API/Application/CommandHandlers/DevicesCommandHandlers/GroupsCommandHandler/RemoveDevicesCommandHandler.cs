using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.GroupsCommandHandler
{
    /// <summary>
    /// 移除分组设备
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class RemoveDevicesCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<RemoveDevicesCommand, bool>
    {
        /// <summary>
        /// 移除分组设备
        /// </summary>
        public async Task<bool> Handle(RemoveDevicesCommand request, CancellationToken cancellationToken)
        {
            var info = await context.Groups.Include(i => i.Devices).ThenInclude(i => i.DeviceInfo).FirstOrDefaultAsync(w => w.Id == request.groupId);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.RemoveDevices(request.deviceIds);
            var res = await context.SaveChangesAsync();
            return res > 0;
        }
    }
}
