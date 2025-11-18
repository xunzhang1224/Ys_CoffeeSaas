using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 币种展示位置枚举
    /// </summary>
    public enum CurrencyShowFormatEnum
    {
        /// <summary>
        /// 货币符号显示在金额前面
        /// </summary>
        [Description("货币符号显示在金额前面")]
        CurrencyBefore,
        /// <summary>
        /// 货币符号显示在金额后面
        /// </summary>
        [Description("货币符号显示在金额后面")]
        CurrencyAfter,
    }
}
