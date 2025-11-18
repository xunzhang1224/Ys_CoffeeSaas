using YS.Cabinet.Payment.WechatPay;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.DivideAccountsConfigDtos
{
    /// <summary>
    /// 支付分账配置信息
    /// </summary>
    public class DivideAccountsConfigDto
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public long SysPaymentMethodId { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public long MerchantId { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 分账比列%
        /// </summary>
        public decimal Bibliography { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        ///  接收方类型
        /// </summary>
        public WxProfitSharingReceiverTypeEnum? type { get; set; } = null;

        /// <summary>
        /// 接收方类型字符串
        /// </summary>
        public string TypeText { get { return type != null ? type.GetEnumDefaultValue() : string.Empty; } }

        /// <summary>
        /// 支付宝分账接收方类型
        /// </summary>
        public AlipayRoyaltyTypeEnum? alipayRoyaltyType { get; set; } = null;

        /// <summary>
        /// 支付宝分账接收方类型字符串
        /// </summary>
        public string AlipayRoyaltyTypeText { get { return alipayRoyaltyType != null ? alipayRoyaltyType.GetEnumDefaultValue() : string.Empty; } }

        /// <summary>
        /// 接收方账号
        /// </summary>
        public string? account { get; set; }

        /// <summary>
        /// 分账接收方全称  分账接收方真实姓名
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 与分账方的关系类型
        /// </summary>
        public WxProfitSharingRelationTypeEnum? relation_type { get; set; } = null;

        /// <summary>
        /// 与分账方的关系类型字符串
        /// </summary>
        public string RelationText { get { return relation_type != null ? relation_type.GetEnumDefaultValue() : string.Empty; } }

        /// <summary>
        /// 绑定的设备
        /// </summary>
        public List<long>? DeviceIds { get; set; } = new List<long>();

        /// <summary>
        /// Desc:是否启用（枚举类型：EnabledEnum）
        /// Default:1
        /// Nullable:False
        /// </summary>
        public EnabledEnum IsEnabled { get; set; }
    }

    /// <summary>
    /// 支付分账配置信息入参
    /// </summary>
    public class DivideAccountsConfigInput : QueryRequest
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public string? MerchantName { get; set; } = null;

        /// <summary>
        /// 接收方账号
        /// </summary>
        public string? account { get; set; } = null;

        /// <summary>
        /// 分账接收方全称  分账接收方真实姓名
        /// </summary>
        public string? name { get; set; } = null;
    }

    /// <summary>
    /// 支付分账配置信息出参
    /// </summary>
    public class DivideAccountsConfigOutput : DivideAccountsConfigDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 商户Id
        /// </summary>
        public string SystemPaymentMethodId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTimeText { get; set; }
    }

    /// <summary>
    /// 删除分账配置入参
    /// </summary>
    public class DeleteDiviDeAccountConfigInput : DivideAccountsConfigDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
    }
}