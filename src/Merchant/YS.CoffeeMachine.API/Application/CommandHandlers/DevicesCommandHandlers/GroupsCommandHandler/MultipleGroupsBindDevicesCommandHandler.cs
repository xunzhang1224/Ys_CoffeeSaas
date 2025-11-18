using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.GroupsCommandHandler
{
    /// <summary>
    /// 分组与设备多对多绑定
    /// </summary>
    /// <param name="context"></param>
    public class MultipleGroupsBindDevicesCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<MultipleGroupsBindDevicesCommand, bool>
    {
        /// <summary>
        /// 分组与设备多对多绑定
        /// </summary>
        public async Task<bool> Handle(MultipleGroupsBindDevicesCommand request, CancellationToken cancellationToken)
        {
            var groups = await context.Groups.Include(i => i.Devices).Where(w => request.groupIds.Contains(w.Id)).ToListAsync();
            if (groups.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);
            foreach (var device in groups)
            {
                var newDeviceIds = device.Devices.Select(s => s.DeviceInfoId).ToList();
                newDeviceIds.AddRange(newDeviceIds);
                newDeviceIds.AddRange(request.deviceIds);
                device.BindDevices(newDeviceIds.Distinct().ToList());
            }
            return true;
        }
    }
}
