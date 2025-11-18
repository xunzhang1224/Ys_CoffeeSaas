using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment
{
    /// <summary>
    /// 支付的服务商表(微信|支付宝)
    /// </summary>
    public class SystemPaymentServiceProvider : BaseEntity
    {
        /// <summary>
        /// 服务商商户号或服务商的APPID
        /// 微信支付：服务商商户号（对应微信支付的sp_mchid）
        /// 支付宝支付：服务商的APPID（对应支付宝支付的APPID）
        /// </summary>
        public string SpMerchantId { get; private set; }

        /// <summary>
        /// 服务商名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 小程序appId
        /// </summary>
        public string AppletAppID { get; private set; }

        /// <summary>
        /// 微信APIv2密钥或支付宝公钥
        /// 微信支付：微信APIv2密钥
        /// 支付宝支付： 支付宝公钥
        /// </summary>
        public string AppKey { get; private set; }

        /// <summary>
        /// 微信APIv3密钥或支付宝应用私钥
        /// 微信支付：微信APIv3密钥
        /// 支付宝支付： 支付宝应用私钥（开发者私钥）
        /// </summary>
        public string ApiV3Key { get; private set; }

        /// <summary>
        /// 默认异步通知地址
        /// </summary>
        public string NotifyUrl { get; private set; }

        /// <summary>
        /// 支付平台
        /// </summary>
        public PaymentPlatformTypeEnum PaymentPlatformType { get; private set; }

        /// <summary>
        /// 是否默认服务商(IsDefaultEnum枚举)
        /// </summary>
        public IsDefaultEnum IsDefault { get; private set; }

        #region 只有微信支付有
        /// <summary>
        /// 证书路径（只有微信支付有）
        /// </summary>
        public string CretFileUrl { get; private set; }

        /// <summary>
        /// 证书密码（只有微信支付有）
        /// </summary>
        public string CretPassWrod { get; private set; }

        /// <summary>
        /// 微信平台证书序列号（只有微信支付有）
        /// </summary>
        public string PlatformSerialNumber { get; private set; }

        /// <summary>
        /// 微信平台证书公钥（只有微信支付有）
        /// </summary>
        public string PlatformPublicKey { get; private set; }
        #endregion
    }
}