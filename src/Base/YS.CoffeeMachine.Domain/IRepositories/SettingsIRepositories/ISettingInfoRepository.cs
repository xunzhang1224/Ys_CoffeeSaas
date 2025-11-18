using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IRepositories.SettingsIRepositories
{
    /// <summary>
    /// 接口样式
    /// </summary>
    public interface ISettingInfoRepository : IYsRepository<SettingInfo, long>
    {
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SettingInfo> GetSettingInfoByIdAsync(long id);
    }
}
