using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 设备错误日志枚举
    /// </summary>
    public enum DeviceErrorLogEnum
    {
        /// <summary>
        /// 恢复
        /// </summary>
        [Description("恢复")]
        Restore,

        /// <summary>
        /// 故障
        /// </summary>
        [Description("故障")]
        Fault
    }
}
