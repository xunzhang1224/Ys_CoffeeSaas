using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 出货结果枚举
    /// </summary>
    public enum OrderShipmentResult
    {
        /// <summary>
        /// 出货失败
        /// </summary>
        [Description("出货失败")]
        Fail = 0,

        /// <summary>
        /// 出货成功
        /// </summary>
        [Description("出货成功")]
        Success = 1,

        /// <summary>
        /// 部分成功
        /// </summary>
        [Description("部分成功")]
        Part = 2,

        /// <summary>
        /// 未出货
        /// </summary>
        [Description("未出货")]
        NotShipped = 3
    }

    /// <summary>
    /// 订单售卖结果枚举
    /// </summary>
    public enum OrderSaleResult
    {
        /// <summary>
        /// 未支付
        /// </summary>
        [Description("未支付")]
        NotPay = 0,

        /// <summary>
        /// 支付成功
        /// </summary>
        [Description("支付成功")]
        Success = 1,

        /// <summary>
        /// 支付超时
        /// </summary>
        [Description("支付超时")]
        Timeout = 2,

        /// <summary>
        /// 出品异常
        /// </summary>
        //[Description("出品异常")]
        //Exception = 3,

        /// <summary>
        /// 取消支付
        /// </summary>
        [Description("取消支付")]
        Cancel = 4,

        /// <summary>
        /// 全部退款
        /// </summary>
        [Description("全部退款")]
        Refund = 5,

        /// <summary>
        /// 支付失败
        /// </summary>
        [Description("支付失败")]
        Fail = 6,

        /// <summary>
        /// 部分退款
        /// </summary>
        [Description("部分退款")]
        PartialRefund = 7,

        /// <summary>
        /// 退款中
        /// </summary>
        [Description("退款中")]
        Refunding = 8,
    }

    /// <summary>
    /// 订单状态枚举
    /// </summary>
    public enum OrderStatusEnum
    {
        ///// <summary>
        ///// 已创建
        ///// </summary>
        //[Description("已创建")]
        //Created = 0,

        /// <summary>
        /// 支付中
        /// </summary>
        [Description("支付中")]
        PaymentInProgress = 1,

        /// <summary>
        /// 支付失败
        /// </summary>
        [Description("支付失败")]
        Fail = 2,

        /// <summary>
        /// 取消支付
        /// </summary>
        [Description("取消支付")]
        CancelPayment = 3,

        /// <summary>
        /// 支付成功
        /// </summary>
        [Description("支付成功")]
        Success = 10,

        /// <summary>
        /// 退款中
        /// </summary>
        [Description("退款中")]
        Refunding = 11,

        /// <summary>
        /// 部分退款
        /// </summary>
        [Description("部分退款")]
        PartialRefund = 12,

        /// <summary>
        /// 全部退款
        /// </summary>
        [Description("全部退款")]
        FullRefund = 13
    }

    /// <summary>
    /// 退款状态
    /// </summary>
    public enum RefundStatusEnum
    {
        /// <summary>
        /// 退款成功
        /// </summary>
        [Description("退款成功")]
        Success = 0,

        /// <summary>
        /// 退款失败
        /// </summary>
        [Description("退款失败")]
        Fail = 1,

        /// <summary>
        /// 退款中
        /// </summary>
        [Description("退款中")]
        Refunding = 2,

        /// <summary>
        /// 待退款
        /// </summary>
        [Description("待退款")]
        UnRefund = 4
    }

    /// <summary>
    /// 子订单退款状态
    /// </summary>
    public enum OrderDetailRerundStatusEnum
    {
        /// <summary>
        /// 未退款
        /// </summary>
        [Description("未退款")]
        NotRefunded,

        /// <summary>
        /// 退款中
        /// </summary>
        [Description("退款中")]
        Refunding,

        /// <summary>
        /// 部分退款
        /// </summary>
        [Description("部分退款")]
        PartialRefund,

        /// <summary>
        /// 全部退款
        /// </summary>
        [Description("全部退款")]
        FullRefund
    }

    /// <summary>
    /// 退款的处理方式
    /// </summary>
    public enum HandlingMethodEnum
    {
        /// <summary>
        /// 全额退款
        /// </summary>
        [Description("全额退款")]
        FullRefund = 0,

        /// <summary>
        /// 部分退款
        /// </summary>
        [Description("部分退款")]
        PartialRefund = 1,

        /// <summary>
        /// 仅更新库存（副柜接入前无实际作用）
        /// </summary>
        [Description("仅更新库存")]
        UpdateInventory = 2,
    }
}