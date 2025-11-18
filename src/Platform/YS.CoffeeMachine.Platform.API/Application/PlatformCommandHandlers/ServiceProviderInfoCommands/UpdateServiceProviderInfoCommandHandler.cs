using YS.CoffeeMachine.Application.PlatformCommands.ServiceProviderInfoCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ServiceProvidersRepository;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ServiceProviderInfoCommands
{
    /// <summary>
    /// 编辑服务商
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateServiceProviderInfoCommandHandler(IPServiceProviderInfoRepository repository) : ICommandHandler<UpdateServiceProviderInfoCommand, bool>
    {
        /// <summary>
        /// 编辑服务商
        /// </summary>
        public async Task<bool> Handle(UpdateServiceProviderInfoCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Update(request.name, request.tel);
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
