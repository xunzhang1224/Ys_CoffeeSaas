using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories
{
    /// <summary>
    /// 设备
    /// </summary>
    public interface IDeviceRepository : IYsRepository<Groups>
    {
    }
}
