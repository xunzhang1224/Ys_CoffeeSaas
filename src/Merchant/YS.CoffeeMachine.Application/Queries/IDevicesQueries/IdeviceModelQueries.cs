using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Queries.IDevicesQueries
{
    /// <summary>
    /// 设备型号查询
    /// </summary>
    public interface IdeviceModelQueries
    {
        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceModelDto> GetDeviceModelAsync(long id);

        /// <summary>
        /// 获取设备型号列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceModelDto>> GetDeviceModelListAsync(QueryRequest request);

        /// <summary>
        /// 获取设备型号列表
        /// </summary>
        /// <returns></returns>
        Task<List<DeviceModel>> GetAllDeviceModels();
    }
}
