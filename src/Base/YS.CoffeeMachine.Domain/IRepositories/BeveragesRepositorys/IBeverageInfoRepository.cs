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
    /// 聚合根仓储接口
    /// </summary>
    /// </summary>
    public interface IBeverageInfoRepository : IYsRepository<BeverageInfo, long>
    {
    }
}
