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
    /// 接口样式
    /// </summary>
    public interface IPInterfaceStylesRepository : IYsRepository<InterfaceStyles, long>
    {
    }
}
