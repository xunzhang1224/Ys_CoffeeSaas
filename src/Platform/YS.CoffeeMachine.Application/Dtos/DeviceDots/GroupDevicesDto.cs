using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备分组dto
    /// </summary>
    public class GroupDevicesDto
    {
        /// <summary>
        /// 分组Id
        /// </summary>
        public long GroupsId { get; set; }
        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceInfoId { get; set; }
    }
}
