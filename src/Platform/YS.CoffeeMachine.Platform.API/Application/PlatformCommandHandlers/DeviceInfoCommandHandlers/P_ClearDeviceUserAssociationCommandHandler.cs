using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 设备企业/用户解绑
    /// </summary>
    /// <param name="context"></param>
    public class P_ClearDeviceUserAssociationCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<P_ClearDeviceUserAssociationCommand, bool>
    {
        /// <summary>
        /// 设备企业/用户解绑
        /// </summary>
        public async Task<bool> Handle(P_ClearDeviceUserAssociationCommand request, CancellationToken cancellationToken)
        {
            var deviceInfo = await context.DeviceInfo.Include(i => i.GroupDevices).Include(i => i.DeviceUserAssociations).FirstAsync(w => w.Id == request.deviceId);
            if (deviceInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);

            //清除设备绑定的企业
            deviceInfo.EnterpriseinfoId = -1;

            //删除设备分组
            deviceInfo.RemoveGroups();

            //获取当前设备原分配信息
            var curDeviceAllocationList = await context.EnterpriseDevices.Where(w => w.DeviceId == request.deviceId).ToListAsync();
            //清除当前设备原分配信息
            if (curDeviceAllocationList.Count > 0)
                context.EnterpriseDevices.RemoveRange(curDeviceAllocationList);

            //如果设备没有绑定用户则不需要解绑用户
            if (deviceInfo.DeviceUserAssociations == null)
                return true;
            //清除绑定的用户
            deviceInfo.RemoveUsers();
            return true;
        }
    }
}
