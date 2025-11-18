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
    /// 编辑菜单
    /// </summary>
    /// <param name="context"></param>
    public class P_UpdateMenuCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<P_UpdateMenuCommand, bool>
    {
        /// <summary>
        /// 编辑菜单
        /// </summary>
        public async Task<bool> Handle(P_UpdateMenuCommand request, CancellationToken cancellationToken)
        {
            var info = await context.ApplicationMenu.FirstOrDefaultAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (request.parentId != null && request.parentId != 0)
            {
                var parentMenuInfo = await context.ApplicationMenu.FirstOrDefaultAsync(w => w.Id == request.parentId.Value);
                if (parentMenuInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0017)]);
                if (parentMenuInfo.MenuType != MenuTypeEnum.Menu && request.menuType == MenuTypeEnum.Btn)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0018)]);
            }
            info.Update(request.parentId, request.menuType, request.sysMenuType, request.title, request.name, request.path, request.component, request.rank, request.redirect, request.icon, request.extraIcon, request.enterTransition, request.leaveTransition, request.auths, request.frameSrc, request.frameLoading, request.keepAlive, request.hiddenTag, request.fixedTag, request.showLink, request.showParent, request.remark,request.activePath);

            return true;
        }
    }
}
