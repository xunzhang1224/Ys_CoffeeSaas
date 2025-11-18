using YS.Cabinet.Payment.WechatPay;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayPaymentDtos
{
    /// <summary>
    /// 发起订单支付input
    /// </summary>
    public class CreateWxpayOrderInput
    {
        /// <summary>
        /// 系统支付方式Id
        /// </summary>
        public long SystemPaymentServiceProviderId { get; set; }

        /// <summary>
        /// 支付方式Id
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// 支付金额(单位元)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 支付的用户OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 人脸凭证, 用于刷脸支付。
        /// </summary>
        public string FaceCode { get; set; }

        /// <summary>
        /// 商品信息
        /// </summary>
        public List<WxPromotionGoodsDetail> GoodsDetail { get; set; }
    }
}
