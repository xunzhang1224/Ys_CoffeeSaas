using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 冲洗部件类型
    /// </summary>
    public enum FlushComponentTypeEnum
    {
        /// <summary>
        /// 搅拌器
        /// </summary>
        [Description("搅拌器")]
        Blender,

        /// <summary>
        /// 冲泡器
        /// </summary>
        [Description("冲泡器")]
        Brewer,

        /// <summary>
        /// 料盒
        /// </summary>
        [Description("料盒")]
        MaterialBox
    }

    /// <summary>
    /// 冲洗类型
    /// </summary>
    public enum FlushTypeEnum
    {
        /// <summary>
        /// 整机清洗
        /// </summary>
        [Description("整机清洗")]
        MachineCleaning,

        /// <summary>
        /// 自动清洗
        /// </summary>
        [Description("自动清洗")]
        AutomaticCleaning,

        /// <summary>
        /// 手动清洗
        /// </summary>
        [Description("手动清洗")]
        ManualCleaning
    }

    /// <summary>
    /// 冲洗状态类型
    /// </summary>
    public enum FlushResultEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail
    }
}
