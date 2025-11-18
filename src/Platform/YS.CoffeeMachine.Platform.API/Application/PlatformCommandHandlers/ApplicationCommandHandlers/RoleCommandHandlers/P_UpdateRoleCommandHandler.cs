using FreeRedis;
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
    /// 编辑角色命令处理程序
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    public class P_UpdateRoleCommandHandler(CoffeeMachinePlatformDbContext context, IRedisClient _redisClient) : ICommandHandler<P_UpdateRoleCommand, bool>
    {
        private static string GetBasketUserKey(long userId) => $"/UserMenus/Platform/Menu{userId}";

        /// <summary>
        /// 编辑角色命令处理程序
        /// </summary>
        public async Task<bool> Handle(P_UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var exists = await context.ApplicationRole.AsQueryable().AnyAsync(w => w.Name == request.name && w.Id != request.id && w.SysMenuType == SysMenuTypeEnum.Platform);
            if (exists)
                throw ExceptionHelper.AppFriendly("角色名称重复");

            var info = await context.ApplicationRole.Include(i => i.ApplicationRoleMenus).FirstAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Update(request.name, request.sort, request.remark);

            // 修改管理员状态
            if (request.hasSuperAdmin != null)
                info.UpdateSuperAdmin(request.hasSuperAdmin);

            // 修改角色状态
            if (request.roleStatus != null)
                info.UpdateStatus(request.roleStatus.Value);

            // 修改角色菜单
            if (request.menuIds != null && !request.menuIds.OrderBy(o => o).SequenceEqual(info.ApplicationRoleMenus.Select(s => s.MenuId).OrderBy(o => o)))
            {
                //获取当前角色用户Ids
                var userKeys = await context.ApplicationUserRole.AsNoTracking().Where(w => w.RoleId == info.Id).Select(s => GetBasketUserKey(s.UserId)).ToListAsync();

                info.UpdateRoleMenuIds(request.menuIds.ToList(), request.halfMenuIds);

                if (userKeys != null && userKeys.Count > 0)
                {
                    await _redisClient.DelAsync(userKeys.ToArray());
                }
            }

            // 修改所属系统类型
            if (request.sysMenuType != null)
                info.UpdateSysMenuType(request.sysMenuType.Value);

            //提交数据
            //return await context.SaveChangesAsync() > 0;
            return true;
        }
    }
}
