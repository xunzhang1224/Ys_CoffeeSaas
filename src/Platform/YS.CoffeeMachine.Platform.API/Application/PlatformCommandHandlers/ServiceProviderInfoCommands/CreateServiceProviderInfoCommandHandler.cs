using YS.CoffeeMachine.Application.PlatformCommands.ServiceProviderInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.ServiceProviders;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ServiceProvidersRepository;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ServiceProviderInfoCommands
{
    /// <summary>
    /// 添加服务商
    /// </summary>
    /// <param name="repository"></param>
    public class CreateServiceProviderInfoCommandHandler(IPServiceProviderInfoRepository repository) : ICommandHandler<CreateServiceProviderInfoCommand, bool>
    {
        /// <summary>
        /// 添加服务商
        /// </summary>
        public async Task<bool> Handle(CreateServiceProviderInfoCommand request, CancellationToken cancellationToken)
        {
            var info = new ServiceProviderInfo(request.name, request.tel);
            var res = repository.AddAsync(info);
            return res != null;
        }
    }
}
