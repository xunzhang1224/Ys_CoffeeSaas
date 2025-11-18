using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.ServiceProviders;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IPlatformRepositories.ServiceProvidersRepository
{
    /// <summary>
    /// 服务商信息
    /// </summary>
    public interface IPServiceProviderInfoRepository : IYsRepository<ServiceProviderInfo, long>
    {
    }
}
