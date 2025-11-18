using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 企业资质信息传输对象
    /// </summary>
    public class EnterpriseQualificationInfoDto
    {
        /// <summary>
        /// 企业资质类型
        /// </summary>
        public EnterpriseOrganizationTypeEnum? organizationType { get; set; } = null;

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
        /// 地区（字典）
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 国家名称
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 语言（字典）
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 币种（字典）
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 时区
        /// </summary>
        public string TimeZone { get; set; }

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
}