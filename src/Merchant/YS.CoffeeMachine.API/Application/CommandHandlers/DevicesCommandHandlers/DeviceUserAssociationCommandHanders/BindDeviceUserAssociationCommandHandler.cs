using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceUserAssociationCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceUserAssociationCommandHanders
{
    /// <summary>
    /// 设备用户多对多绑定
    /// </summary>
    /// <param name="context"></param>
    public class BindDeviceUserAssociationCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<BindDeviceUserAssociationCommand, bool>
    {
        /// <summary>
        /// 设备用户多对多绑定
        /// </summary>
        public async Task<bool> Handle(BindDeviceUserAssociationCommand request, CancellationToken cancellationToken)
        {
            if (request.enterpriseId == null)
            {
                var deviceInfos = await context.DeviceInfo.Include(i => i.DeviceUserAssociations).Where(w => request.deviceIds.Contains(w.Id)).ToListAsync();
                if (deviceInfos.Count == 0)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
                deviceInfos.ForEach(e => { e.BindUsers(request.userIds); });
                return true;
            }
            else
            {
                if (request.deviceIds.Count == 0)
                {
                    var userIds = await context.ApplicationUser.AsQueryable().IgnoreQueryFilters().Where(w => request.userIds.Contains(w.Id) && w.EnterpriseId == request.enterpriseId && !w.IsDelete).Select(s => s.Id).ToListAsync();
                    await context.DeviceUserAssociation.Where(w => userIds.Contains(w.UserId)).ExecuteDeleteAsync();
                    return true;
                }
                else
                {
                    var deviceInfos = await context.DeviceInfo.IgnoreQueryFilters().Include(i => i.DeviceUserAssociations)
                      .Where(w => request.deviceIds.Contains(w.Id) && w.EnterpriseinfoId == request.enterpriseId && !w.IsDelete).ToListAsync();
                    if (deviceInfos.Count == 0)
                        throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
                    deviceInfos.ForEach(e => { e.BindUsers(request.userIds); });
                    return true;
                }

            }

        }
    }
}