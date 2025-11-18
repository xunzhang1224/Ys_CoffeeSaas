namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayPaymentDtos
{
    /// <summary>
    /// 支付成功回调超时(入参)
    /// </summary>
    public class PaymentSuccessCallbackTimeoutDto
    {
        /// <summary>
        /// 主订单号(Order表的Code)
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 交易订单号
        /// </summary>
        public string OrderNo { get; set; }
    }
}