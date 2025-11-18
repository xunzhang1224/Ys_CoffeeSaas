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
    /// 企业信息仓储接口
    /// </summary>
    /// </summary>
    public interface IPEnterpriseTypesRepository : IYsRepository<EnterpriseTypes, long>
    {
    }
}
