using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 回调类型枚举
    /// </summary>
    public enum CallBackTypeEnum
    {
        /// <summary>
        /// 设备上线
        /// </summary>
        [Description("device.online")]
        DeviceOnline,

        /// <summary>
        /// 设备下线
        /// </summary>
        [Description("device.offline")]
        DeviceOffline,

        /// <summary>
        /// 设备属性数据上报
        /// </summary>
        [Description("device.up.properties")]
        DeviceUpProperties,

        /// <summary>
        /// 设备事件上报
        /// </summary>
        [Description("device.up.events")]
        DeviceUpEvents,

        /// <summary>
        /// 设备指令上报
        /// </summary>
        [Description("device.up.commands")]
        DeviceUpCommands
    }
}
