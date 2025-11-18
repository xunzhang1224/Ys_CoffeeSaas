using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 优惠劵
    /// </summary>
    public enum CouponStatusEnum
    {
        /// <summary>
        /// 可用
        /// </summary>
        [Description("可用")]
        Active,

        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")]
        Used,

        /// <summary>
        /// 已过期
        /// </summary>
        [Description("已过期")]
        Expired,
    }
}
