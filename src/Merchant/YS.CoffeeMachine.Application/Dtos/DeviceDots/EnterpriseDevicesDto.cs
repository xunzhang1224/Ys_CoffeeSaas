using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 企业设备dto
    /// </summary>
    public class EnterpriseDevicesDto
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceId { get; set; }
        /// <summary>
        /// 设备SN
        /// </summary>
        public string EquipmentNumber { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string DeviceModel { get; set; }
        /// <summary>
        /// 所属企业Id
        /// </summary>
        public long BelongingEnterpriseId { get; set; }
        /// <summary>
        /// 所属企业名称
        /// </summary>
        public string BelongingEnterpriseName { get; set; }
        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseId { get; set; }
        /// <summary>
        /// 目标企业名称
        /// </summary>
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 设备分配方式
        /// </summary>
        public DeviceAllocationEnum DeviceAllocationType { get; set; }
        /// <summary>
        /// 回收时间
        /// </summary>
        public DateTime? RecyclingTime { get; set; }
        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTime? AllocateTime { get; set; }
    }

    /// <summary>
    /// 设备列表
    /// </summary>
    public class EnterpriseDevicesListDto
    {
        /// <summary>
        /// 设备列表
        /// </summary>
        public List<EnterpriseDevicesDto> EnterpriseDevicesList { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EnterpriseDevicesListDto() { EnterpriseDevicesList = new List<EnterpriseDevicesDto>(); }
    }
}
