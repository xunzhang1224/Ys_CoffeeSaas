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
    /// 设备信息
    /// </summary>
    public interface IDeviceInfoRepository : IYsRepository<DeviceInfo, long>
    {
        /// <summary>
        /// 根据Id获取设备信息
        /// </summary>
        Task<DeviceInfo> GetByIdAsync(long id);

        /// <summary>
        /// 根据国家地区Id获取国家地区名称
        /// </summary>
        Task<(bool, string, long)> GetCountryRegionTextByCountryRegionId(long countryRegionId);

        /// <summary>
        /// 批量添加设备信息
        /// </summary>
        Task<bool> BatchAddAsync(List<DeviceInfo> devices);
    }
}
