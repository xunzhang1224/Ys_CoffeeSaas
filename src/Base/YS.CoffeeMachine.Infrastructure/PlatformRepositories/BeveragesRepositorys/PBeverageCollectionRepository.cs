using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.IPlatformRepositories.BeveragesRepositorys;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.BeveragesRepositorys
{
    /// <summary>
    /// 聚合根仓储
    /// </summary>
    /// <param name="context"></param>
    public class PBeverageCollectionRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<BeverageCollection, long, CoffeeMachinePlatformDbContext>(context), IPBeverageCollectionRepository
    {
    }
}
