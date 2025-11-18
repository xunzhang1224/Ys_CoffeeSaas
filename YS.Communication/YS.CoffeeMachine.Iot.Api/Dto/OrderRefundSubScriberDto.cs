namespace YS.CoffeeMachine.Iot.Api.Dto
{
    /// <summary>
    /// 订单退款消息
    /// </summary>
    public class OrderRefundSubscriberDto
    {
        /// <summary>
        /// 事务Id(不能为空，唯一Id，建议使用雪花Id)
        /// </summary>
        public string TransId { get; set; } = string.Empty;

        /// <summary>
        /// 主订单号（Order表的OrderId）
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 退款的订单商品
        /// </summary>
        public List<RefundOrderProduct> RefundOrderProducts { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        public string RefundReason { get; set; }

        /// <summary>
        /// 操作退款的用户Id
        /// </summary>
        public long OperateUserId { get; set; } = 0;

        /// <summary>
        /// 退款类型（true:系统自动退款（比如出货失败，系统自动退款等等），false：用户操作退款）
        /// </summary>
        public bool OrderRefundType { get; set; } = false;
    }

    /// <summary>
    /// 退款的订单商品
    /// </summary>
    public class RefundOrderProduct
    {
        /// <summary>
        /// 子订单号
        /// </summary>
        public string OrderProductId { get; set; }

        /// <summary>
        /// 退款金额（退款金额等于-1，就是当前子订单全部退款）
        /// </summary>
        public decimal RefundAmount { get; set; } = 0;
    }
}
