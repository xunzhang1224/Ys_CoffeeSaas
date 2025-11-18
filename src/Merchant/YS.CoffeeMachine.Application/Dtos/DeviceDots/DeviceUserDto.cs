using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 用户绑定的设备信息
    /// </summary>
    public class DeviceUserDto
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long DeviceId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long? BaseInfoId { get; set; }

        /// <summary>
        /// 设备名字
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// mid
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool? IsOnline { get; set; }

        /// <summary>
        /// 设备激活状态
        /// </summary>
        public DeviceActiveEnum? DeviceActiveState { get; set; }
    }
}
