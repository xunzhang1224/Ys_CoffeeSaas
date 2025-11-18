using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Const
{
    /// <summary>
    /// cap常量
    /// </summary>
    public class CapConst
    {
        /// <summary>
        /// 设备绑定
        /// </summary>
        public const string DeviceBind = "1100";

        /// <summary>
        /// 设备初始化
        /// </summary>
        public const string DeviceInit = "1000";

        /// <summary>
        /// 指标上报
        /// </summary>
        public const string MetricCap = "MetricCap";

        /// <summary>
        /// 属性上报
        /// </summary>
        public const string AttributeReporting = "AttributeReporting";

        /// <summary>
        /// 能力上报
        /// </summary>
        public const string AbilityReporting = "AbilityReporting";

        /// <summary>
        /// 能力配置上报
        /// </summary>
        public const string CapabilityConfigure = "CapabilityConfigure";

        /// <summary>
        /// 能力配置下发
        /// </summary>
        public const string SeedCapabilityCfg = "SeedCapabilityCfg";

        /// <summary>
        /// 通用下发
        /// </summary>
        public const string GeneralSeed = "GeneralSeed";

        /// <summary>
        /// 创建操作日志
        /// </summary>
        public const string CreateOperationLog = "CreateOperationLog";

        /// <summary>
        /// 平台操作日志
        /// </summary>
        public const string PlatformOperationLog = "PlatformOperationLog";

        /// <summary>
        /// 创建操作日志
        /// </summary>
        public const string UpdateOperationLog = "UpdateOperationLog";

        /// <summary>
        /// 推送应用平台更新结果
        /// </summary>
        public const string SeedYYPSoftUpdate = "SeedYYPSoftUpdate";

        /// <summary>
        /// 指标通知
        /// </summary>
        public const string MetricNotice = "MetricNotice";

        /// <summary>
        /// 邮件通知
        /// </summary>
        public const string Email = "Email";

        /// <summary>
        /// 邮件通知
        /// </summary>
        public const string Sms = "Sms";

        /// <summary>
        /// 记录消息通知
        /// </summary>
        public const string CreateNotityMsg = "CreateNotityMsg";

        /// <summary>
        /// 商户进件回调
        /// </summary>
        public const string MerchantIncomingCallback = "MerchantIncomingCallback";

        /// <summary>
        /// 在线支付成功回调
        /// </summary>
        public const string OnlineOrderPaymentSuccess = "OnlineOrderPaymentSuccess";

        /// <summary>
        /// 设备上下线日志
        /// </summary>
        public const string CreateDeviceOnlineLog = "CreateDeviceOnlineLog";

        #region 微信支付宝支付相关

        /// <summary>
        /// 微信进件提交
        /// </summary>
        public const string WechatMerchantIncomingSubmit = "WechatMerchantIncomingSubmit";

        /// <summary>
        /// 微信支付成功异步回调订阅
        /// </summary>
        public const string WechatPaySuccessCallback = "WechatPaySuccessCallback";

        /// <summary>
        /// 支付宝进件提交
        /// </summary>
        public const string AlipayMerchantIncomingSubmit = "AlipayMerchantIncomingSubmit";

        /// <summary>
        /// 支付宝异步回调订阅
        /// </summary>
        public const string AlipaySuccessCallback = "AlipaySuccessCallback";

        /// <summary>
        /// 订单退款
        /// </summary>
        public const string DomesticPaymentOrderRefund = "DomesticPaymentOrderRefund";

        /// <summary>
        /// 订单退款状态同步
        /// </summary>
        public const string DomesticPaymentOrderRefundSyncStatus = "DomesticPaymentOrderRefundSyncStatus";
        #endregion
    }
}
