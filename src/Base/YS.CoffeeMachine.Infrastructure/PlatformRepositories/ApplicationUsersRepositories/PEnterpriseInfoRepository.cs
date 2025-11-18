using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.ApplicationUsersRepositories
{
    /// <summary>
    /// 企业信息
    /// </summary>
    /// <param name="context"></param>
    public class PEnterpriseInfoRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<EnterpriseInfo, long, CoffeeMachinePlatformDbContext>(context), IPEnterpriseInfoRepository
    {
        /// <summary>
        /// 根据id获取企业信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EnterpriseInfo> GetByIdAsync(long id)
        {
            return await context.EnterpriseInfo.Include(i => i.Users).Include(i => i.Roles).FirstAsync(w => w.Id == id);
        }

        /// <summary>
        /// 获取所有企业信息
        /// </summary>
        public async Task<List<EnterpriseInfo>> GetAllAsync()
        {
            return await context.EnterpriseInfo.AsNoTracking().ToListAsync();
        }
    }
}
