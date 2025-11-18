using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.EnterpriseDevicesCommand;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.EnterpriseDevicesCommandHandler
{
    /// <summary>
    /// 设备分配
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_user"></param>
    public class CreateEnterpriseDevicesCommandHandler(CoffeeMachinePlatformDbContext context, UserHttpContext _user) : ICommandHandler<CreateEnterpriseDevicesCommand, bool>
    {
        /// <summary>
        /// 设备分配
        /// </summary>
        public async Task<bool> Handle(CreateEnterpriseDevicesCommand request, CancellationToken cancellationToken)
        {
            if (request.allocationEnum == DeviceAllocationEnum.Lease && request.recyclingTime == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0066)]);
            var List = new List<EnterpriseDevices>();
            //所属企业为当前设备企业
            foreach (var deviceId in request.deviceIds)
            {
                var deviceInfo = await context.DeviceInfo.Include(i => i.GroupDevices).FirstOrDefaultAsync(w => w.Id == deviceId);
                if (deviceInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
                if (deviceInfo.EnterpriseinfoId != _user.TenantId)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0065)]);
                //更新设备所属企业
                deviceInfo.EnterpriseinfoId = request.enterpriseId;
                //清除设备分组
                deviceInfo.RemoveGroups();
                //设备分配
                List.Add(new EnterpriseDevices(_user.TenantId, deviceId, request.enterpriseId, request.allocationEnum, request.recyclingTime));
            }
            await context.EnterpriseDevices.AddRangeAsync(List);
            return await context.SaveChangesAsync() > 0;
        }
    }
}