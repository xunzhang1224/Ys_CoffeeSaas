using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos
{
    /// <summary>
    /// 获取设备绑定的商户支付方式信息
    /// </summary>
    public class GetDevicePaymentMethodDto
    {
        /// <summary>
        /// 商户支付方式表的Id（M_PaymentMethod表的Id）
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 系统支付方式表的Id（SystemPaymentMethod表的Id）
        /// </summary>
        public long SystemPaymentMethodId { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public long OrgId { get; set; }

        /// <summary>
        /// 支付进件状态（枚举类型：InternalOnboardingStatusEnum）
        /// </summary>
        public InternalOnboardingStatusEnum PaymentEntryStatus { get; set; }

        /// <summary>
        /// Desc:子商户Id
        /// </summary>
        public string MerchantId { get; set; } = null!;

        /// <summary>
        /// Desc:是否启用（枚举类型：EnabledEnum）
        /// Default:1
        /// Nullable:False
        /// </summary>
        public EnabledEnum IsEnabled { get; set; }

        /// <summary>
        /// 支付的服务商表（SystemPaymentServiceProvider表的Id）
        /// </summary>
        public long SystemPaymentServiceProviderId { get; set; }
    }

    /// <summary>
    /// 支付方式绑定的设备
    /// </summary>
    public class PaymentMethodBindDeviceSelect
    {
        /// <summary>
        /// 设备主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 返回设备绑定的支付方式
    /// </summary>
    public class DeviceBindPaymentMethodDto
    {
        /// <summary>
        /// 设备标识
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 设备支付方式集合
        /// </summary>
        public List<DPaymentMethod> PaymentMethods { get; set; }
    }

    /// <summary>
    /// 设备的支付方式
    /// </summary>
    public class DPaymentMethod
    {
        /// <summary>
        /// 支付名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 支付类型（枚举类型：OrderPaymentTypeEnum）
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// 支付方式Id（M_PaymentMethod表的Id）
        /// </summary>
        public string MerchantId { get; set; }
    }
}