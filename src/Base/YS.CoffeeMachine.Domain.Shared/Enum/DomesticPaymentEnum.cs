using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    #region 二级商户支付方式相关枚举

    /// <summary>
    /// 支付模式
    /// </summary>
    public enum PaymentModeEnum
    {
        /// <summary>
        /// 在线支付
        /// </summary>
        [Description("在线支付")]
        OnlinePayment = 0,

        /// <summary>
        /// 离线支付
        /// </summary>
        [Description("离线支付")]
        OfflinePayment = 1,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 5
    }

    /// <summary>
    /// 商户类型枚举
    /// </summary>
    public enum DomesticMerchantTypeEnum
    {
        /// <summary>
        /// 个人
        /// </summary>
        [Description("小微商户/个人卖家")]
        Individual = 1,

        /// <summary>
        /// 企业
        /// </summary>
        [Description("企业")]
        Enterprise = 2
    }

    /// <summary>
    /// 进件状态枚举
    /// </summary>
    public enum InternalOnboardingStatusEnum
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
        /// 商户已撤销授权
        /// </summary>
        [Description("已撤销")]
        Revoke = 3,

        /// <summary>
        /// 待提交
        /// </summary>
        [Description("待提交")]
        Pending = 4,

        /// <summary>
        /// 待签约
        /// </summary>
        [Description("待签约")]
        Signing = 5
    }

    /// <summary>
    /// 支付绑定的类型
    /// </summary>
    public enum BindTypeEnum
    {
        /// <summary>
        /// 全部设备
        /// </summary>
        [Description("全部设备")]
        AllDevice = 0,

        /// <summary>
        ///指定设备
        /// </summary>
        [Description("指定设备")]
        DefineEquipment = 1
    }

    /// <summary>
    /// 订单支付方式枚举类
    /// </summary>
    public enum OrderPaymentTypeEnum
    {
        /// <summary>
        /// 微信支付jsapi
        /// </summary>
        [Description("微信JsApi")]
        WxJsApi = 1,

        /// <summary>
        /// 支付宝支付jsapi
        /// </summary>
        [Description("支付宝JsApi")]
        AlipayJsApi = 2,

        /// <summary>
        /// 微信刷脸支付
        /// </summary>
        [Description("微信刷脸支付")]
        WxFacepay = 3,

        /// <summary>
        /// 支付宝刷脸支付
        /// </summary>
        [Description("支付宝刷脸支付")]
        AlipayFacepay = 4,

        /// <summary>
        /// 微信扫码支付
        /// </summary>
        [Description("微信扫码支付")]
        WxNativePay = 5
    }
    #endregion

    #region 支付宝进件相关枚举

    /// <summary>
    /// 商家类型
    /// </summary>
    public enum MerchantTypeEnum
    {
        /// <summary>
        /// 企业
        /// </summary>
        [Description("企业")]
        Enterprise = 1,

        /// <summary>
        /// 事业单位
        /// </summary>
        [Description("事业单位")]
        Institution = 2,

        /// <summary>
        /// 民办非企业组织
        /// </summary>
        [Description("民办非企业组织")]
        PrivateNonEnterprise = 3,

        /// <summary>
        /// 社会团体
        /// </summary>
        [Description("社会团体")]
        SocialOrganization = 4,

        /// <summary>
        /// 党政及国家机关
        /// </summary>
        [Description("党政及国家机关")]
        GovernmentAgency = 5,

        /// <summary>
        /// 个人商户
        /// </summary>
        [Description("个人商户")]
        IndividualBusiness = 6,

        /// <summary>
        /// 个体工商户
        /// </summary>
        [Description("个体工商户")]
        SoleProprietorship = 7
    }

    /// <summary>
    /// 证件类型
    /// </summary>
    public enum LegalCertTypeEnum
    {
        /// <summary>
        /// 大陆身份证
        /// </summary>
        [Description("大陆身份证")]
        MainlandIDCard = 100,

        /// <summary>
        /// 港澳居民往来内地通行证
        /// </summary>
        [Description("港澳居民往来内地通行证")]
        HKMacaoResidentPermit = 105,

        /// <summary>
        /// 台湾同胞往来大陆通行证
        /// </summary>
        [Description("台湾同胞往来大陆通行证")]
        TaiwanCompatriotPermit = 106,

        /// <summary>
        /// 外国人居留证
        /// </summary>
        [Description("外国人居留证")]
        ForeignerResidencePermit = 108,

        /// <summary>
        /// 警官证
        /// </summary>
        [Description("警官证")]
        PoliceOfficerCard = 109
    }

    /// <summary>
    /// 微信支付宝进件状态枚举
    /// </summary>
    public enum ApplymentFlowStatusEnum
    {
        /// <summary>
        /// 初始化状态 待提交到平台审核
        /// </summary>
        [Description("待提交")]
        Initialize = 0,

        /// <summary>
        /// 平台审核；提交到对应的支付宝/微信平台审核
        /// </summary>
        [Description("平台审核")]
        PlatformReview = 1,

        /// <summary>
        /// 已完结
        /// </summary>
        [Description("已完结")]
        Finish = 2,

        /// <summary>
        /// 失败 | 驳回
        /// </summary>
        [Description("驳回")]
        Failed = 3,

        /// <summary>
        /// 已冻结
        /// </summary>
        [Description("已冻结")]
        Frozen = 4,

        /// <summary>
        /// 商家取消授权
        /// </summary>
        [Description("商家取消授权")]
        MerchantCanceled = 5,

        /// <summary>
        /// 取消授权-提交支付平台
        /// </summary>
        [Description("取消授权")]
        Canceled = 6,

        /// <summary>
        /// 待签约
        /// </summary>
        [Description("待签约")]
        TobeSigned = 7,

        /// <summary>
        /// 待账户验证
        /// </summary>
        [Description("待账户验证")]
        AccountTeedVerify = 8,

        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")]
        AUDITING = 9
    }

    /// <summary>
    /// 手工进件枚举
    /// </summary>
    public enum ArtificialApplymentEnum
    {
        /// <summary>
        /// 否
        /// </summary>
        [Description("否")]
        No = 0,

        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        Yes = 1
    }
    #endregion

    #region 微信进件相关枚举

    ///// <summary>
    ///// 微信进件经营者身份证件类型
    ///// </summary>
    //public enum WxApplymentIdentityCertTypeEnum
    //{
    //    /// <summary>
    //    /// 中国大陆居民-身份证
    //    /// </summary>
    //    [Description("中国大陆居民-身份证")]
    //    IDENTIFICATION_TYPE_IDCARD,

    //    /// <summary>
    //    /// 其他国家或地区居民-护照
    //    /// </summary>
    //    [Description("其他国家或地区居民-护照")]
    //    IDENTIFICATION_TYPE_OVERSEA_PASSPORT,

    //    /// <summary>
    //    /// 中国香港居民-来往内地通行证
    //    /// </summary>
    //    [Description("中国香港居民-来往内地通行证")]
    //    IDENTIFICATION_TYPE_HONGKONG_PASSPORT,

    //    /// <summary>
    //    /// 中国澳门居民-来往内地通行证
    //    /// </summary>
    //    [Description("中国澳门居民-来往内地通行证")]
    //    IDENTIFICATION_TYPE_MACAO_PASSPORT,

    //    /// <summary>
    //    /// 中国台湾居民-来往大陆通行证
    //    /// </summary>
    //    [Description("中国台湾居民-来往大陆通行证")]
    //    IDENTIFICATION_TYPE_TAIWAN_PASSPORT,

    //    /// <summary>
    //    /// 外国人居留证
    //    /// </summary>
    //    [Description("外国人居留证")]
    //    IDENTIFICATION_TYPE_FOREIGN_RESIDENT,

    //    /// <summary>
    //    /// 港澳居民证
    //    /// </summary>
    //    [Description("港澳居民证")]
    //    IDENTIFICATION_TYPE_HONGKONG_MACAO_RESIDENT,

    //    /// <summary>
    //    /// 台湾居民证
    //    /// </summary>
    //    [Description("台湾居民证")]
    //    IDENTIFICATION_TYPE_TAIWAN_RESIDENT
    //}

    ///// <summary>
    ///// 微信特约商户进件主体类型枚举
    ///// </summary>
    //[Description("微信商户主体类型")]
    //public enum WxApplymentSubjectTypeEnum
    //{
    //    /// <summary>
    //    /// （小微）：无营业执照、免办理工商注册登记的实体商户
    //    /// </summary>
    //    [Description("小微商户/个人卖家")]
    //    SUBJECT_TYPE_MICRO,

    //    /// <summary>
    //    /// （个体户）：营业执照上的主体类型一般为个体户、个体工商户、个体经营；
    //    /// </summary>
    //    [Description("个体户")]
    //    SUBJECT_TYPE_INDIVIDUAL,

    //    /// <summary>
    //    /// （企业）：营业执照上的主体类型一般为有限公司、有限责任公司；
    //    /// </summary>
    //    [Description("企业")]
    //    SUBJECT_TYPE_ENTERPRISE,

    //    /// <summary>
    //    /// （政府机关）：包括各级、各类政府机关，如机关党委、税务、民政、人社、工商、商务、市监等；
    //    /// </summary>
    //    [Description("政府机关")]
    //    SUBJECT_TYPE_GOVERNMENT,

    //    /// <summary>
    //    /// （事业单位）：包括国内各类事业单位，如：医疗、教育、学校等单位；
    //    /// </summary>
    //    [Description("事业单位")]
    //    SUBJECT_TYPE_INSTITUTIONS,

    //    /// <summary>
    //    /// （社会组织）： 包括社会团体、民办非企业、基金会、基层群众性自治组织、农村集体经济组织等组织。
    //    /// </summary>
    //    [Description("社会组织")]
    //    SUBJECT_TYPE_OTHERS
    //}

    ///// <summary>
    ///// 微信进件记录状态
    ///// </summary>
    //[Description("微信进件记录状态")]
    //public enum WxApplymentStateEnum
    //{
    //    /// <summary>
    //    /// （编辑中）：提交申请发生错误导致，请尝试重新提交。
    //    /// </summary>
    //    [Description("编辑中")]
    //    APPLYMENT_STATE_EDITTING,

    //    /// <summary>
    //    /// （审核中）：申请单正在审核中，超级管理员用微信打开“签约链接”，完成绑定微信号后，申请单进度将通过微信公众号通知超级管理员，引导完成后续步骤。
    //    /// </summary>
    //    [Description("审核中")]
    //    APPLYMENT_STATE_AUDITING,

    //    /// <summary>
    //    /// （已驳回）：请按照驳回原因修改申请资料，超级管理员用微信打开“签约链接”，完成绑定微信号，后续申请单进度将通过微信公众号通知超级管理员。
    //    /// </summary>
    //    [Description("已驳回")]
    //    APPLYMENT_STATE_REJECTED,

    //    /// <summary>
    //    /// （待账户验证）：请超级管理员使用微信打开返回的“签约链接”，根据页面指引完成账户验证。
    //    /// </summary>
    //    [Description("待账户验证")]
    //    APPLYMENT_STATE_TO_BE_CONFIRMED,

    //    /// <summary>
    //    /// （待签约）：请超级管理员使用微信打开返回的“签约链接”，根据页面指引完成签约。
    //    /// </summary>
    //    [Description("待签约")]
    //    APPLYMENT_STATE_TO_BE_SIGNED,

    //    /// <summary>
    //    /// （开通权限中）：系统开通相关权限中，请耐心等待。
    //    /// </summary>
    //    [Description("开通权限中")]
    //    APPLYMENT_STATE_SIGNING,

    //    /// <summary>
    //    /// （已完成）：商户入驻申请已完成。
    //    /// </summary>
    //    [Description("已完成")]
    //    APPLYMENT_STATE_FINISHED,

    //    /// <summary>
    //    /// （已作废）：申请单已被撤销。
    //    /// </summary>
    //    [Description("已作废")]
    //    APPLYMENT_STATE_CANCELED,
    //}

    /// <summary>
    /// 支付平台
    /// </summary>
    public enum PaymentPlatformTypeEnum
    {
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        Wechat,

        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay
    }
    #endregion

    #region 进件短信通知类型

    /// <summary>
    /// 进件短信通知类型
    /// </summary>
    public enum ApplymentSmsNoticeTypeEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None,

        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Finished,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Failed,

        /// <summary>
        /// 需验证
        /// </summary>
        [Description("需验证")]
        Confirmed,

        /// <summary>
        /// 需签约授权
        /// </summary>
        [Description("需签约授权")]
        TobeSigned
    }
    #endregion

    #region 分账相关枚举

    /// <summary>
    /// 支付宝分账接收方类型
    /// </summary>
    public enum AlipayRoyaltyTypeEnum
    {
        [Description("支付宝账号对应的支付宝唯一用户号")]
        UserId,
        [Description("支付宝登录号")]
        LoginName,
        [Description("支付宝openId")]
        OpenId
    }
    #endregion
}