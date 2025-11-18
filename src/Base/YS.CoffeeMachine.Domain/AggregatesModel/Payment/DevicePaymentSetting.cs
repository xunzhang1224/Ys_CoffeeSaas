namespace YS.CoffeeMachine.Domain.AggregatesModel.Payment
{
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 设备支付设置
    /// </summary>
    public class DevicePaymentSetting : BaseEntity
    {
        /// <summary>
        /// 币种（字典）
        /// </summary>
        public string? Currency { get; private set; }

        /// <summary>
        /// 支付等待时间
        /// </summary>
        public int? PayWait { get; private set; } = 60;

        /// <summary>
        /// 币种统一位置开关
        /// </summary>
        public EnabledEnum? CurrencyLocationEnable { get; private set; }

        /// <summary>
        /// 币种统一位置
        /// </summary>
        public PaymentCurrencyLocatonEnum? PaymentCurrencyLocaton { get; private set; } = PaymentCurrencyLocatonEnum.CurrencyBefore;
    }
}
