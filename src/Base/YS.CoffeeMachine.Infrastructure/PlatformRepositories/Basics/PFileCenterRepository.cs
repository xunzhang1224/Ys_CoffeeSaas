using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.IPlatformRepositories.Basics;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.Basics
{
    /// <summary>
    /// 文件存储
    /// </summary>
    /// <param name="context"></param>
    public class PFileCenterRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<FileCenter, CoffeeMachinePlatformDbContext>(context), IPFileCenterRepository
    {
    }
}
