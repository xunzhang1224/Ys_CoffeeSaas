using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.EnterpriseDevicesCommand;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.EnterpriseDevicesCommandHandler
{
    /// <summary>
    /// 编辑设备分配
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class UpdateEnterpriseDevicesCommandHandler(IEnterpriseDevicesRepository repository, CoffeeMachineDbContext context) : ICommandHandler<UpdateEnterpriseDevicesCommand, bool>
    {
        /// <summary>
        /// 编辑设备分配
        /// </summary>
        public async Task<bool> Handle(UpdateEnterpriseDevicesCommand request, CancellationToken cancellationToken)
        {
            if (request.allocationEnum == DeviceAllocationEnum.Lease && request.recyclingTime == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0066)]);
            var info = await repository.GetAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            //var curDevice = await context.DeviceInfo.Include(i => i.GroupDevices).Include(i => i.DeviceUserAssociations).Where(w => w.Id == info.DeviceId).FirstOrDefaultAsync();
            //if (curDevice == null)
            //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            ////删除设备分组
            //curDevice.RemoveGroups();
            ////删除关联用户
            //curDevice.RemoveUsers();
            //var allocateTime = request.enterpriseId != info.EnterpriseId ? DateTime.UtcNow : info.AllocateTime;
            //重新分配设备
            info.Update(request.enterpriseId, request.allocationEnum, request.recyclingTime, null);
            var res = repository.UpdateAsync(info);
            return res != null;
        }
    }
}
