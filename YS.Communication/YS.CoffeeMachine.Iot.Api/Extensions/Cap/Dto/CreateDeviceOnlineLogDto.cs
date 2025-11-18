using System.ComponentModel.DataAnnotations;

namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto
{
    /// <summary>
    /// 上下线日志入参
    /// </summary>
    public class CreateDeviceOnlineLogDto
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 设备名
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// 型号名
        /// </summary>
        public string DeviceModelName { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        [Required]
        public long DeviceId { get; set; } = 0;

        /// <summary>
        /// 设备生成编号
        /// </summary>
        public string? Mid { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 企业信息Id
        /// </summary>
        public long? EnterpriseinfoId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceId"></param>
        /// <param name="deviceModelName"></param>
        /// <param name="status"></param>
        /// <param name="enterpriseinfoId"></param>
        public CreateDeviceOnlineLogDto(string mid, string deviceName, long deviceId, string deviceModelName, bool status, long enterpriseinfoId)
        {
            Mid = mid;
            DeviceId = deviceId;
            DeviceName = deviceName;
            DeviceModelName = deviceModelName;
            Status = status;
            EnterpriseinfoId = enterpriseinfoId;
        }
    }
}
