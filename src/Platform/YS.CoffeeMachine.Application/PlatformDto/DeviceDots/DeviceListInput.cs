using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.PlatformDto.DeviceDots
{
    /// <summary>
    /// 设备列表入参
    /// </summary>
    public class DeviceListInput : QueryRequest
    {
        /// <summary>
        /// 设备sn编号
        /// </summary>
        public string EquipmentNumber { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public bool? Status { get; set; } = null;
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long? DeviceModelId { get; set; }
        /// <summary>
        /// 上线时间
        /// </summary>
        public List<DateTime>? OnlineData { get; set; } = null;
    }
}
