using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 更新用户状态
    /// </summary>
    /// <param name="repository"></param>
    public class P_UpdateUserStatusCommandHandler(IPApplicationUserRepository repository) : ICommandHandler<P_UpdateUserStatusCommand, bool>
    {
        /// <summary>
        /// 更新用户状态
        /// </summary>
        public async Task<bool> Handle(P_UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);
            info.UpdateStatus(request.status);
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
