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
    /// 创建菜单
    /// </summary>
    /// <param name="repository"></param>
    public class CreateApplicationMenuCommandHandler(IPApplicationMenuRepository repository) : ICommandHandler<CreateApplicationMenuCommand, bool>
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        public async Task<bool> Handle(CreateApplicationMenuCommand request, CancellationToken cancellationToken)
        {
            if (request.parentId != null && request.parentId != 0)
            {
                var parentMenuInfo = await repository.GetAsync(request.parentId.Value);
                if (parentMenuInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0017)]);
                if (parentMenuInfo.MenuType != MenuTypeEnum.Menu && request.menuType == MenuTypeEnum.Btn)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0018)]);
            }
            var info = new ApplicationMenu(request.parentId, request.menuType, request.sysMenuType, request.title, request.name, request.path, request.component, request.rank, request.redirect, request.icon, request.extraIcon, request.enterTransition, request.leaveTransition, request.auths, request.frameSrc, request.frameLoading, request.keepAlive, request.hiddenTag, request.fixedTag, request.showLink, request.showParent, request.remark);
            var res = await repository.AddAsync(info);
            return res != null;
        }
    }
}
