namespace YS.CoffeeMachine.Application.Dtos.DomesticPayment
{
    /// <summary>
    /// 支付配置信息
    /// </summary>
    public class PaymentConfigDto
    {
        /// <summary>
        /// 服务商商户号或服务商的APPID
        /// 微信支付：服务商商户号（对应微信支付的sp_mchid）
        /// 支付宝支付：服务商的APPID（对应支付宝支付的APPID）
        /// </summary>
        public string? SPMerchantId { get; set; }

        /// <summary>
        /// 小程序AppId
        /// </summary>
        public string AppletAppID { get; set; }

        /// <summary>
        /// 微信APIv2密钥或支付宝公钥
        /// 微信支付：微信APIv2密钥
        /// 支付宝支付： 支付宝公钥
        /// </summary>
        public string? AppKey { get; set; }

        /// <summary>
        /// 微信APIv3密钥或支付宝私钥
        /// 微信支付：微信APIv3密钥
        /// 支付宝支付： 支付宝私钥（开发者私钥）
        /// </summary>
        public string? ApiV3Key { get; set; }

        /// <summary>
        /// 异步通知全局地址
        /// </summary>
        public string NotifyUrl { get; set; }

        #region 微信商户证书

        /// <summary>
        /// 证书路径（只有微信支付有）
        /// </summary>
        public string? CretFileUrl { get; set; } = "/cert/apiclient_cert.p12";

        /// <summary>
        /// 证书密码（只有微信支付有）
        /// </summary>
        public string? CretPassWrod { get; set; } = "1511125271";
        #endregion
    }
}