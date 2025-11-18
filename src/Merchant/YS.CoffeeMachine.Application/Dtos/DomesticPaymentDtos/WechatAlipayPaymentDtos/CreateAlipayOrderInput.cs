using YS.Cabinet.Payment.Alipay;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayPaymentDtos
{
    /// <summary>
    /// 创建jsapi支付入参
    /// </summary>
    public class CreateAlipayOrderInput
    {
        /// <summary>
        /// 系统支付服务商Id
        /// </summary>
        public long SystemPaymentServiceProviderId { get; set; }

        /// <summary>
        /// 商户Id
        /// </summary>
        /// <remarks>
        /// 支付宝:子商户Id smid
        /// </remarks>
        public string MerchantId { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 金额 元
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 客户端人脸识别得到的ftoken
        /// </summary>
        public string FToken { get; set; }

        /// <summary>
        /// 商品标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 用户OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 商户支付方式Me_PaymentMethod表的Id
        /// </summary>
        public long PaymentMethodId { get; set; }

        /// <summary>
        /// 商品信息
        /// </summary>
        public List<Alipay_GoodsDetail> GoodsDetail { get; set; }
    }
}
