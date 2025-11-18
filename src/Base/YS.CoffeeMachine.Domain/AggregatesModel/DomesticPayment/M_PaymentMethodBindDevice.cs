using YS.CoffeeMachine.Domain.AggregatesModel.Basics;

namespace YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment
{
    /// <summary>
    /// 商户支付方式绑定的设备表
    /// </summary>
    public class M_PaymentMethodBindDevice : EnterpriseBaseEntity
    {
        /// <summary>
        /// Desc:商户支付方式表的Id（Me_PaymentMethod表的Id）
        /// Default:
        /// Nullable:False
        /// </summary>
        public long PaymentMethodId { get; private set; }

        /// <summary>
        /// DeviceInfo表Id
        /// </summary>
        public long DeviceId { get; private set; }

        /// <summary>
        /// 系统支付方式Id
        /// </summary>
        public long? SystemPaymentMethodId { get; private set; }

        /// <summary>
        /// 保护构造
        /// </summary>
        protected M_PaymentMethodBindDevice() { }

        /// <summary>
        /// 绑定关系
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <param name="deviceId"></param>
        public M_PaymentMethodBindDevice(long paymentMethodId, long deviceId, long? systemPaymentMethodId)
        {
            PaymentMethodId = paymentMethodId;
            DeviceId = deviceId;
            SystemPaymentMethodId = systemPaymentMethodId;
        }
    }
}