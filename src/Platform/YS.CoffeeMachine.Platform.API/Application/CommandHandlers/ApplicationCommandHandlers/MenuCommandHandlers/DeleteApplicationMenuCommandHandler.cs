using YS.CoffeeMachine.Application.Commands.ApplicationCommands.MenuCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.MenuCommandHandlers
{
    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteApplicationMenuCommandHandler(IPApplicationMenuRepository repository) : ICommandHandler<DeleteApplicationMenuCommand, bool>
    {
        /// <summary>
        /// 删除菜单
        /// </summary>
        public async Task<bool> Handle(DeleteApplicationMenuCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var res = await repository.DeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}
