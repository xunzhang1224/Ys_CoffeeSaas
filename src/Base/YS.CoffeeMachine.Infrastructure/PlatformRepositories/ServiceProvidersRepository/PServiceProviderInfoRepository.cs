using YS.CoffeeMachine.Domain.AggregatesModel.ServiceProviders;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ServiceProvidersRepository;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.ServiceProvidersRepository
{
    /// <summary>
    /// 服务商信息
    /// </summary>
    /// <param name="context"></param>
    public class PServiceProviderInfoRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<ServiceProviderInfo, long, CoffeeMachinePlatformDbContext>(context), IPServiceProviderInfoRepository
    {
    }
}
