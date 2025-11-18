using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories.ApplicationUsersRepositories
{
    /// <summary>
    /// 企业信息
    /// </summary>
    /// <param name="context"></param>
    public class EnterpriseInfoRepository(CoffeeMachineDbContext context) : YsRepositoryBase<EnterpriseInfo, long, CoffeeMachineDbContext>(context), IEnterpriseInfoRepository
    {
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EnterpriseInfo> GetByIdAsync(long id)
        {
            return await context.EnterpriseInfo.Include(i => i.Users).ThenInclude(i => i.User).ThenInclude(i => i.ApplicationUserRoles).Include(i => i.Roles).FirstAsync(w => w.Id == id);
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<EnterpriseInfo>> GetAllAsync()
        {
            return await context.EnterpriseInfo.AsNoTracking().ToListAsync();
        }
    }
}
