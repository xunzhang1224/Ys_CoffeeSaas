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
    /// <param name="context"></param>
    public class ApplicationMenuRepository(CoffeeMachineDbContext context) : YsRepositoryBase<ApplicationMenu, long, CoffeeMachineDbContext>(context), IApplicationMenuRepository
    {
    }
}
