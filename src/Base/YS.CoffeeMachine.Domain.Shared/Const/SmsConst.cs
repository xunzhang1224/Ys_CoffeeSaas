namespace YS.CoffeeMachine.Domain.Shared.Const
{
    /// <summary>
    /// 短信模板CODE常量
    /// </summary>
    public class SmsConst
    {
        /// <summary>
        /// 短信登录验证码
        /// </summary>
        public const string LoginVerify = "SMS_495955989";

        /// <summary>
        /// 运营商注册
        /// </summary>
        public const string RegisterOperator = "SMS_496900347";

        /// <summary>
        /// 运营商忘记密码
        /// </summary>
        public const string OperatorForgotPassword = "SMS_486465090";

        /// <summary>
        /// 运营商重置密码
        /// </summary>
        public const string OperatorResetPassword = "SMS_496025154";

        /// <summary>
        /// 运营商在本机构创建账号
        /// </summary>
        public const string OperatorCreateAccount = "SMS_486505087";

        /// <summary>
        /// 新增机构同时创建超管账号
        /// </summary>
        public const string CreateSuperAdminWhenAdd = "SMS_495865141";

        /// <summary>
        /// 平台端重置用户密码
        /// </summary>
        public const string PlatformResetPassword = "SMS_496080129";

        /// <summary>
        /// 商户支付进件验证码
        /// </summary>
        public const string MerchantPaymentOnboardingVerificationCode = "SMS_479750194";

        /// <summary>
        /// 平台端创建商户账号
        /// </summary>
        public const string PlatformCreateMerchantAccount = "SMS_486540115";

        /// <summary>
        /// 机构审核通过
        /// </summary>
        public const string OrganizationAuditApproved = "SMS_486575132";

        /// <summary>
        /// 机构审核失败
        /// </summary>
        public const string OrganizationAuditFailed = "SMS_486335298";

        /// <summary>
        /// 注册平台账号
        /// </summary>
        public const string RegisterPlatformAccount = "SMS_491460251";

        /// <summary>
        /// 进件申请成功
        /// </summary>
        public const string MerchantApplymentSuccess = "SMS_496070188";

        /// <summary>
        /// 进件申请失败
        /// </summary>
        public const string MerchantApplymentFailed = "SMS_495915170";

        /// <summary>
        /// 进件申请签约
        /// </summary>
        public const string MerchantApplymentSign = "SMS_493245203";

        /// <summary>
        /// 您好！检测到位于${device_address}的机器（设备编号：${device_code}）物料不足，缺料处：${material_name}，检测时间：${detection_time}。为保证设备正常出品，请尽快安排人员补料处理。
        /// </summary>
        public const string SmsShortageTemplate = "SMS_496070186";

        /// <summary>
        /// 您好！检测到位于${device_address}的机器（设备编号：${device_code}）已于${time} 离线，请尽快前往现场处理或联系技术支持。
        /// </summary>
        public const string SmsOffLineTemplate = "SMS_497860120";

        /// <summary>
        /// 您好！检测到位于 ${device_address} 的设备 (设备编号: ${device_code})，经系统检测，${flushNames}于 ${time} 使用次数达预警值，需进行清洗，请及时处理。
        /// </summary>
        public const string SmsFulshWarningTemplate = "SMS_498325014";

        /// <summary>
        /// 您好！检测到位于${device_address}的机器（设备编号：${device_code}）发生故障，故障类型：${error_code}，故障时间：${detection_time}，处理建议：${advice}。为避免影响售卖，请尽快安排人员检查处理。
        /// </summary>
        public const string SmsErrTemplate = "SMS_496005171";
    }
}