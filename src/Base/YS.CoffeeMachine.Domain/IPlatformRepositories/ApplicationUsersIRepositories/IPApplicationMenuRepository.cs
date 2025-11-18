using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories
{
    /// <summary>
    /// 应用菜单仓储
    /// </summary>
    public interface IPApplicationMenuRepository : IYsRepository<ApplicationMenu, long>
    {
    }
}
