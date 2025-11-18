using Newtonsoft.Json;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayApplymentDtos;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos
{
    /// <summary>
    /// 支付宝支付进件数据传输对象
    /// </summary>
    public class M_PaymentAlipayApplymentsDto
    {
        /// <summary>
        ///  商户支付方式表的OriginId(Me_PaymentMethod表的OriginId)
        /// </summary>
        public long PaymentOriginId { get; set; }

        /// <summary>
        /// 申请单ID
        /// </summary>
        public string? OrderId { get; set; }

        /// <summary>
        /// 商家类型：01：企业；02：事业单位；03：民办非企业组织；04：社会团体；05：党政及国家机关；06：个人商户；07：个体工商户
        /// </summary>
        public MerchantTypeEnum? MerchantType { get; set; } = null!;

        /// <summary>
        /// 商户类别码mcc
        /// </summary>
        /// <remarks>参见附件描述中的“类目code” https://gw.alipayobjects.com/os/basement_prod/82cb70f7-abbd-417a-91ba-73c1849f07ea.xlsx 如果要求资质一栏不为空，表明是特殊行业，会有人工审核。注：文档更新可能有滞后性，以实际为准</remarks>
        public string Mcc { get; set; } = null!;

        /// <summary>
        /// 证件类型
        /// </summary>
        /// <remarks>
        /// <para>类型枚举值：</para>
        /// <list type="bullet">
        /// <item>大陆身份证: 100</item>
        /// <item>港澳居民往来内地通行证: 105</item>
        /// <item>台湾同胞往来大陆通行证: 106</item>
        /// <item>外国人居留证: 108</item>
        /// <item>警官证: 109</item>
        /// </list>
        /// <para>示例值：100</para>
        /// <para>字段长度：16</para>
        /// </remarks>
        public LegalCertTypeEnum? LegalCertType { get; set; } = null!;

        /// <summary>
        /// 证件正面照片URL
        /// </summary>
        public string LegalCertFrontImage { get; set; } = null!;

        /// <summary>
        /// 证件反面照片URL
        /// </summary>
        public string LegalCertBackImage { get; set; } = null!;

        /// <summary>
        /// 证件姓名
        /// </summary>
        public string LegalCertName { get; set; } = null!;

        /// <summary>
        /// 联系人手机号
        /// </summary>
        public string Phone { get; set; } = null!;

        /// <summary>
        /// 证件号码
        /// </summary>
        public string LegalCertNo { get; set; } = null!;

        /// <summary>
        /// 证件地址
        /// </summary>
        public string LegalCertAddress { get; set; } = null!;

        /// <summary>
        /// 证件有效期开始时间
        /// </summary>
        public string? LegalCertValidTimeBegin { get; set; } = null!;

        /// <summary>
        /// 证件有效期结束时间(长期直接文本长期)
        /// </summary>
        public string LegalCertValidTimeEnd { get; set; } = null!;

        /// <summary>
        /// 商户别名
        /// </summary>
        public string MerchantShortName { get; set; } = null!;

        /// <summary>
        /// 经营地址 Json字符串
        /// </summary>
        /// <remarks>
        /// <para>【描述】经营地址。</para>
        /// <para>当使用当面付服务时，本字段要求必填。</para>
        /// <para>地址对象中省、省份编码（province_code）、市（city_code）、区（district_code）、地址（address）必填，其余选填。</para>
        /// </remarks>
        //public BusinessAddressInfoDto BusinessAddress { get; set; } = null!;

        /// <summary>
        /// 客服电话
        /// </summary>
        public string ServicePhone { get; set; } = null!;

        /// <summary>
        /// 经营详细地址
        /// </summary>
        public string BusinessAddressDetail { get; set; } = null!;

        /// <summary>
        /// 内景照Url（门内照片）
        /// </summary>
        public string InDoorImages { get; set; } = null!;

        /// <summary>
        /// 门头照Url
        /// </summary>
        public string OutDoorImages { get; set; } = null!;

        /// <summary>
        /// 营业执照照片URL
        /// </summary>
        public string? BusinessLicenseImage { get; set; } = null!;

        /// <summary>
        /// 营业期限有效开始时间
        /// </summary>
        public string? BusinessTimeBegin { get; set; } = null;

        /// <summary>
        /// 营业期限有效结束时间
        /// </summary>
        public string? BusinessTimeEnd { get; set; } = null;

        /// <summary>
        /// 结算支付宝账号
        /// </summary>
        public string AlipayLogonId { get; set; } = null!;

        /// <summary>
        /// 统一信用代码(营业执照号)
        /// </summary>
        public string? UnifiedSocialCreditCode { get; set; }

        /// <summary>
        /// 营业执照名称
        /// </summary>
        public string? BusinessLicenseName { get; set; } = null!;

        /// <summary>
        /// 营业执照法人姓名
        /// </summary>
        public string? BusinessLicenseLegalName { get; set; }

        /// <summary>
        /// 营业执照注册地址
        /// </summary>
        public string? BusinessLicenseAddress { get; set; }

        /// <summary>
        /// 商户联系人信息Json字符串（支付宝字段：contact_infos，可选，ContactInfo）
        /// </summary>
        public string? ContactInfos { get; set; }

        /// <summary>
        /// 结算银行卡信息（支付宝字段：biz_cards，特殊可选， settleCardInfo[]）
        /// </summary>
        public string? BizCards { get; set; }

        // /// <summary>
        // /// 商户行业资质Json字符串（支付宝字段：qualifications，可选，IndustryQualificationInfo[]）
        // /// </summary>
        // /// <remarks>
        // /// <para>【描述】商户行业资质，当商户是特殊行业时必填。</para>
        // /// <para>每项行业资质信息中，industry_qualification_type 和 industry_qualification_image 均必填。</para>
        // /// </remarks>
        // [SugarColumn(ColumnName = "Qualifications", Length = 500)]
        // public string? Qualifications { get;  set; }

        /// <summary>
        /// 授权函
        /// </summary>
        public string? LicenseAuthLetterImage { get; set; }

        ///// <summary>
        ///// 商户使用服务Json字符串
        ///// </summary>
        //public List<string> Service { get; set; } = null!;

        /// <summary>
        /// 二级商户与服务商的签约时间
        /// </summary>
        /// <remarks>
        /// <para>【描述】二级商户与服务商的签约时间。</para>
        /// <para>示例值：2015-04-15</para>
        /// <para>字段长度：20</para>
        /// </remarks>
        public string? SignTimeWithIsv { get; set; }

        /// <summary>
        /// 商户站点信息Json字符串（支付宝字段：sites，可选，SiteInfo[]）
        /// </summary>
        public string? Sites { get; set; }

        /// <summary>
        /// 开票资料信息Json字符串
        /// </summary>
        public string? InvoiceInfo { get; set; }

        /// <summary>
        /// 签约支付宝账户
        /// </summary>
        public string BindingAlipayLogonId { get; set; } = null!;

        /// <summary>
        /// 入驻时间
        /// </summary>
        public DateTime? Addtime { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; } = null!;

        /// <summary>
        /// 申请单创建时间（支付宝字段：apply_time，必选，date(20)）
        /// </summary>
        /// <remarks>
        /// <para>【描述】申请单创建时间。</para>
        /// <para>示例值：2017-11-11 12:00:00</para>
        /// <para>通过ant.merchant.expand.order.query(商户申请单查询)接口获得</para>
        /// </remarks>
        public DateTime? ApplyTime { get; set; }

        /// <summary>
        /// 拓展信息（支付宝字段：ext_info，必选，string(2048)）
        /// </summary>
        /// <remarks>
        /// <para>【描述】返回申请单相关参数。当前返回内容有 cardAliasNo、smid。</para>
        /// <para>示例值：{"cardAliasNo":"ab7c65ab96","smid":"20881234567890"}</para>
        /// <para>字段长度：2048</para>
        /// <para>通过ant.merchant.expand.order.query(商户申请单查询)接口获得</para>
        /// </remarks>
        public string? ExtInfo { get; set; }

        /// <summary>
        /// 二级商户id
        /// </summary>
        public string? Smid { get; set; }

        /// <summary>
        /// 卡编号,从ExtInfo中获取
        /// </summary>
        /// <remarks>在发起结算时可以作为结算账号</remarks>
        public string? CardAliasNo { get; set; }

        /// <summary>
        /// 审核备注信息
        /// </summary>
        public string? Memo { get; set; }

        /// <summary>
        /// 进件流程状态
        /// </summary>
        public ApplymentFlowStatusEnum FlowStatus { get; set; }

        /// <summary>
        /// 驳回原因
        /// </summary>
        public string? RejectReason { get; set; }

        /// <summary>
        /// 授权token
        /// </summary>
        /// <remarks>
        /// <para>文档：https://opendocs.alipay.com/isv/10467/xldcyq?pathHash=abce531a</para>
        /// </remarks>
        public string? AppAuthToken { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public long? VerifyUserId { get; set; }

        /// <summary>
        /// 审核时间（成功失败都会记录）
        /// </summary>
        public DateTime? VerifyTime { get; set; }

        /// <summary>
        /// 手工进件标识(ArtificialApplymentEnum枚举)
        /// </summary>
        public ArtificialApplymentEnum? IsArtificialApplyment { get; set; } = ArtificialApplymentEnum.No;
    }

    /// <summary>
    /// 支付宝进件数据输出对象
    /// </summary>
    public class M_PaymentAlipayApplymentsOutput : M_PaymentAlipayApplymentsDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 经营地址 Json字符串
        /// </summary>
        /// <remarks>
        /// <para>【描述】经营地址。</para>
        /// <para>当使用当面付服务时，本字段要求必填。</para>
        /// <para>地址对象中省、省份编码（province_code）、市（city_code）、区（district_code）、地址（address）必填，其余选填。</para>
        /// </remarks>
        public string BusinessAddress { get; set; } = null!;

        /// <summary>
        /// 经营地址 Json字符串
        /// </summary>
        /// <remarks>
        /// <para>【描述】经营地址。</para>
        /// <para>当使用当面付服务时，本字段要求必填。</para>
        /// <para>地址对象中省、省份编码（province_code）、市（city_code）、区（district_code）、地址（address）必填，其余选填。</para>
        /// </remarks>
        public BusinessAddressInfoDto BusinessAddressDto
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(BusinessAddress))
                    return JsonConvert.DeserializeObject<BusinessAddressInfoDto>(BusinessAddress) ?? new BusinessAddressInfoDto();
                return new BusinessAddressInfoDto();
            }
        }

        /// <summary>
        /// 商户使用服务Json字符串
        /// </summary>
        public List<string> ServiceDto
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Service))
                    return JsonConvert.DeserializeObject<List<string>>(Service) ?? new List<string>();
                return new List<string>();
            }
        }

        /// <summary>
        /// 商户使用服务Json字符串
        /// </summary>
        public string Service { get; set; } = null!;
    }

    /// <summary>
    /// 支付宝进件数据输入对象
    /// </summary>
    public class M_PaymentAlipayApplymentsInput : M_PaymentAlipayApplymentsDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 经营地址 Json字符串
        /// </summary>
        /// <remarks>
        /// <para>【描述】经营地址。</para>
        /// <para>当使用当面付服务时，本字段要求必填。</para>
        /// <para>地址对象中省、省份编码（province_code）、市（city_code）、区（district_code）、地址（address）必填，其余选填。</para>
        /// </remarks>
        public BusinessAddressInfoDto BusinessAddress { get; set; } = null!;

        /// <summary>
        /// 商户使用服务Json字符串
        /// </summary>
        public List<string>? Service { get; set; } = new List<string>() { "jsapi支付" };

        //#region 主体信息

        ///// <summary>
        ///// 证件姓名
        ///// </summary>
        //public string IdCardName { get; set; } = null!;

        ///// <summary>
        ///// 证件号码
        ///// </summary>
        //public string IdCardNumber { get; set; } = null!;

        ///// <summary>
        ///// 证件地址
        ///// </summary>
        //public string IdCardAddress { get; set; } = null!;

        ///// <summary>
        ///// 证件有效开始时间
        ///// </summary>
        //public string IdCardValidTimeBegin { get; set; } = null!;

        ///// <summary>
        ///// 证件有效结束时间,(长期直接文本长期)
        ///// </summary>
        //public string IdCardValidTime { get; set; } = null!;

        ///// <summary>
        ///// 证件正面照片
        ///// </summary>
        //public string IdCardCopy { get; set; } = null!;

        ///// <summary>
        ///// 证件反面照片
        ///// </summary>
        //public string IdCardNational { get; set; } = null!;

        //#endregion
    }

    /// <summary>
    /// 商户证件类型数据传输对象
    /// </summary>
    public class CertTypeDto
    {
        /// <summary>
        /// 商户证件类型
        /// </summary>
        public string CertType { get; set; } = null!;

        /// <summary>
        /// 商户证件图片url
        /// </summary>
        public string CertImage { get; set; } = null!;

        /// <summary>
        /// 商户证件背面图片url
        /// </summary>
        public string CertImageBack { get; set; } = null!; // 证件背面图片url

        /// <summary>
        /// 商户证件号码
        /// </summary>
        public string CertNo { get; set; } = null!; // 证件号码

        /// <summary>
        /// 商户证件名称
        /// </summary>
        public string CertName { get; set; } = null!; // 证件名称

        /// <summary>
        /// 内部商户类型
        /// </summary>
        public DomesticMerchantTypeEnum DomesticMerchantType { get; set; }
    }
}
