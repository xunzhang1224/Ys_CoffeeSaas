using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IPlatformRepositories.BeverageWarehouseRepositorys
{
    /// <summary>
    /// 饮料信息模板仓库接口
    /// </summary>
    public interface IPBeverageInfoTemplateRepository : IYsRepository<BeverageInfoTemplate, long>
    {
    }
}
