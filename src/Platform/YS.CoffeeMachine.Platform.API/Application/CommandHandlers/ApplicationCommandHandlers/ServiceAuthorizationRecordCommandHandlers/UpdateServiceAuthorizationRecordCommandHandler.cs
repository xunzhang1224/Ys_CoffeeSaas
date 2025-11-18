using YS.CoffeeMachine.Application.Commands.ApplicationCommands.ServiceAuthorizationRecordCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.ServiceAuthorizationRecordCommandHandlers
{
    /// <summary>
    /// 更改服务授权状态
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateServiceAuthorizationRecordCommandHandler(IPServiceAuthorizationRecordRepository repository) : ICommandHandler<UpdateServiceAuthorizationRecordCommand, bool>
    {
        /// <summary>
        /// 更改服务授权状态
        /// </summary>
        public async Task<bool> Handle(UpdateServiceAuthorizationRecordCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (request.state == ServiceAuthorizationStateEnum.Completed || request.state == ServiceAuthorizationStateEnum.Rejected)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0024)]);
            info.Update(request.state);
            var res = repository.UpdateAsync(info);
            return res != null;
        }
    }
}
