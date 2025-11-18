using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.IRepositories.BeveragesRepositorys;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories.BeveragesRepositorys
{
    /// <summary>
    /// 饮料集合仓库
    /// </summary>
    /// <param name="context"></param>
    public class BeverageCollectionRepository(CoffeeMachineDbContext context) : YsRepositoryBase<BeverageCollection, long, CoffeeMachineDbContext>(context), IBeverageCollectionRepository
    {
    }
}
