namespace YS.CoffeeMachine.Domain.Shared.Const
{
    /// <summary>
    /// 缓存常量
    /// </summary>
    public class CacheConst
    {
        #region redis

        /// <summary>
        /// 设备订单前缀
        /// </summary>
        public const string DeviceOrderCode = "WB{0}";

        /// <summary>
        /// 订单号
        /// </summary>
        public const string OrderCode = "NB{0}";

        /// <summary>
        /// 多语言缓存
        /// </summary>
        public const string MultilingualAll = "MultilingualAll";

        ///// <summary>
        ///// 前端使用的语言文本缓存
        ///// 语种code
        ///// </summary>
        //public const string Multilingual = "Multilingual_{0}";

        ///// <summary>
        ///// 默认语种
        ///// </summary>
        //public const string DefaultLanguage = "DefaultLanguage";

        /// <summary>
        /// NotityToken
        /// </summary>
        public const string NotityToken = "NotityToken";

        /// <summary>
        /// 操作日志自增长code 的key
        /// </summary>
        public const string OperationLogKey = "OperationLogKey";

        /// <summary>
        /// 设备基础信息 的key
        /// </summary>
        public const string DeviceBaseKey = "DeviceBaseKey";

        /// <summary>
        /// 设备在线key
        /// </summary>
        public const string DeviceOnlineKey = "DeviceOnlineKey";

        /// <summary>
        /// 企业注册邮箱验证码
        /// </summary>
        public const string RegisterEnterpriseEMailVCodeKey = "Verify:RegisterEnterpriseEMailVCodeKey:{0}";

        /// <summary>
        /// 企业注册短信验证码
        /// </summary>
        public const string RegisterEnterpriseSMSVCodeKey = "Verify:RegisterEnterpriseSMSVCodeKey:{0}";

        /// <summary>
        /// 商户进件手机验证码
        /// </summary>
        public const string MerchantOnboardingPhoneVCodeKey = "Verify:MerchantOnboardingPhoneVCodeKey:{0}";

        /// <summary>
        /// 微信进件银行卡列表缓存
        /// </summary>
        public const string WxBankListCacheKey = "DomesticPayment:WxBankListCache:{0}:{1}";

        /// <summary>
        /// 省份缓存 ProvinceCache
        /// </summary>
        public const string ProvinceCache = "DomesticPayment:ProvinceCache";

        /// <summary>
        /// 城市缓存 CityCache:{0}
        /// </summary>
        public const string CityCache = "DomesticPayment:CityCache:{0}";
        #endregion

        /// <summary>
        /// 系统支付方式缓存key
        /// </summary>
        public const string SystemPaymentMethodsKey = "DomesticPayment:SystemPaymentMethodsKey";

        /// <summary>
        /// 系统支付服务商缓存key
        /// </summary>
        public const string SystemPaymentServiceProviderKey = "DomesticPayment:SystemPaymentServiceProviderKey:{0}";

        /// <summary>
        /// 回调时获取系统支付服务商缓存key
        /// </summary>
        public const string ServiceProviderKey = "DomesticPayment:ServiceProviderProviderKey:{0}";
        #region 支付

        /// <summary>
        /// 支付配置信息缓存 PaymentConfigCache:{0}
        /// </summary>
        public const string PaymentConfigCache = "PaymentConfigCache:{0}";
        #endregion

        #region 本地缓存
        /// <summary>
        /// 当前语种字典
        /// </summary>
        public const string CacheCurrentLanguageDic = "CacheCurrentLanguageDic";

        /// <summary>
        /// 当前语种
        /// </summary>
        public const string CacheCurrentLanguage = "CacheCurrentLanguage";

        /// <summary>
        /// 下发指令消息唯一idKEY
        /// </summary>
        public const string DownSendEventKey = "DownSendEventKey";

        #endregion

        /// <summary>
        /// 设备初始化密钥
        /// </summary>
        public const string DeviceInitializationKey = "DeviceInitializationKey:{0}";

        /// <summary>
        /// 设备基础信息
        /// </summary>
        public const string VendBaciInfo = "Vend_BaciInfo:{0}";

        /// <summary>
        /// 设备通道
        /// </summary>
        public const string VendChannelKey = "YS.Coffee_Vend_Channel:{0}";

        /// <summary>
        /// 连接通道密钥
        /// </summary>
        public const string ConnectionChannelKey = "connection:{0}";

        /// <summary>
        /// 已亡的设备号
        /// </summary>
        public const string MidDead = "888844444";

        /// <summary>
        /// 设备预警配置
        /// </summary>
        public const string WarningConfigs = "WarningConfigs";

        /// <summary>
        /// 邮件通知
        /// </summary>
        public const string Email = "Email:{0}:{1}";

        /// <summary>
        /// 短信通知
        /// </summary>
        public const string Sms = "Sms:{0}:{1}";

        /// <summary>
        /// 离线任务
        /// </summary>
        public const string OffOnlineTask = "OffOnlineTask:{0}";

        /// <summary>
        /// 故障码缓存Key
        /// </summary>
        public const string FaultCodeKey = "faultcode:{0}";
    }
}
