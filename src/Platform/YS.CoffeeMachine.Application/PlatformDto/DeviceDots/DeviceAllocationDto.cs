using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.PlatformDto.DeviceDots
{
    /// <summary>
    /// 设备应用dto
    /// </summary>
    public class DeviceAllocationDto
    {
        /// <summary>
        /// 设备基础Id
        /// </summary>
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? DeviceId { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentNumber { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string DeviceModel { get; set; }

        /// <summary>
        /// 企业id
        /// </summary>
        public long? EnterpriseId { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 绑定的用户
        /// </summary>
        public string UserNames { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegisterTime { get; set; }
    }
}
