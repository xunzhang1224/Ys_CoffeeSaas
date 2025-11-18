using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.IPlatformRepositories.SettingsIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.SettingsRepositories
{
    /// <summary>
    /// 接口样式
    /// </summary>
    /// <param name="context"></param>
    public class PInterfaceStylesRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<InterfaceStyles, long, CoffeeMachinePlatformDbContext>(context), IPInterfaceStylesRepository
    {
    }
}
