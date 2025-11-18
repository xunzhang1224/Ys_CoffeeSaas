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
    /// 分组绑定设备
    /// </summary>
    /// <param name="context"></param>
    public class BindDevicesCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<BindDevicesCommand, bool>
    {
        /// <summary>
        /// 分组绑定设备
        /// </summary>
        public async Task<bool> Handle(BindDevicesCommand request, CancellationToken cancellationToken)
        {
            var info = await context.Groups.Include(i => i.Devices).ThenInclude(i => i.DeviceInfo).FirstOrDefaultAsync(w => w.Id == request.groupId);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var newDeviceIds = info.Devices.Select(s => s.DeviceInfoId).ToList();
            newDeviceIds.AddRange(newDeviceIds);
            newDeviceIds.AddRange(request.deviceIds);
            info.BindDevices(newDeviceIds.Distinct().ToList());
            var res = await context.SaveChangesAsync();
            return res > 0;
        }
    }
}
