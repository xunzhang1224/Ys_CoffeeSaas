using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.IRepositories.SettingsIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories.SettingsRepositories
{
    /// <summary>
    /// 界面样式
    /// </summary>
    public class TimeZoneInfosRepository(CoffeeMachineDbContext context) : YsRepositoryBase<TimeZoneInfos, long, CoffeeMachineDbContext>(context), ITimeZoneInfosRepository
    {
    }
}
