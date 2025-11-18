using System.ComponentModel.DataAnnotations;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 用户注册信息传输对象
    /// </summary>
    public class RegisterUserInfoInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 邮箱验证码
        /// </summary>
        public string? EmailVCode { get; set; } = null;

        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string? SmsVCode { get; set; } = null;
    }

    /// <summary>
    /// 企业资质信息传输对象
    /// </summary>
    public class EnterpriseQualificationInfoDto
    {
        /// <summary>
        /// 企业资质类型
        /// </summary>
        public EnterpriseOrganizationTypeEnum organizationType { get; set; }

        /// <summary>
        /// 国家名
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 法人姓名
        /// </summary>
        public string LegalPersonName { get; set; }

        /// <summary>
        /// 法人身份证号
        /// </summary>
        public string LegalPersonIdCardNumber { get; set; }

        /// <summary>
        /// 国家/币种关联表Id
        /// </summary>
        public long? AreaRelationId { get; set; } = null;

        /// <summary>
        /// 证件正面照
        /// </summary>
        public string FrontImageUrl { get; set; }

        /// <summary>
        /// 证件背面照
        /// </summary>
        public string BackImageUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #region 企业资质

        /// <summary>
        /// 客服邮箱
        /// </summary>
        public string CustomerServiceEmail { get; set; }

        /// <summary>
        /// 门店详细地址
        /// </summary>
        public string StoreAddress { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public string BusinessLicenseUrl { get; set; }

        /// <summary>
        /// 其他证件
        /// </summary>
        public string Othercertificate { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        #endregion
    }

    /// <summary>
    /// 企业资质信息输出对象
    /// </summary>
    public class EnterpriseQualificationInfoOutput : EnterpriseQualificationInfoDto
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long? EnterpriseId { get; set; } = null;

        /// <summary>
        /// 注册进度
        /// </summary>
        public RegistrationProgress? RegistrationProgress { get; set; } = null;

    }
}