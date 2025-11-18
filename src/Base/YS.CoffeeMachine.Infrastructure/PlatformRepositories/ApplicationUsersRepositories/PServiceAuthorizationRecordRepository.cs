using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.ApplicationUsersRepositories
{
    /// <summary>
    /// 登录授权记录
    /// </summary>
    /// <param name="context"></param>
    public class PServiceAuthorizationRecordRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<ServiceAuthorizationRecord, long, CoffeeMachinePlatformDbContext>(context), IPServiceAuthorizationRecordRepository
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
        /// 根据用户Id获取用户
        /// </summary>
        public async Task<ApplicationUser?> GetUserByUserId(long id)
        {
            return await context.ApplicationUser.FirstOrDefaultAsync(w => w.Id == id);
        }

        /// <summary>
        /// 获取用户授权的企业Id
        /// </summary>
        public async Task<List<long>> GetEnterpriseByUserIds(List<long> ids)
        {
            return await context.EnterpriseInfo.SelectMany(m => m.Users).Where(w => ids.Contains(w.UserId)).Select(s => s.EnterpriseId).ToListAsync();
        }
    }
}
