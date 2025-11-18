using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IRepositories.BeveragesRepositorys
{
    /// <summary>
    /// 饮料集合仓库
    /// </summary>
    public interface IBeverageCollectionRepository : IYsRepository<BeverageCollection, long>
    {
    }
}
