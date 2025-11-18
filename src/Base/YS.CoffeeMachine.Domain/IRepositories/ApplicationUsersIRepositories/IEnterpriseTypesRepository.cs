using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories
{
    /// <summary>
    /// 企业类型
    /// </summary>
    public interface IEnterpriseTypesRepository : IYsRepository<EnterpriseTypes, long>
    {
    }
}
