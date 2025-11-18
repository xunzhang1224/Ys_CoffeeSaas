using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 操作结果
    /// </summary>
    public enum OperationResultEnum
    {
        /// <summary>
        /// 指令已下发
        /// </summary>
        [Description("指令已下发")]
        CommandIssued,
        /// <summary>
        /// 设备已执行
        /// </summary>
        [Description("设备已执行")]
        CommandExecuted,
        /// <summary>
        /// 设备未执行
        /// </summary>
        [Description("设备未执行")]
        CommandUnexecuted,
    }
}
