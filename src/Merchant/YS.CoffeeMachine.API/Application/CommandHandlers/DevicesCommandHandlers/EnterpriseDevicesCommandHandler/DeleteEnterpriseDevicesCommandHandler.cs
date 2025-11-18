using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.EnterpriseDevicesCommand;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.EnterpriseDevicesCommandHandler
{
    /// <summary>
    /// 取消企业设备分配、并删除分配记录
    /// </summary>
    /// <param name="context"></param>
    public class DeleteEnterpriseDevicesCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<DeleteEnterpriseDevicesCommand, bool>
    {
        /// <summary>
        /// 取消企业设备分配、并删除分配记录
        /// </summary>
        public async Task<bool> Handle(DeleteEnterpriseDevicesCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            // 获取分配信息
            var info = context.EnterpriseDevices.IgnoreQueryFilters().FirstOrDefault(x => !x.IsDelete && x.Id == request.id);
            if (info is null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            // 只有租赁设备才能取消分配
            if (info.DeviceAllocationType != DeviceAllocationEnum.Lease)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0067)]);
            //获取设备信息
            var deviceInfo = await context.DeviceInfo.IgnoreQueryFilters().Include(i => i.GroupDevices).FirstOrDefaultAsync(w => !w.IsDelete && w.Id == info.DeviceId);
            if (deviceInfo is null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);

            // 将设备所属企业变更为目标企业
            // deviceInfo.EnterpriseinfoId = info.BelongingEnterpriseId;
            deviceInfo.EnterpriseinfoId = info.EnterpriseinfoId;

            //清除设备分组
            deviceInfo.RemoveGroups();
            info.IsDelete = true;
            //提交更改数据
            return await context.SaveChangesAsync() > 0;
        }
    }
}