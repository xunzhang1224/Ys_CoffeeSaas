using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.IRepositories.SettingsIRepositories;
using YSCore.Provider.EntityFrameworkCore;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Infrastructure.Repositories.SettingsRepositories
{
    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="context"></param>
    public class SettingInfoRepository(CoffeeMachineDbContext context) : YsRepositoryBase<SettingInfo, long, CoffeeMachineDbContext>(context), ISettingInfoRepository
    {
        /// <summary>
        /// 通过Id获取设置信息
        /// </summary>
        public async Task<SettingInfo> GetSettingInfoByIdAsync(long id)
        {
            return await context.SettingInfo
                //.AsNoTracking()
                .Include(i => i.MaterialBoxs).FirstAsync(w => w.Id == id);
        }
    }
}
