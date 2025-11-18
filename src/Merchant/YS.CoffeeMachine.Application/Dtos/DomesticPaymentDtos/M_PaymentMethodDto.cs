using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos
{
    /// <summary>
    /// 二级商户支付方式查询输入参数
    /// </summary>
    public class M_PaymentMethodInput : QueryRequest
    {
        /// <summary>
        /// 系统支付方式Id
        /// </summary>
        public long SystemPaymentMethodId { get; set; }
    }

    /// <summary>
    /// 二级商户支付方式数据传输对象
    /// </summary>
    public class M_PaymentMethodDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 进件申请单Id
        /// </summary>
        public long? ApplymentId { get; set; }

        /// <summary>
        /// Desc:系统支付方式表的Id（SystemPaymentMethod表的Id）
        /// Default:
        /// Nullable:False
        /// </summary>
        public long SystemPaymentMethodId { get; set; }

        /// <summary>
        /// Desc:进件手机
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Phone { get; set; } = null!;

        /// <summary>
        /// 二级商户支付方式的商户类型
        /// </summary>
        public DomesticMerchantTypeEnum? MerchantType { get; set; } = null;

        /// <summary>
        /// 支付模式
        /// </summary>
        public PaymentModeEnum PaymentMode { get; set; } = PaymentModeEnum.OnlinePayment;

        /// <summary>
        /// Desc:支付进件状态（枚举类型：InternalOnboardingStatusEnum）
        /// Default:
        /// Nullable:False
        /// </summary>
        public InternalOnboardingStatusEnum PaymentEntryStatus { get; set; }

        /// <summary>
        /// 进件流程状态
        /// </summary>
        public ApplymentFlowStatusEnum? FlowStatus { get; set; }

        /// <summary>
        /// Desc:支付绑定的类型（ 0:全部设备，1：指定设备）
        /// Default:
        /// Nullable:False
        /// </summary>
        public BindTypeEnum BindType { get; set; } = BindTypeEnum.AllDevice;

        /// <summary>
        /// Desc:是否启用（枚举类型：EnabledEnum）
        /// Default:1
        /// Nullable:False
        /// </summary>
        public EnabledEnum IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Desc:子商户Id
        /// Default:
        /// Nullable:False
        /// </summary>
        public string MerchantId { get; set; } = null!;

        /// <summary>
        /// Desc:支付的服务商表（SystemPaymentServiceProvider表的Id）
        /// Default:
        /// Nullable:False
        /// </summary>
        public long SystemPaymentServiceProviderId { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string? RejectReason { get; set; } = null;

        /// <summary>
        /// 签约链接
        /// </summary>
        public string? SignUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}