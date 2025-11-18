namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos
{
    /// <summary>
    /// 退款output
    /// </summary>
    public class OrderRefundOutput
    {
        /// <summary>
        /// 商家自定义退款号
        /// </summary>
        public string OutRefundNo { get; set; }

        /// <summary>
        /// 支付平台返回的退款单号
        /// </summary>
        public string RefundId { get; set; }

        /// <summary>
        /// 商家订单号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 支付平台订单号
        /// </summary>
        public string OrderNo { get; set; }
    }
}