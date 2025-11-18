using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 营销活动
    /// </summary>
    public enum PromotionType
    {
        /// <summary>
        /// 满减
        /// </summary>
        [Description("满减")]
        Discount,

        /// <summary>
        /// 折扣
        /// </summary>
        [Description("折扣")]
        Coupon
    }
}
