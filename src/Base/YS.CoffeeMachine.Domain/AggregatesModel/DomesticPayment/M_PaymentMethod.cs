using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment
{
    /// <summary>
    /// 二级商户国内支付方式
    /// </summary>
    public class M_PaymentMethod : EnterpriseBaseEntity
    {
        /// <summary>
        /// Desc:系统支付方式表的Id（SystemPaymentMethod表的Id）
        /// Default:
        /// Nullable:False
        /// </summary>
        public long SystemPaymentMethodId { get; private set; }

        /// <summary>
        /// Desc:进件手机
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Phone { get; private set; } = null!;

        /// <summary>
        /// 商户类型
        /// </summary>
        public DomesticMerchantTypeEnum? DomesticMerchantType { get; private set; } = null;

        /// <summary>
        /// 支付模式
        /// </summary>
        public PaymentModeEnum PaymentMode { get; private set; } = PaymentModeEnum.OnlinePayment;

        /// <summary>
        /// Desc:支付进件状态（枚举类型：InternalOnboardingStatusEnum）
        /// Default:
        /// Nullable:False
        /// </summary>
        public InternalOnboardingStatusEnum PaymentEntryStatus { get; private set; }

        /// <summary>
        /// Desc:支付绑定的类型（ 0:全部设备，1：指定设备）
        /// Default:
        /// Nullable:False
        /// </summary>
        public BindTypeEnum BindType { get; private set; } = BindTypeEnum.AllDevice;

        /// <summary>
        /// Desc:是否启用（枚举类型：EnabledEnum）
        /// Default:1
        /// Nullable:False
        /// </summary>
        public EnabledEnum IsEnabled { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// Desc:子商户Id
        /// Default:
        /// Nullable:False
        /// </summary>
        public string MerchantId { get; private set; } = null!;

        /// <summary>
        /// Desc:支付参数Json格式(目前只有离线支付使用)
        /// Default:
        /// Nullable:False
        /// </summary>
        public string PaymentParameters { get; private set; }

        /// <summary>
        /// Desc:支付的服务商表（SystemPaymentServiceProvider表的Id）
        /// Default:
        /// Nullable:False
        /// </summary>
        public long SystemPaymentServiceProviderId { get; private set; }

        /// <summary>
        /// 保护构造
        /// </summary>
        protected M_PaymentMethod() { }

        /// <summary>
        /// 添加商户支付方式
        /// </summary>
        /// <param name="systemPaymentMethodId"></param>
        /// <param name="phone"></param>
        /// <param name="onboardingStatusEnum"></param>
        /// <param name="remark"></param>
        /// <param name="systemPaymentServiceProviderId"></param>
        public M_PaymentMethod(long systemPaymentMethodId, string phone, InternalOnboardingStatusEnum onboardingStatusEnum, string remark, long systemPaymentServiceProviderId)
        {
            SystemPaymentMethodId = systemPaymentMethodId;
            Phone = phone;
            PaymentEntryStatus = onboardingStatusEnum;
            Remark = remark;
            SystemPaymentServiceProviderId = systemPaymentServiceProviderId;
            IsEnabled = EnabledEnum.Enable;
            PaymentParameters = string.Empty;
            MerchantId = string.Empty;
        }

        /// <summary>
        /// 设置商户类型
        /// </summary>
        /// <param name="domesticMerchantType"></param>
        public void SetDomesticMerchantType(DomesticMerchantTypeEnum domesticMerchantType)
        {
            DomesticMerchantType = domesticMerchantType;
        }

        /// <summary>
        /// 更新备注
        /// </summary>
        /// <param name="remark"></param>
        public void ModifyRemark(string remark)
        {
            Remark = remark;
        }

        /// <summary>
        /// 更新子商户进件状态
        /// </summary>
        /// <param name="subMchId"></param>
        /// <param name="status"></param>
        public void UpdateApplymentStatus(string subMchId, InternalOnboardingStatusEnum status)
        {
            MerchantId = subMchId;
            PaymentEntryStatus = status;
        }

        /// <summary>
        /// 设置进件状态
        /// </summary>
        /// <param name="status"></param>
        public void SetPaymentEntryStatus(InternalOnboardingStatusEnum status)
        {
            PaymentEntryStatus = status;
        }

        /// <summary>
        /// 设置二级商户Id
        /// </summary>
        /// <param name="merchantId"></param>
        public void SetMerchantId(string? merchantId)
        {
            if (!string.IsNullOrWhiteSpace(merchantId))
                MerchantId = merchantId;
        }
    }
}