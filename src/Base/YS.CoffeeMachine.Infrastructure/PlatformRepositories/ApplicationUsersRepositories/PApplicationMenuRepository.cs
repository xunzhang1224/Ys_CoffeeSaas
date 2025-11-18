using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.ApplicationUsersRepositories
{
    /// <summary>
    /// 应用菜单仓储
    /// </summary>
    /// <param name="context"></param>
    public class PApplicationMenuRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<ApplicationMenu, long, CoffeeMachinePlatformDbContext>(context), IPApplicationMenuRepository
    {
    }
}
