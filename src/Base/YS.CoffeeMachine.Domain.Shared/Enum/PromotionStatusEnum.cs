using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 营销活动状态类型
    /// </summary>
    public enum PromotionStatusEnum
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Description("未开始")]
        NotStarted,

        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")]
        Ing,

        /// <summary>
        /// 活动结束
        /// </summary>
        [Description("活动结束")]
        End,
    }
}
