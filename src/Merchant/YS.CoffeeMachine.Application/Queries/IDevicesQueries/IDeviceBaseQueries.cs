using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Queries.IDevicesQueries
{
    /// <summary>
    /// 设备基础信息查询
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
        /// 获取指标
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        Task<List<DeviceMetricsOutput>> GetDeviceMetrics(long deviceBaseId);

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        Task<List<DeviceMaterialInfo>> GetDeviceMaterialInfos(long deviceBaseId, MaterialTypeEnum? type = null);

        /// <summary>
        /// 获取预警
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        Task<List<DeviceEarlyWarningsOutput>> GetDeviceWarnings(long deviceBaseId);

        /// <summary>
        /// 获取预警
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        Task<List<DeviceEarlyWarningsOutput>> GetDeviceWarningsAll(long deviceBaseId);

        /// <summary>
        /// 获取异常记录
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        Task<List<DeviceAbnormal>> GetDeviceAbnormals(long deviceBaseId);

        /// <summary>
        /// 获取其他信息
        /// </summary>
        Task<List<DeviceOtherMsgDto>> GetDeviceOtherMsg(long deviceId);

        /// <summary>
        /// 获取设备在线日志
        /// </summary>
        Task<PagedResultDto<DeviceOnlineLogDto>> GetDeviceOnlineLog(DeviceOnlineLogInput input);

        /// <summary>
        /// 获取设备事件日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceEventLogDto>> GetDeviceEventLog(DeviceEventLogInput input);

        /// <summary>
        /// 获取设备故障日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceErrorLogDto>> GetDeviceErrorLog(DeviceErrorLogInput input);

        /// <summary>
        /// 获取设备清洗日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceFlushComponentsLog>> GetDeviceFlushLog(GetDeviceFlushLogInput input);

        /// <summary>
        /// 获取设备清洗日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceRestockLogDto>> GetDeviceRestockLogs(GetDeviceRestockLogInput input);

        /// <summary>
        /// 获取设备补货详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<DeviceRestockLog> GetDeviceRestockSubLogs(long id);

        /// <summary>
        /// 获取设备base info信息
        /// </summary>
        /// <param name="code">永久编码</param>
        /// <returns></returns>
        Task<DeviceBaseInfoForBind> GetDeviceBaseInfo(string code);

        /// <summary>
        /// 获取设备在线状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> GetDeviceOnLineStatus(long id);
    }
}
