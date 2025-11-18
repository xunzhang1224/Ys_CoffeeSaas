using YS.CoffeeMachine.Application.Commands.ApplicationCommands.RolesCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.RolesCommandHandlers
{
    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteApplicationRoleCommandHandler(IPApplicationRoleRepository repository) : ICommandHandler<DeleteApplicationRoleCommand, bool>
    {
        /// <summary>
        /// 删除角色
        /// </summary>
        public async Task<bool> Handle(DeleteApplicationRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var res = await repository.FakeDeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}
