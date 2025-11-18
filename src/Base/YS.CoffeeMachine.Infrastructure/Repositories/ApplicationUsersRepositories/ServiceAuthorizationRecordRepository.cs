using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories.ApplicationUsersRepositories
{
    /// <summary>
    /// 授权记录
    /// </summary>
    /// <param name="context"></param>
    public class ServiceAuthorizationRecordRepository(CoffeeMachineDbContext context) : YsRepositoryBase<ServiceAuthorizationRecord, long, CoffeeMachineDbContext>(context), IServiceAuthorizationRecordRepository
    {
        /// <summary>
        /// 根据账号获取用户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<ApplicationUser?> GetUserByUserAccount(string account)
        {
            return await context.ApplicationUser.FirstOrDefaultAsync(w => w.Account == account);
        }

        /// <summary>
        /// 根据id获取用户
        /// </summary>
        public async Task<ApplicationUser?> GetUserByUserId(long id)
        {
            return await context.ApplicationUser.FirstOrDefaultAsync(w => w.Id == id);
        }

        /// <summary>
        /// 根据用户id获取企业id
        /// </summary>
        public async Task<List<long>> GetEnterpriseByUserIds(List<long> ids)
        {
            return await context.EnterpriseInfo.SelectMany(m => m.Users).Where(w => ids.Contains(w.UserId)).Select(s => s.EnterpriseId).ToListAsync();
        }
    }
}
