using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备列表查询参数
    /// </summary>
    public class EnterpriseDevicesInput : QueryRequest
    {
        /// <summary>
        /// 设备sn编号
        /// </summary>
        public string EquipmentNumber { get; set; }
        /// <summary>
        /// 所属企业Id
        /// </summary>
        public long? EnterpriseId { get; set; }
        /// <summary>
        /// 目标企业Id
        /// </summary>
        public long? BelongingEnterpriseId { get; set; }
        /// <summary>
        /// 设备分配方式
        /// </summary>
        public DeviceAllocationEnum? DeviceAllocationType { get; set; }
        /// <summary>
        /// 分配时间范围
        /// </summary>
        public List<DateTime> AllocateTimeRange { get; set; }
        /// <summary>
        /// 回收时间范围
        /// </summary>
        public List<DateTime> RecyclingTimeRange { get; set; }
    }
}
