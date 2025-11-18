namespace YS.CoffeeMachine.Domain.AggregatesModel.Payment
{
    using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
    using YS.CoffeeMachine.Domain.Shared.Enum;

    /// <summary>
    /// 支付配置
    /// </summary>

    public class PaymentConfig : EnterpriseBaseEntity
    {
        /// <summary>
        /// 支付方式id
        /// </summary>
        public long P_PaymentConfigId { get; private set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// 进件状态
        /// </summary>
        public PaymentConfigStatueEnum PaymentConfigStatue { get; private set; }

        /// <summary>
        /// 商户编码
        /// </summary>
        public string MerchantCode { get; private set; }

        /// <summary>
        /// 支付平台appid
        /// </summary>
        public string PaymentPlatformAppId { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum Enabled { get; private set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string PictureUrl { get; private set; }

        private PaymentConfig() { }

        /// <summary>
        /// 新增进件信息
        /// </summary>
        /// <param name="p_paymentConfigId"></param>
        /// <param name="email"></param>
        /// <param name="remark"></param>
        /// <param name="merchantCode"></param>
        /// <param name="pictureUrl"></param>
        public PaymentConfig(long p_paymentConfigId, string email, string remark, string merchantCode, string pictureUrl)
        {
            P_PaymentConfigId = p_paymentConfigId;
            Email = email;
            Remark = remark;
            MerchantCode = merchantCode;
            PictureUrl = pictureUrl;
            PaymentConfigStatue = PaymentConfigStatueEnum.Onboarding;
            PaymentPlatformAppId = "0";
        }

        /// <summary>
        /// 更新进件状态
        /// </summary>
        /// <param name="paymentConfigStatue"></param>
        public void UpdateStatue(PaymentConfigStatueEnum paymentConfigStatue)
        {
            PaymentConfigStatue = paymentConfigStatue;
        }

        /// <summary>
        /// 更新备注
        /// </summary>
        /// <param name="paymentConfigStatue"></param>
        public void UpdateRemark(string remark)
        {
            Remark = remark;
        }
    }
}
