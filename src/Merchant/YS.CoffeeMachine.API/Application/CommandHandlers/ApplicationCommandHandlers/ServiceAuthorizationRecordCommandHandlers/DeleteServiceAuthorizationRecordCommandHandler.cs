using YS.CoffeeMachine.Application.Commands.ApplicationCommands.ServiceAuthorizationRecordCommands;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.ServiceAuthorizationRecordCommandHandlers
{
    /// <summary>
    /// 删除服务授权记录
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteServiceAuthorizationRecordCommandHandler(IServiceAuthorizationRecordRepository repository) : ICommandHandler<DeleteServiceAuthorizationRecordCommand, bool>
    {
        /// <summary>
        /// 删除服务授权记录
        /// </summary>
        public async Task<bool> Handle(DeleteServiceAuthorizationRecordCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var res = await repository.FakeDeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}
