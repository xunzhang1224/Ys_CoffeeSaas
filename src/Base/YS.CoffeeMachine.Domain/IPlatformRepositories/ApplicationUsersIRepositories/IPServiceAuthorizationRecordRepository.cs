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
    public interface IPServiceAuthorizationRecordRepository : IYsRepository<ServiceAuthorizationRecord, long>
    {
        /// <summary>
        /// 通过用户账号获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<ApplicationUser?> GetUserByUserAccount(string account);

        /// <summary>
        /// 通过用户Id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationUser?> GetUserByUserId(long id);

        /// <summary>
        /// 通过用户Id获取用户所属企业Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<long>> GetEnterpriseByUserIds(List<long> ids);
    }
}
