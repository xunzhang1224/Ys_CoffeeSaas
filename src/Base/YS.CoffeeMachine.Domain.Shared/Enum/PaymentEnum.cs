using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 支付方式枚举
    /// </summary>
    public enum PaymentEnum
    {
        [Description("在线支付")]
        PaymentOnline,

        [Description("离线支付")]
        PaymentOffline
    }

    /// <summary>
    /// 支付方式枚举
    /// </summary>
    public enum PayTypeEnum
    {
        /// <summary>
        /// 现金
        /// </summary>
        [Description("现金")]
        Cash,

        /// <summary>
        /// 刷卡
        /// </summary>
        [Description("刷卡")]
        Card,

        /// <summary>
        /// IPAY88
        /// </summary>
        [Description("IPAY88")]
        IPAY88,

        /// <summary>
        /// 微信扫码支付
        /// </summary>
        [Description("微信扫码支付")]
        WxNativePay,

        /// <summary>
        /// 支付宝扫码支付
        /// </summary>
        [Description("支付宝扫码支付")]
        AlipayJsApi,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        OTHER
    }

    /// <summary>
    /// 进件状态
    /// </summary>
    public enum PaymentConfigStatueEnum
    {
        /// <summary>
        /// 进件中
        /// </summary>
        [Description("进件中")]
        Onboarding = 0,

        /// <summary>
        /// 进件成功
        /// </summary>
        [Description("进件成功")]
        OnboardingSuccess = 1,

        /// <summary>
        /// 进件失败
        /// </summary>
        [Description("进件失败")]
        OnboardingFail = 2,

        /// <summary>
        /// 已撤销
        /// </summary>
        [Description("已撤销")]
        Revoke = 3,

        /// <summary>
        /// 待提交
        /// </summary>
        [Description("待提交")]
        Pending = 4
    }

    /// <summary>
    /// 币种位置
    /// </summary>
    public enum PaymentCurrencyLocatonEnum
    {
        /// <summary>
        /// 币种前
        /// </summary>
        [Description("币种前")]
        CurrencyBefore,

        /// <summary>
        /// 币种后
        /// </summary>
        [Description("币种后")]
        CurrencyAfter,
    }

    /// <summary>
    /// 支付平台回调主题
    /// </summary>
    public enum RequestTopicEnum
    {
        /// <summary>
        /// 商户进件状态变化通知
        /// </summary>
        [Description("商户进件状态变化通知")]
        MerchantIncomingChanges = 10001,

        /// <summary>
        /// 订单支付成功消息通知(在线支付)
        /// </summary>
        [Description("订单支付成功消息通知(在线支付)")]
        OnlineOrderPaymentSuccessful = 10016,

        /// <summary>
        /// 订单支付成功消息通知(离线支付)
        /// </summary>
        [Description("订单支付成功消息通知(离线支付)")]
        OfflineOrderPaymentSuccessful = 10017,

        /// <summary>
        /// 订单支付失败消息通知
        /// </summary>
        [Description("订单支付失败消息通知")]
        OrderPaymentFail = 10018,

        /// <summary>
        /// 取消支付消息通知
        /// </summary>
        [Description("取消支付消息通知")]
        CancelPayment = 10019,

        /// <summary>
        /// 订单退款成功消息通知
        /// </summary>
        [Description("订单退款成功消息通知")]
        OrderRefundSuccess = 10020
    }
}
