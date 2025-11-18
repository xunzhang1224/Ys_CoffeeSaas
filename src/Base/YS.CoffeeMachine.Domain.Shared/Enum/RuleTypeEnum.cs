using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 营销活动规则类型
    /// </summary>
    public enum RuleTypeEnum
    {
        /// <summary>
        /// 满减
        /// </summary>
        [Description("满减")]
        AmountThreshold,

        /// <summary>
        /// 折扣商品
        /// </summary>
        [Description("折扣商品")]
        ProductScope,

        /// <summary>
        /// 会员折扣
        /// </summary>
        [Description("会员折扣")]
        MemberLevel,
    }
}
