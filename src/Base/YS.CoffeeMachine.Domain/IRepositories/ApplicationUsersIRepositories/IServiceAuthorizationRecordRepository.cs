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
    public interface IServiceAuthorizationRecordRepository : IYsRepository<ServiceAuthorizationRecord, long>
    {
        /// <summary>
        /// 根据用户账号获取用户信息
        /// </summary>
        Task<ApplicationUser?> GetUserByUserAccount(string account);

        /// <summary>
        /// 根据用户id获取用户信息
        /// </summary>
        Task<ApplicationUser?> GetUserByUserId(long id);

        /// <summary>
        /// 根据用户id获取用户所属企业id
        /// </summary>
        Task<List<long>> GetEnterpriseByUserIds(List<long> ids);
    }
}
