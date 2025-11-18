using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 订单类型
    /// </summary>
    [Description("订单类型")]
    public enum OrderTypeEnum
    {
        /// <summary>
        /// 非订单
        /// </summary>
        [Description("非订单")]
        Not,

        /// <summary>
        /// 安卓订单
        /// </summary>
        [Description("安卓订单")]
        AndroidOrder,

        /// <summary>
        /// 线上订单
        /// </summary>
        [Description("线上订单")]
        OnlineOrder,

        /// <summary>
        /// 其他(现金支付订单等)
        /// </summary>
        [Description("其他")]
        Other
    }
}