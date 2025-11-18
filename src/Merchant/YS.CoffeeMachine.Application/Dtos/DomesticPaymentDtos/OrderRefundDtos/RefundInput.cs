namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos
{
    /// <summary>
    /// 订单退款
    /// </summary>
    public class RefundInput
    {
        /// <summary>
        /// 主订单号
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        public string? RefundReason { get; set; }

        /// <summary>
        /// 退款的订单商品集合
        /// </summary>
        public List<RefundOrderProduct> OrderProducts { get; set; }
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