using YS.CoffeeMachine.Application.Commands.ApplicationCommands.MenuCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.MenuCommandHandlers
{
    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateApplicationMenuCommandHandler(IPApplicationMenuRepository repository) : ICommandHandler<UpdateApplicationMenuCommand, bool>
    {
        /// <summary>
        /// 更新菜单
        /// </summary>
        public async Task<bool> Handle(UpdateApplicationMenuCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (request.parentId != null && request.parentId != 0)
            {
                var parentMenuInfo = await repository.GetAsync(request.parentId.Value);
                if (parentMenuInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0017)]);
                if (parentMenuInfo.MenuType != MenuTypeEnum.Menu && request.menuType == MenuTypeEnum.Btn)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0018)]);
            }
            info.Update(request.parentId, request.menuType, request.sysMenuType, request.title, request.name, request.path, request.component, request.rank, request.redirect, request.icon, request.extraIcon, request.enterTransition, request.leaveTransition, request.auths, request.frameSrc, request.frameLoading, request.keepAlive, request.hiddenTag, request.fixedTag, request.showLink, request.showParent, request.remark, request.activePath);
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
