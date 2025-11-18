using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.IRepositories.BeverageWarehouseRepositorys;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories.BeverageWarehouseRepositorys
{
    /// <summary>
    /// 饮料信息模板仓储
    /// </summary>
    /// <param name="context"></param>
    public class BeverageInfoTemplateRepository(CoffeeMachineDbContext context) : YsRepositoryBase<BeverageInfoTemplate, long, CoffeeMachineDbContext>(context), IBeverageInfoTemplateRepository
    {
    }
}
