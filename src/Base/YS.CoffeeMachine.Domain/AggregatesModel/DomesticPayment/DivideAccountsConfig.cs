using YS.Cabinet.Payment.WechatPay;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment
{
    /// <summary>
    /// 支付分账配置信息
    /// </summary>
    public class DivideAccountsConfig : EnterpriseBaseEntity
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public long SysPaymentMethodId { get; private set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public long MerchantId { get; private set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; private set; }

        /// <summary>
        /// 分账比列%
        /// </summary>
        public decimal Bibliography { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; private set; }

        /// <summary>
        ///  接收方类型
        /// </summary>
        public WxProfitSharingReceiverTypeEnum? type { get; private set; }

        /// <summary>
        /// 支付宝接收方类型
        /// </summary>
        public AlipayRoyaltyTypeEnum? AlipayRoyaltyType { get; private set; }

        /// <summary>
        /// 接收方账号
        /// </summary>
        public string? account { get; private set; }

        /// <summary>
        /// 分账接收方全称  分账接收方真实姓名
        /// </summary>
        public string? name { get; private set; }

        /// <summary>
        /// 与分账方的关系类型
        /// </summary>
        public WxProfitSharingRelationTypeEnum? relation_type { get; private set; }

        /// <summary>
        /// 绑定的设备
        /// </summary>
        public List<long>? VendCodes { get; private set; } = new List<long>();

        /// <summary>
        /// Desc:是否启用（枚举类型：EnabledEnum）
        /// Default:1
        /// Nullable:False
        /// </summary>
        public EnabledEnum IsEnabled { get; private set; }

        /// <summary>
        /// 保护构造
        /// </summary>
        protected DivideAccountsConfig() { }

        /// <summary>
        /// 添加分账配置
        /// </summary>
        /// <param name="sysPaymentMethodId"></param>
        /// <param name="merchantId"></param>
        /// <param name="merchantName"></param>
        /// <param name="bibliography"></param>
        /// <param name="remarks"></param>
        /// <param name="type"></param>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <param name="relation_type"></param>
        /// <param name="vendCodes"></param>
        public DivideAccountsConfig(long sysPaymentMethodId, long merchantId, string merchantName, decimal bibliography, string remarks, WxProfitSharingReceiverTypeEnum? type, string? account, string? name,
            WxProfitSharingRelationTypeEnum? relation_type, List<long>? vendCodes)
        {
            SysPaymentMethodId = sysPaymentMethodId;
            MerchantId = merchantId;
            MerchantName = merchantName;
            Bibliography = bibliography;
            Remarks = remarks;
            this.type = type;
            this.account = account;
            this.name = name;
            this.relation_type = relation_type;
            VendCodes = vendCodes;
            IsEnabled = EnabledEnum.Enable;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sysPaymentMethodId"></param>
        /// <param name="merchantId"></param>
        /// <param name="merchantName"></param>
        /// <param name="bibliography"></param>
        /// <param name="remarks"></param>
        /// <param name="type"></param>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <param name="relation_type"></param>
        /// <param name="vendCodes"></param>
        public void Update(long sysPaymentMethodId, long merchantId, string merchantName, decimal bibliography, string remarks, WxProfitSharingReceiverTypeEnum? type, string? account, string? name,
            WxProfitSharingRelationTypeEnum? relation_type, List<long>? vendCodes)
        {
            SysPaymentMethodId = sysPaymentMethodId;
            MerchantId = merchantId;
            MerchantName = merchantName;
            Bibliography = bibliography;
            Remarks = remarks;
            this.type = type;
            this.account = account;
            this.name = name;
            this.relation_type = relation_type;
            VendCodes = vendCodes;
            IsEnabled = EnabledEnum.Enable;
        }

        /// <summary>
        /// 更新启用状态
        /// </summary>
        /// <param name="isEnabled"></param>
        public void UpdateIsEnabled(EnabledEnum isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}