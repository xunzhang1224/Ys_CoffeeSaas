using FreeRedis;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.RolesCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.RolesCommandHandlers
{
    /// <summary>
    /// 编辑角色命令处理程序
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    public class UpdateApplicationRoleCommandHandler(CoffeeMachineDbContext context, IRedisClient _redisClient) : ICommandHandler<UpdateApplicationRoleCommand, bool>
    {
        private static string GetBasketUserKey(long userId) => $"/UserMenus/Merchant/Menu{userId}";

        /// <summary>
        /// 编辑角色命令处理程序
        /// </summary>
        public async Task<bool> Handle(UpdateApplicationRoleCommand request, CancellationToken cancellationToken)
        {
            var info = await context.ApplicationRole.Include(i => i.ApplicationRoleMenus).FirstAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (info.IsDefault)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0019)]);
            info.Update(request.name, request.sort, request.remark);
            // 修改角色状态
            if (request.roleStatus != null)
                info.UpdateStatus(request.roleStatus.Value);
            // 修改角色菜单
            if (request.menuIds != null && !request.menuIds.OrderBy(o => o).SequenceEqual(info.ApplicationRoleMenus.Select(s => s.MenuId).OrderBy(o => o)))
            {
                //获取当前角色用户Ids
                var userKeys = await context.ApplicationUserRole.AsNoTracking().Where(w => w.RoleId == info.Id).Select(s => GetBasketUserKey(s.UserId)).ToListAsync();
                info.UpdateRoleMenuIds(request.menuIds.ToList(), request.halfMenuIds);
                if (userKeys.Count > 0)
                {
                    await _redisClient.DelAsync(userKeys.ToArray());
                }
            }
            // 修改所属系统类型
            if (request.sysMenuType != null)
                info.UpdateSysMenuType(request.sysMenuType.Value);
            // 执行修改
            context.Update(info);
            // 提交数据
            return await context.SaveChangesAsync() > 0;
        }
    }
}
