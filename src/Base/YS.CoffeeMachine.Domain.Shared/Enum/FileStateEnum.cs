using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 文件中心文件状态
    /// </summary>
    public enum FileStateEnum
    {
        /// <summary>
        /// 导出中
        /// </summary>
        [Description("导出中")]
        Exporting,

        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail,

        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Delete,
    }
}
