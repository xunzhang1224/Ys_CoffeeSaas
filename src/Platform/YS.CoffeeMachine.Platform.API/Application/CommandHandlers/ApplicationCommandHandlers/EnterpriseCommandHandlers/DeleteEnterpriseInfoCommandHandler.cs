using YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 删除企业信息
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteEnterpriseInfoCommandHandler(IEnterpriseInfoRepository repository) : ICommandHandler<DeleteEnterpriseInfoCommand, bool>
    {
        /// <summary>
        /// 删除企业信息
        /// </summary>
        public async Task<bool> Handle(DeleteEnterpriseInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var res = await repository.FakeDeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}
