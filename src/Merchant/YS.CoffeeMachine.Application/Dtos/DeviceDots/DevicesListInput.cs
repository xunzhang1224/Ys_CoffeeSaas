using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备列表查询参数
    /// </summary>
    public class DevicesListInput : QueryRequest
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long enterpriseinfoId { get; set; }
        /// <summary>
        /// 设备名称或编号
        /// </summary>
        public string deviceName_Number { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceStatusEnum? status { get; set; } = null;
        /// <summary>
        /// 设备型号
        /// </summary>
        public long? deviceModelId { get; set; } = null;
        /// <summary>
        /// 设备分组
        /// </summary>
        public List<long> groupIds { get; set; }
        /// <summary>
        /// 上线时间
        /// </summary>
        public List<string> timeRange { get; set; }

        /// <summary>
        /// 当天时间范围
        /// </summary>
        public List<DateTime>? DateRange { get; set; }
    }

    /// <summary>
    /// 设备列表查询参数
    /// </summary>
    public class DevicesH5ListInput : QueryRequest
    {
        /// <summary>
        /// 设备名称或编号
        /// </summary>
        public string deviceName_Number { get; set; }
    }

    /// <summary>
    /// 设备列表查询参数
    /// </summary>
    public class DeviceInfoListByDeviceIdInput : QueryRequest
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long deviceId { get; set; }
        /// <summary>
        /// 设备名称或sn
        /// </summary>
        public string deviceName_SN { get; set; }
        /// <summary>
        /// 分组Ids
        /// </summary>
        public List<long> groupIds { get; set; }
    }

    /// <summary>
    /// 设备列表查询参数
    /// </summary>
    public class DeviceInfoListByDeviceModelIdInput : QueryRequest
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long enterpriseinfoId { get; set; }
        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long deviceModelId { get; set; }
        /// <summary>
        /// 设备名称或sn
        /// </summary>
        public string deviceName_SN { get; set; }
        /// <summary>
        /// 分组Ids
        /// </summary>
        public List<long> groupIds { get; set; }
    }
}
