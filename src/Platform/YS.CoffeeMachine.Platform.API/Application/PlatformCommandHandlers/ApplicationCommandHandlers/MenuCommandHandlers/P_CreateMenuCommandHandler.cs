using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.MenuCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.MenuCommandHandlers
{
    /// <summary>
    /// 创建菜单
    /// </summary>
    /// <param name="context"></param>
    public class P_CreateMenuCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<P_CreateMenuCommand, bool>
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        public async Task<bool> Handle(P_CreateMenuCommand request, CancellationToken cancellationToken)
        {
            if (request.parentId != null && request.parentId != 0)
            {
                var parentMenuInfo = await context.ApplicationMenu.FirstOrDefaultAsync(w => w.Id == request.parentId.Value);
                if (parentMenuInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0017)]);
                if (parentMenuInfo.MenuType != MenuTypeEnum.Menu && request.menuType == MenuTypeEnum.Btn)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0018)]);
            }
            // 组装菜单信息
            var info = new ApplicationMenu(request.parentId, request.menuType, request.sysMenuType, request.title, request.name, request.path, request.component, request.rank, request.redirect, request.icon, request.extraIcon, request.enterTransition, request.leaveTransition, request.auths, request.frameSrc, request.frameLoading, request.keepAlive, request.hiddenTag, request.fixedTag, request.showLink, request.showParent, request.remark);
            await context.ApplicationMenu.AddAsync(info);

            // 获取所有商户菜单
            var menus = await context.ApplicationMenu.AsQueryable().AsNoTracking().Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant || w.SysMenuType == SysMenuTypeEnum.H5).ToListAsync();
            var allMenuIds = menus.Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant).Select(s => s.Id).ToList();
            var allH5MenuIds = menus.Where(w => w.SysMenuType == SysMenuTypeEnum.H5).Select(s => s.Id).ToList();

            // 包括当前新添加的菜单
            allMenuIds.Add(info.Id);
            allH5MenuIds.Add(info.Id);

            // 获取所有一级企业
            var allEnterprises = await context.EnterpriseInfo.AsNoTracking().Where(w => w.Pid == null).ToListAsync();

            // 更新全部一级企业用于菜单Ids
            foreach (var item in allEnterprises)
            {
                item.UpdateMenuIds(allMenuIds);
                item.UpdateH5MenuIds(allH5MenuIds);
            }
            context.UpdateRange(allEnterprises);
            return true;
        }
    }
}