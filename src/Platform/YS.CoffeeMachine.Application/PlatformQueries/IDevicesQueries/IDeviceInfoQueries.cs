using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.DeviceDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.PlatformQueries.IDevicesQueries
{
    /// <summary>
    /// 设备查询
    /// </summary>
    public interface IDeviceInfoQueries
    {
        /// <summary>
        /// 设备分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceListDto>> GetDevicePageListAsync(DeviceListInput request);

        /// <summary>
        /// 根据设备id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceInfoDto> GetDeviceInfoByIdAsync(long id);

        /// <summary>
        /// 根据mid获取设备信息
        /// </summary>
        /// <param name="mids"></param>
        /// <returns></returns>
        Task<List<DeviceInfo>> GetDeviceInfoByMids(List<string> mids);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceInfoDto> GetDeviceInfoAsync(long id);

        /// <summary>
        /// 根据箱体获取平台设备
        /// </summary>
        /// <param name="boxId"></param>
        /// <returns></returns>
        Task<DeviceBaseInfo> GetDeviceBaseInfoByBoxIdAsync(string boxId);

        /// <summary>
        /// 获取设备初始化信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        Task<DeviceInitialization> GetDeviceInitializationAsync(string mid);
    }
}
