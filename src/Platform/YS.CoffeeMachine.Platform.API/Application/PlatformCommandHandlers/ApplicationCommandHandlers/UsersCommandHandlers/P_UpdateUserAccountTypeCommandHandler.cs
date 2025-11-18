using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 更新用户账户类型
    /// </summary>
    /// <param name="repository"></param>
    public class P_UpdateUserAccountTypeCommandHandler(IPApplicationUserRepository repository) : ICommandHandler<P_UpdateUserAccountTypeCommand, bool>
    {
        /// <summary>
        /// 更新用户账户类型
        /// </summary>
        public async Task<bool> Handle(P_UpdateUserAccountTypeCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);
            info.UpdateAccountType(request.accountType);
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
