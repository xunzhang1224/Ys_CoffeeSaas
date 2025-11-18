using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories.ApplicationUsersRepositories
{
    /// <summary>
    /// 菜单仓储
    /// </summary>
    public class EnterpriseTypesRepository(CoffeeMachineDbContext context) : YsRepositoryBase<EnterpriseTypes, long, CoffeeMachineDbContext>(context), IEnterpriseTypesRepository
    {
    }
}
