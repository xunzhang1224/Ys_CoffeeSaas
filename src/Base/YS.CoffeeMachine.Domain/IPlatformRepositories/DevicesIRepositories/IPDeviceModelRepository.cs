using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IPlatformRepositories.DevicesIRepositories
{
    /// <summary>
    /// 设备型号
    /// </summary>
    public interface IPDeviceModelRepository : IYsRepository<DeviceModel, long>
    {
        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <returns></returns>
        Task<List<DeviceModel>> GetDeviceModelsAsync();
    }
}
