using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IPlatformRepositories.SettingsIRepositories
{
    /// <summary>
    /// 接口样式仓储接口
    /// </summary>
    /// </summary>
    public interface IPSettingInfoRepository : IYsRepository<SettingInfo, long>
    {
        /// <summary>
        /// 根据id获取设置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SettingInfo> GetSettingInfoByIdAsync(long id);
    }
}
