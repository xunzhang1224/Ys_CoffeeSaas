namespace YS.CoffeeMachine.Domain.AggregatesModel.Payment
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 设备支付配置
    /// </summary>
    public class DevicePaymentConfig : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long? DeviceInfoId { get; set; }

        /// <summary>
        /// 支付账号（配置）id
        /// </summary>
        public long? PaymentConfigId { get; private set; }

        ///// <summary>
        ///// 币种（字典）
        ///// </summary>
        //public string? Currency { get; private set; }

        ///// <summary>
        ///// 支付等待时间
        ///// </summary>
        //public int? PayWait { get; private set; } = 60;

        ///// <summary>
        ///// 币种统一位置开关
        ///// </summary>
        //public EnabledEnum? CurrencyLocationEnable { get; private set; }

        ///// <summary>
        ///// 币种统一位置
        ///// </summary>
        //public PaymentCurrencyLocatonEnum? PaymentCurrencyLocaton { get; private set; } = PaymentCurrencyLocatonEnum.CurrencyBefore;

        /// <summary>
        /// 设备支付配置
        /// </summary>
        protected DevicePaymentConfig() { }

        /// <summary>
        /// 设备支付配置
        /// </summary>
        /// <param name="paymentConfigId"></param>
        /// <param name="deviceInfoId"></param>
        public DevicePaymentConfig(long paymentConfigId, long deviceInfoId)
        {
            DeviceInfoId = deviceInfoId;
            PaymentConfigId = paymentConfigId;
        }
    }
}
