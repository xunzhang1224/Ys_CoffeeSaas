using YS.CoffeeMachine.Application.PlatformCommands.ServiceProviderInfoCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ServiceProvidersRepository;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ServiceProviderInfoCommands
{
    /// <summary>
    /// 删除服务商
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteServiceProviderInfoCommandHandler(IPServiceProviderInfoRepository repository) : ICommandHandler<DeleteServiceProviderInfoCommand, bool>
    {
        /// <summary>
        /// 删除服务商
        /// </summary>
        public async Task<bool> Handle(DeleteServiceProviderInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var res = await repository.FakeDeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}
