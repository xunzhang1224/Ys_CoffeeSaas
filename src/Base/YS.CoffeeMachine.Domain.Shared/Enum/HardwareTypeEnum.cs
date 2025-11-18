using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 部件类型枚举
    /// </summary>
    public enum HardwareTypeEnum
    {
        /// <summary>
        /// 物料
        /// </summary>
        [Description("物料")]
        Material,

        /// <summary>
        /// 粉料
        /// </summary>
        [Description("粉料")]
        Powder,

        /// <summary>
        /// 关键部件
        /// </summary>
        [Description("关键部件")]
        CriticalComponent
    }
}
