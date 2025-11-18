using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 优惠劵折扣方式
    /// </summary>
    public enum DiscountType
    {
        /// <summary>
        /// 固定金额
        /// </summary>
        [Description("固定金额")]
        FixedAmount,

        /// <summary>
        /// 百分比折扣
        /// </summary>
        [Description("百分比折扣")]
        Percentage
    }
}
