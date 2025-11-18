namespace YS.CoffeeMachine.Domain.Shared.Const
{
    /// <summary>
    /// 通用常量
    /// </summary>
    public class CommonConst
    {
        #region(订单支付方式)

        /// <summary>
        /// 支付宝支付的Id(SystemPaymentMethod表的Id)
        /// </summary>
        public const long AlipaPaymenteId = 1001;

        /// <summary>
        /// 微信支付的Id(SystemPaymentMethod表的Id)
        /// </summary>
        public const long WechatPaymentId = 1002;

        /// <summary>
        /// 其他支付的Id(SystemPaymentMethod表的Id)
        /// </summary>
        public const long OtherPaymentId = 1003;

        /// <summary>
        /// 支付宝刷脸支付的Id(SystemPaymentMethod表的Id)
        /// </summary>
        public const long AlipaFacePaymentId = 2013;

        /// <summary>
        /// 支付宝JSAPI支付的Id(SystemPaymentMethod表的Id)
        /// </summary>
        public const long AlipaJSAPIPaymentId = 2014;

        /// <summary>
        /// 微信JSAPI支付的Id(SystemPaymentMethod表的Id)
        /// </summary>
        public const long WechatJSAPIPaymentId = 2015;

        /// <summary>
        /// 微信刷脸支付的Id(SystemPaymentMethod表的Id)
        /// </summary>
        public const long WechatFacePaymentId = 2016;
        #endregion

        #region 故障码常量

        /// <summary>
        /// 支付成功回调超时，不进行出货（设备故障代码表的故障代码(FaultCode表的Code)）
        /// </summary>
        public const string PaymentSuccessCallbackTimeout = "支付成功但回调超时";

        #endregion

        #region 物联网卡接口查询信息

        /// <summary>
        /// 物联网卡接口地址
        /// </summary>
        public const string IotCardApiUrl = "https://i3.ourvend.com";

        /// <summary>
        /// 缓存物联网卡策略天数
        /// </summary>
        public const int IotCardPolicyCacheDays = 7;
        #endregion
    }
}