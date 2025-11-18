using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.IPlatformRepositories.BeverageWarehouseRepositorys;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.BeverageWarehouseRepositorys
{
    /// <summary>
    /// 饮料信息模板仓储
    /// </summary>
    /// <param name="context"></param>
    public class PBeverageInfoTemplateRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<BeverageInfoTemplate, long, CoffeeMachinePlatformDbContext>(context), IPBeverageInfoTemplateRepository
    {
    }
}
