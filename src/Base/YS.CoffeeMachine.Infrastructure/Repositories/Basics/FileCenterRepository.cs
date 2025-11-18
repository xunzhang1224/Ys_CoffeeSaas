using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.IRepositories.Basics;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories.Basics
{
    /// <summary>
    /// 文件存储
    /// </summary>
    /// <param name="context"></param>
    public class FileCenterRepository(CoffeeMachineDbContext context) : YsRepositoryBase<FileCenter, CoffeeMachineDbContext>(context), IFileCenterRepository
    {

    }
}
