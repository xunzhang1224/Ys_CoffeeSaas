using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.RolesCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.RoleCommandHandlers
{
    /// <summary>
    /// 删除角色命令处理程序
    /// </summary>
    /// <param name="repository"></param>
    public class P_DeleteRoleCommandHandler(IPApplicationRoleRepository repository) : ICommandHandler<P_DeleteRoleCommand, bool>
    {
        /// <summary>
        /// 删除角色命令处理程序
        /// </summary>
        public async Task<bool> Handle(P_DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var res = await repository.FakeDeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}
