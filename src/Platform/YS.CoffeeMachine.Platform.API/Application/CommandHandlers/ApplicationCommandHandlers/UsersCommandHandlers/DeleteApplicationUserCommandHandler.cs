using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteApplicationUserCommandHandler(IPApplicationUserRepository repository) : ICommandHandler<DeleteApplicationUserCommand, bool>
    {
        /// <summary>
        /// 删除用户
        /// </summary>
        public async Task<bool> Handle(DeleteApplicationUserCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);

            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            if (info.IsDefault)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0028)]);

            var res = await repository.FakeDeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}