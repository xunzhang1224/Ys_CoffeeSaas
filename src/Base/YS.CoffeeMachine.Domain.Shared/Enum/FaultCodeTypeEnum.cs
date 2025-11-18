using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 故障码枚举
    /// </summary>
    public enum FaultCodeTypeEnum
    {
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error = 1,

        /// <summary>
        /// 事件
        /// </summary>
        [Description("事件")]
        Event = 0,
    }
}
