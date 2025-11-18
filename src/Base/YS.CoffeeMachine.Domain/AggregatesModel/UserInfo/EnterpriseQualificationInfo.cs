using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 企业资质信息
    /// </summary>
    public class EnterpriseQualificationInfo : BaseEntity
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseId { get; private set; }

        /// <summary>
        /// 法人姓名
        /// </summary>
        public string LegalPersonName { get; private set; }

        /// <summary>
        /// 法人身份证号
        /// </summary>
        public string LegalPersonIdCardNumber { get; private set; }

        /// <summary>
        /// 证件正面照
        /// </summary>
        public string FrontImageUrl { get; private set; }

        /// <summary>
        /// 证件背面照
        /// </summary>
        public string BackImageUrl { get; private set; }

        #region 企业资质

        /// <summary>
        /// 客服邮箱
        /// </summary>
        public string CustomerServiceEmail { get; private set; }

        /// <summary>
        /// 门店详细地址
        /// </summary>
        public string StoreAddress { get; private set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public string BusinessLicenseUrl { get; private set; }

        /// <summary>
        /// 其他证件
        /// </summary>
        public string Othercertificate { get; private set; }

        /// <summary>
        /// 企业信息
        /// </summary>
        public EnterpriseInfo EnterpriseInfo { get; private set; }
        #endregion

        /// <summary>
        /// 企业资质信息构造函数
        /// </summary>
        protected EnterpriseQualificationInfo() { }

        /// <summary>
        /// 个人资质信息构造函数
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <param name="legalPersonName"></param>
        /// <param name="legalPersonIdCardNumber"></param>
        /// <param name="frontImageUrl"></param>
        /// <param name="backImageUrl"></param>
        public EnterpriseQualificationInfo(
            long enterpriseId,
            string legalPersonName,
            string legalPersonIdCardNumber,
            string frontImageUrl,
            string backImageUrl)
        {
            EnterpriseId = enterpriseId;
            LegalPersonName = legalPersonName;
            LegalPersonIdCardNumber = legalPersonIdCardNumber;
            FrontImageUrl = frontImageUrl;
            BackImageUrl = backImageUrl;
        }

        /// <summary>
        /// 更新企业资质信息
        /// </summary>
        /// <param name="legalPersonName"></param>
        /// <param name="legalPersonIdCardNumber"></param>
        /// <param name="frontImageUrl"></param>
        /// <param name="backImageUrl"></param>
        public void UpdateEnterpriseQualificationInfo(
            string legalPersonName,
            string legalPersonIdCardNumber,
            string frontImageUrl,
            string backImageUrl)
        {
            LegalPersonName = legalPersonName;
            LegalPersonIdCardNumber = legalPersonIdCardNumber;
            FrontImageUrl = frontImageUrl;
            BackImageUrl = backImageUrl;
        }

        /// <summary>
        /// 企业资质信息补充方法
        /// </summary>
        /// <param name="customerServiceEmail"></param>
        /// <param name="storeAddress"></param>
        /// <param name="businessLicenseUrl"></param>
        /// <param name="othercertificate"></param>
        public void CompleteEnterpriseQualification(
            string customerServiceEmail,
            string storeAddress,
            string businessLicenseUrl,
            string othercertificate)
        {
            CustomerServiceEmail = customerServiceEmail;
            StoreAddress = storeAddress;
            BusinessLicenseUrl = businessLicenseUrl;
            Othercertificate = othercertificate;
        }

        /// <summary>
        /// 清空企业资质信息
        /// </summary>
        public void ClearCompleteEnterpriseQualification()
        {
            CustomerServiceEmail = string.Empty;
            StoreAddress = string.Empty;
            BusinessLicenseUrl = string.Empty;
            Othercertificate = string.Empty;
        }
    }
}