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
    /// 企业设备
    /// </summary>
    public interface IEnterpriseDevicesRepository : IYsRepository<EnterpriseDevices, long>
    {
    }
}
