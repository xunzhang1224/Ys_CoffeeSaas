using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备上线日志入参
    /// </summary>
    public class DeviceOnlineLogInput : QueryRequest
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long? DeviceId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string? DeviceNo { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long? DeviceModelId { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool? IsOnline { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public List<DateTime>? DateTimes { get; set; }
    }

    /// <summary>
    /// 设备事件日志输入
    /// </summary>
    public class DeviceEventLogInput : QueryRequest
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public long? DeviceId { get; set; }

        /// <summary>
        /// 事件名称(从字典获取)
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 时间范围
        /// </summary>
        public List<DateTime> DateTimeRange { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 设备型号id
        /// </summary>
        public long? DeviceModelId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
    }

    /// <summary>
    /// 设备错误日志输入
    /// </summary>
    public class DeviceErrorLogInput : QueryRequest
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public long? DeviceId { get; set; }

        /// <summary>
        /// 错误名称(从字典获取)
        /// </summary>
        public string ErrorName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 时间范围
        /// </summary>
        public List<DateTime> DateTimeRange { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 设备型号id
        /// </summary>
        public long? DeviceModelId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }
    }
}
