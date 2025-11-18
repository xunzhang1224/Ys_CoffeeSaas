using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos
{
    /// <summary>
    /// 系统支付方式数据传输对象
    /// </summary>
    public class SystemPaymentMethodDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Desc:支付方式名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Desc:支付方式的图片
        /// Default:
        /// Nullable:False
        /// </summary>
        public string PaymentImage { get; set; } = null!;

        /// <summary>
        /// Desc:是否支持在线支付（true：支持）
        /// Default:
        /// Nullable:False
        /// </summary>
        public bool OnlinePayment { get; set; }

        /// <summary>
        /// Desc:是否支持安卓离线支付（true：支持）
        /// Default:
        /// Nullable:False
        /// </summary>
        public bool OfflinePayment { get; set; }

        /// <summary>
        /// Desc:支付方式支持的国家（SysDictData表的Code，多个用 | 拼接）
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Country { get; set; } = null!;

        /// <summary>
        /// Desc:支付平台对应的支付方式Id（支付平台PaymentMethod表的Id，只有OnlinePayment字段等于“true”时需要）
        /// Default:
        /// Nullable:True
        /// </summary>
        public long PaymentPlatformId { get; set; } = 0;

        /// <summary>
        /// 支付平台类型
        /// </summary>
        public PaymentPlatformTypeEnum PaymentPlatformType { get; set; }
    }

    /// <summary>
    /// 系统支付方式下拉框
    /// </summary>
    public class SystemPaymentMethodSelect
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 支付方式下拉框
    /// </summary>
    public class M_PaymentMethodSelect
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string Name { get; set; }
    }
}