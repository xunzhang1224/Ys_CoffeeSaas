using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 饮品制作状态枚举
    /// </summary>
    public enum DrinkMakeStatusEnum
    {
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail,
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success,
        /// <summary>
        /// 制作中取消
        /// </summary>
        [Description("制作中取消")]
        CancelledDuringProduction,
        /// <summary>
        /// 制作前取消
        /// </summary>
        [Description("制作前取消")]
        CancelBeforeProduction,
    }
}
