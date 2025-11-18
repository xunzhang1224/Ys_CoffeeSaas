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
    /// 时间区域信息仓库
    /// </summary>
    public interface ITimeZoneInfosRepository : IYsRepository<TimeZoneInfos, long>
    {
    }
}
