using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备入参
    /// </summary>
    public class UnDeviceInput : QueryRequest
    {
        /// <summary>
        /// 企业id
        /// </summary>
        public long EnterpriseinfoId { get; set; }
        /// <summary>
        /// 设备sn编号
        /// </summary>
        public string EquipmentNumber { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public long? DeviceModelId { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceStatusEnum? DeviceStatus { get; set; }
        /// <summary>
        /// 分配时间范围
        /// </summary>
        public List<DateTime> OnLineTimeRange { get; set; }
    }
}
