using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.DeviceDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Queries.IDevicesQueries
{
    /// <summary>
    /// 设备基本信息查询
    /// </summary>
    public interface IDeviceBaseQueries
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        Task<List<DeviceCapacityCfg>> GetDeviceCapacityCfgs(long deviceBaseId);

        /// <summary>
        /// 获取管理列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceVersionManageDto>> GetDeviceVersionManageList(DeviceVersionManageInput input);

        /// <summary>
        /// 获取更新记录
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<DeviceVersionUpdateRecordDto>> GetDeviceVersionUpdateRecord(DeviceVersionUpdateRecordInput input);

        /// <summary>
        /// 获取更新记录new
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ExpandoObject>> GetDeviceVersionUpdateRecordNew(DeviceVersionUpdateRecordInput input);

        /// <summary>
        /// 更新记录(根据版本)
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<PushDataByVersionDto>> GetPushDataByVersion(PushDataByVersionInput input);

        /// <summary>
        /// 更新记录(根据设备)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PushDataByDeviceDto>> GetPushDateByDevice(PushDataByDeviceInput input);

        /// <summary>
        /// 获取指标
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        Task<List<DeviceMetrics>> GetDeviceMetrics(long deviceBaseId);

        /// <summary>
        /// 获取其他信息
        /// </summary>
        Task<List<DeviceOtherMsgDto>> GetDeviceOtherMsg(long deviceId);

        /// <summary>
        /// 设备统计
        /// </summary>
        Task<dynamic> GetDeivceCount();
    }
}
