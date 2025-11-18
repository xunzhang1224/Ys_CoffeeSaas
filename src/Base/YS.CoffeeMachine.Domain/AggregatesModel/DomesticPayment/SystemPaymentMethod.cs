using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment
{
    /// <summary>
    /// 系统支付方式
    /// </summary>
    public class SystemPaymentMethod : BaseEntity
    {
        /// <summary>
        /// Desc:支付方式名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Name { get; private set; } = null!;

        /// <summary>
        /// Desc:父支付方式Id（SystemPaymentMethod表的Id，如果自己就是一级，这里就是0）
        /// Default:0
        /// Nullable:False
        /// </summary>
        public long FatherId { get; private set; }

        /// <summary>
        /// Desc:支付方式的图片
        /// Default:
        /// Nullable:False
        /// </summary>
        public string PaymentImage { get; private set; } = null!;

        /// <summary>
        /// Desc:是否支持在线支付（true：支持）
        /// Default:
        /// Nullable:False
        /// </summary>
        public bool OnlinePayment { get; private set; }

        /// <summary>
        /// Desc:是否支持安卓离线支付（true：支持）
        /// Default:
        /// Nullable:False
        /// </summary>
        public bool OfflinePayment { get; private set; }

        /// <summary>
        /// Desc:支付方式支持的国家（SysDictData表的Code，多个用 | 拼接）
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Country { get; private set; } = null!;

        /// <summary>
        /// Desc:多语言（SysLanguageText表的Code）
        /// Default:
        /// Nullable:False
        /// </summary>
        public string LanguageTextCode { get; private set; } = null!;

        /// <summary>
        /// Desc:是否启用（枚举类型：EnabledEnum）
        /// Default:1
        /// Nullable:False
        /// </summary>
        public EnabledEnum IsEnabled { get; private set; }

        /// <summary>
        /// Desc:支付平台对应的支付方式Id（支付平台SystemPaymentServiceProvider表的Id，只有OnlinePayment字段等于“true”时需要）
        /// Default:
        /// Nullable:True
        /// </summary>
        public long PaymentPlatformId { get; private set; } = 0;
    }
}