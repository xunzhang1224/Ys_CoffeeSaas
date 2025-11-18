using YS.Cabinet.Payment.WechatPay;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.Shared.Enum;
using WxApplymentIdentityCertTypeEnum = YS.Cabinet.Payment.WechatPay.WxApplymentIdentityCertTypeEnum;

namespace YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment
{
    /// <summary>
    /// 商户微信进件申请表
    /// </summary>
    public class M_PaymentWechatApplyments : EnterpriseBaseEntity
    {
        /// <summary>
        /// 微信支付申请单号
        /// </summary>
        public string? ApplymentId { get; set; } = null;

        /// <summary>
        /// 商户支付方式表的OriginId(Me_PaymentMethod表的OriginId)
        /// </summary>
        public long PaymentOriginId { get; set; }

        /// <summary>
        /// 经营者/法人证件类型
        /// </summary>
        public WxApplymentIdentityCertTypeEnum IdDocType { get; set; }

        /// <summary>
        /// 身份证姓名
        /// </summary>
        public string IdCardName { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCardNumber { get; set; }

        /// <summary>
        /// 身份证居住地址
        /// </summary>
        public string IdCardAddress { get; set; }

        /// <summary>
        /// 身份证有效开始时间
        /// </summary>
        public string IdCardValidTimeBegin { get; set; }

        /// <summary>
        /// 身份证有效结束时间
        /// </summary>
        public string IdCardValidTime { get; set; }

        /// <summary>
        /// 身份证人像面照片
        /// </summary>
        public string IdCardCopy { get; set; }

        /// <summary>
        /// 身份证国徽面照片
        /// </summary>
        public string IdCardNational { get; set; }

        /// <summary>
        /// 联系人手机
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 商户简称
        /// </summary>
        public string MerchantShortName { get; set; }

        /// <summary>
        /// 客服电话
        /// </summary>
        public string ServicePhone { get; set; }

        /// <summary>
        /// 经营场所地址省份编码
        /// </summary>
        public string? BizProvinceCode { get; set; }

        /// <summary>
        /// 经营场所地址城市编码
        /// </summary>
        public string? BizCityCode { get; set; }

        /// <summary>
        /// 经营详细地址
        /// </summary>
        public string? BizStoreAddress { get; set; }

        /// <summary>
        /// 门店门头照片
        /// </summary>
        public string? StoreEntrancePic { get; set; }

        /// <summary>
        /// 门店环境照片
        /// </summary>
        public string? IndoorPic { get; set; }

        /// <summary>
        /// 主体类型
        /// </summary>
        public WxApplymentSubjectTypeEnum OrganizationType { get; set; }

        /// <summary>
        /// 证件扫描件
        /// </summary>
        public string? BusinessLicenseCopy { get; set; }

        /// <summary>
        /// 证件注册号
        /// </summary>
        public string? BusinessLicenseNumber { get; set; }

        /// <summary>
        /// 营业期限有效开始时间
        /// </summary>
        public string? BusinessTimeBegin { get; set; } = null;

        /// <summary>
        /// 营业期限有效结束时间
        /// </summary>
        public string? BusinessTimeEnd { get; set; } = null;

        /// <summary>
        /// 商户名称
        /// </summary>
        public string? MerchantName { get; set; }

        /// <summary>
        /// 经营者/法定代表人姓名
        /// </summary>
        public string? LegalPerson { get; set; }

        /// <summary>
        /// 营业执照地址
        /// </summary>
        public string? LicenseAddress { get; set; }

        /// <summary>
        /// 单位证明函照片
        /// </summary>
        public string? CertificateLetterCopy { get; set; }

        /// <summary>
        /// 结算银行账户开户名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 结算银行账户开户银行
        /// </summary>
        public string AccountBank { get; set; }

        /// <summary>
        /// 结算银行别名代码
        /// </summary>
        public string? BankAliasCode { get; set; }

        /// <summary>
        /// 结算银行账户银行账号
        /// </summary>
        public string? AccountNumber { get; set; }

        /// <summary>
        /// 结算银行账户开户银行省编码
        /// </summary>
        public string? BankProvinceCode { get; set; }

        /// <summary>
        /// 结算银行账户开户银行市编码
        /// </summary>
        public string? BankCityCode { get; set; }

        /// <summary>
        /// 结算银行账户开户银行联行号
        /// </summary>
        public string? BankBranchId { get; set; }

        /// <summary>
        /// 结算银行账户开户银行全称 （含支行）
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// 申请状态
        /// </summary>
        public WxApplymentStateEnum? ApplymentState { get; set; }

        /// <summary>
        /// 申请状态描述
        /// </summary>
        public string? ApplymentStateDesc { get; set; }

        /// <summary>
        /// 签约状态
        /// </summary>
        public string? SignState { get; set; }

        /// <summary>
        /// 签约链接
        /// </summary>
        public string? SignUrl { get; set; }

        /// <summary>
        /// 二级商户号
        /// </summary>
        public string? SubMchId { get; set; }

        /// <summary>
        /// 汇款账户验证信息付款户名
        /// </summary>
        public string? FAccountName { get; set; }

        /// <summary>
        /// 汇款账户验证信息付款卡号
        /// </summary>
        public string? FAccountNo { get; set; }

        /// <summary>
        /// 汇款金额
        /// </summary>
        public int? PayAmount { get; set; }

        /// <summary>
        /// 汇款账户验证信息收款卡号
        /// </summary>
        public string? DestinationAccountNumber { get; set; }

        /// <summary>
        /// 汇款账户验证信息收款户名
        /// </summary>
        public string? DestinationAccountName { get; set; }

        /// <summary>
        /// 汇款账户验证信息收款账户的开户银行名称
        /// </summary>
        public string? DestinationAccountBank { get; set; }

        /// <summary>
        /// 汇款账户验证信息收款账户的省市
        /// </summary>
        public string? DestinationCity { get; set; }

        /// <summary>
        /// 商户汇款时，需要填写的备注信息。
        /// </summary>
        public string? DestinationRemark { get; set; }

        /// <summary>
        /// 请在此时间前完成汇款
        /// </summary>
        public string? Deadline { get; set; }

        /// <summary>
        /// 法人验证链接
        /// </summary>
        public string? LegalValidationUrl { get; set; }

        /// <summary>
        /// 驳回原因详情
        /// </summary>
        public string? AuditDetail { get; set; }

        /// <summary>
        /// 受理状态
        /// </summary>
        public string? ApplyState { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string? RejectReason { get; set; }

        /// <summary>
        /// 进件流程状态
        /// </summary>
        public ApplymentFlowStatusEnum FlowStatus { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public long? VerifyUserId { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? VerifyTime { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public long? UpdateUserId { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// 手工进件标识(ArtificialApplymentEnum枚举)
        /// </summary>
        public ArtificialApplymentEnum? IsArtificialApplyment { get; set; }

        /// <summary>
        /// 保护构造函数
        /// </summary>
        protected M_PaymentWechatApplyments() { }

        /// <summary>
        /// 设置主键Id
        /// </summary>
        /// <param name="id"></param>
        public void SetId(long id)
        {
            Id = id;
        }

        /// <summary>
        /// 设置进件流程状态
        /// </summary>
        /// <param name="flowStatus"></param>
        public void SetFlowStatus(ApplymentFlowStatusEnum flowStatus)
        {
            FlowStatus = flowStatus;
        }

        /// <summary>
        /// 设置手工进件标识
        /// </summary>
        /// <param name="artificialApplyment"></param>
        public void SetArtificialApplyment(ArtificialApplymentEnum artificialApplyment)
        {
            IsArtificialApplyment = artificialApplyment;
        }

        /// <summary>
        /// 更新进件状态信息
        /// </summary>
        /// <param name="flowStatus"></param>
        /// <param name="applymentId"></param>
        /// <param name="wxApplymentState"></param>
        /// <param name="rejectReason"></param>
        /// <param name="auditDetail"></param>
        public void UpdateApplymentStateInfo(ApplymentFlowStatusEnum flowStatus, string applymentId, WxApplymentStateEnum wxApplymentState, string rejectReason, string auditDetail)
        {
            FlowStatus = flowStatus;
            ApplymentId = applymentId;
            ApplymentState = wxApplymentState;
            RejectReason = rejectReason;
            AuditDetail = auditDetail;
            UpdatedOnUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// 更新驳回原因详情
        /// </summary>
        /// <param name="rejectReason"></param>
        /// <param name="auditDetail"></param>
        public void UpdateDetailInfo(string rejectReason, string auditDetail)
        {
            RejectReason = rejectReason;
            AuditDetail = auditDetail;
            UpdatedOnUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// 更新审核时间
        /// </summary>
        /// <param name="dateTime"></param>
        public void UpdateVerifyTime(DateTime dateTime)
        {
            VerifyTime = dateTime;
            UpdatedOnUtc = DateTime.UtcNow;
        }
    }
}