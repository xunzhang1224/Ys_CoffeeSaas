using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备服务dto
    /// </summary>
    public class DeviceServiceProvidersDto
    {
        /// <summary>
        /// 服务商Id
        /// </summary>
        public long ServiceProviderInfoId { get; set; }
        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceInfoId { get; set; }
    }
}
