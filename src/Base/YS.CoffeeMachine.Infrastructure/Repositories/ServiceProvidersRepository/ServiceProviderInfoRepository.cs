using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.ServiceProviders;
using YS.CoffeeMachine.Domain.IRepositories.ServiceProvidersRepository;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories.ServiceProvidersRepository
{
    /// <summary>
    /// 服务商信息仓储
    /// </summary>
    /// <param name="context"></param>
    public class ServiceProviderInfoRepository(CoffeeMachineDbContext context) : YsRepositoryBase<ServiceProviderInfo, long, CoffeeMachineDbContext>(context), IServiceProviderInfoRepository
    {
    }
}
