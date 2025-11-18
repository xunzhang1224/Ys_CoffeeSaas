using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.RolesCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.RoleCommandHandlers
{
    /// <summary>
    /// 创建角色命令处理程序
    /// </summary>
    /// <param name="context"></param>
    public class P_CreateRoleCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<P_CreateRoleCommand, bool>
    {
        /// <summary>
        /// 创建角色命令处理程序
        /// </summary>
        public async Task<bool> Handle(P_CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var roleStatus = request.RoleStatus == null ? RoleStatusEnum.Enable : request.RoleStatus.Value;
            // 验证角色名称是否存在
            var isExist = await context.ApplicationRole.AnyAsync(x => x.IsDefault && x.Name == request.name && x.SysMenuType == request.sysMenuType);
            if (isExist)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.C0012)], request.name));
            //添加角色信息
            var applicationRole = new ApplicationRole(request.name, roleStatus, request.sysMenuType, request.hasSuperAdmin, request.sort, request.remark, request.menuIds, true);
            await context.AddAsync(applicationRole);
            return true;
        }
    }
}