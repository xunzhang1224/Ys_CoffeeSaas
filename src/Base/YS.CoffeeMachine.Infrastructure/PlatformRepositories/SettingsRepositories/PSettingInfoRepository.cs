using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.IPlatformRepositories.SettingsIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.SettingsRepositories
{
    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="context"></param>
    public class PSettingInfoRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<SettingInfo, long, CoffeeMachinePlatformDbContext>(context), IPSettingInfoRepository
    {
        /// <summary>
        /// 根据id获取设置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SettingInfo> GetSettingInfoByIdAsync(long id)
        {
            return await context.SettingInfo
                //.AsNoTracking()
                .Include(i => i.MaterialBoxs).FirstAsync(w => w.Id == id);
        }
    }
}
