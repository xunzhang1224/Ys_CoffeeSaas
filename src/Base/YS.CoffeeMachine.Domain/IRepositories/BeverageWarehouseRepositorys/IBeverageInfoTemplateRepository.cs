using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IRepositories.BeverageWarehouseRepositorys
{
    /// <summary>
    /// 饮料信息模板仓储
    /// </summary>
    public interface IBeverageInfoTemplateRepository : IYsRepository<BeverageInfoTemplate, long>
    {
    }
}
