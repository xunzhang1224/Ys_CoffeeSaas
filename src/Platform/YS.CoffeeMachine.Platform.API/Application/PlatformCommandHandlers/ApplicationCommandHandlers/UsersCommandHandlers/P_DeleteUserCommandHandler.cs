using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 删除平台用户
    /// </summary>
    /// <param name="repository"></param>
    public class P_DeleteUserCommandHandler(IPApplicationUserRepository repository) : ICommandHandler<P_DeleteUserCommand, bool>
    {
        /// <summary>
        /// 删除平台用户
        /// </summary>
        public async Task<bool> Handle(P_DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var res = await repository.FakeDeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}
