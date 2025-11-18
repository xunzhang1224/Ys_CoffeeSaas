using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 预警类型
    /// </summary>
    public enum EarlyWarningTypeEnum
    {
        /// <summary>
        /// 离线预警
        /// </summary>
        [Description("离线预警")]
        OfflineWarning,

        /// <summary>
        /// 缺料预警
        /// </summary>
        [Description("缺料预警")]
        ShortageWarning,

        /// <summary>
        /// 搅拌器
        /// </summary>
        [Description("搅拌器")]
        BlenderWarning,

        /// <summary>
        /// 冲泡器
        /// </summary>
        [Description("冲泡器")]
        BrewerWarning,

        /// <summary>
        /// 料盒
        /// </summary>
        [Description("料盒")]
        MaterialBoxWarning,
    }

    /// <summary>
    /// 预警类型状态
    /// </summary>
    public enum EarlyWarningStatusEnum
    {
        /// <summary>
        /// 离线预警
        /// </summary>
        [Description("离线预警")]
        OfflineWarning,

        /// <summary>
        /// 缺料预警
        /// </summary>
        [Description("缺料预警")]
        ShortageWarning,
    }
}
