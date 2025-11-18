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
    /// 企业信息仓储
    /// </summary>
    public interface IPEnterpriseInfoRepository : IYsRepository<EnterpriseInfo, long>
    {
        /// <summary>
        /// 根据Id获取企业信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EnterpriseInfo> GetByIdAsync(long id);

        /// <summary>
        /// 获取所有企业信息
        /// </summary>
        /// <returns></returns>
        Task<List<EnterpriseInfo>> GetAllAsync();
    }
}
