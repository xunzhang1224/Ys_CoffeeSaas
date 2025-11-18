using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.PlatformDto.DeviceDots
{
    /// <summary>
    /// 设备应用入参
    /// </summary>
    public class DeviceAllocationInput : QueryRequest
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public List<DateTime> TimeRange { get; set; }
    }
}