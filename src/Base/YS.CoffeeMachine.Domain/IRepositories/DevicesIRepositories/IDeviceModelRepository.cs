using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories
{
    /// <summary>
    /// 设备型号仓储接口
    /// </summary>
    public interface IDeviceModelRepository : IYsRepository<DeviceModel, long>
    {
        /// <summary>
        /// 获取所有设备型号
        /// </summary>
        /// <returns></returns>
        Task<List<DeviceModel>> GetDeviceModelsAsync();
    }
}
