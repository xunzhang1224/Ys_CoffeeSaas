using Aop.Api.Domain;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.Cabinet.Payment.Alipay;
using YS.CoffeeMachine.API.Extensions.Cap.Subscribers.DomesticPaymentSubscribers;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayApplymentDtos;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribe
{
    /// <summary>
    /// 支付宝进件订阅
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_alipayService"></param>
    /// <param name="_logger"></param>
    /// <param name="_paymentPlatformUtil"></param>
    public class AlipayMerchantIncomingSubscriber(CoffeeMachineDbContext context, IAlipayService _alipayService, ILogger<WechatMerchantIncomingSubscriber> _logger, PaymentPlatformUtil _paymentPlatformUtil) : ICapSubscribe
    {
        /// <summary>
        /// 支付宝进件订阅执行方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.AlipayMerchantIncomingSubmit)]
        public async Task Handle(long id)
        {
            if (id == 0)
                _logger.LogError("支付宝进件申请单参数异常：{0}", id);

            var applyment = await context.M_PaymentAlipayApplyments.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == id);
            if (applyment == null)
                return;

            var mPaymentMethod = await context.M_PaymentMethod.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == applyment.PaymentOriginId && w.EnterpriseinfoId == applyment.EnterpriseinfoId);
            if (mPaymentMethod == null)
                return;

            // 提交微信进件
            await SubmitAlipayApplyment(mPaymentMethod.SystemPaymentServiceProviderId, applyment);
        }

        /// <summary>
        /// 提交支付宝进件
        /// </summary>
        /// <param name="serviceProviderId"></param>
        /// <param name="applyment"></param>
        /// <returns></returns>
        public async Task SubmitAlipayApplyment(long serviceProviderId, M_PaymentAlipayApplyments applyment)
        {
            try
            {
                try
                {
                    var alipayService = _alipayService.BuildMerchant(serviceProviderId.ToString());
                    var certInfo = GetCertByMerchantType(((int)applyment.MerchantType).ToString(), applyment.LegalCertNo, applyment.LegalCertName,
                        applyment.LegalCertFrontImage, applyment.LegalCertBackImage, applyment.UnifiedSocialCreditCode,
                        applyment.BusinessLicenseImage, applyment.BusinessLicenseName);

                    #region (构建请求request)

                    //并行上传照片url到支付宝
                    var uploadTasks = new[]
                                    {
                                        UploadImageToAlipay(alipayService, applyment.LegalCertFrontImage),
                                        UploadImageToAlipay(alipayService, certInfo.CertImage),
                                        UploadImageToAlipay(alipayService, certInfo.CertImageBack),
                                        UploadImageToAlipay(alipayService, applyment.LegalCertBackImage),
                                        UploadImageToAlipay(alipayService, applyment.InDoorImages),
                                        UploadImageToAlipay(alipayService, applyment.OutDoorImages),
                                        UploadImageToAlipay(alipayService, applyment.LicenseAuthLetterImage)
                                    };

                    var results = await Task.WhenAll(uploadTasks);

                    var legalCertFrontImage = results[0];
                    var certImage = results[1];
                    var certImageBack = results[2];
                    var legalCertBackImage = results[3];
                    var inDoorImages = results[4];
                    var outDoorImages = results[5];
                    var licenseAuthLetterImage = results[6];

                    _logger.LogInformation($"上传图片到支付宝结果：{applyment.Id} " +
                        $"legalCertFrontImage: {legalCertFrontImage}, certImage: {certImage}, " +
                        $"certImageBack: {certImageBack}, legalCertBackImage: {legalCertBackImage}, " +
                        $"inDoorImages: {inDoorImages}, outDoorImages: {outDoorImages}, licenseAuthLetterImage: {licenseAuthLetterImage}");

                    // var addressInfo = applyments.BusinessAddress.Adapt<ApplymentRequest.Zftaddressinfo>();
                    ApplymentRequest.Zftaddressinfo? addressInfo = null;
                    if (!string.IsNullOrWhiteSpace(applyment.BusinessAddress))
                    {
                        var businessAddress = JsonConvert.DeserializeObject<BusinessAddressInfoDto>(applyment.BusinessAddress);
                        if (businessAddress != null)
                        {
                            addressInfo = new ApplymentRequest.Zftaddressinfo
                            {
                                Address = businessAddress.Address,
                                City_code = businessAddress.City_code,
                                District_code = businessAddress.District_code,
                                Latitude = businessAddress.Latitude,
                                Longitude = businessAddress.Longitude,
                                Poiid = businessAddress.Poiid,
                                Province_code = businessAddress.Province_code,
                                Type = businessAddress.Type
                            };
                        }
                    }
                    List<ApplymentRequest.Zftcontactinfos>? concatInfos = null;
                    if (applyment.ContactInfos != null)
                    {
                        var contactinfos = JsonConvert.DeserializeObject<ContactinfoDto>(applyment.ContactInfos);
                        if (contactinfos != null)
                        {
                            concatInfos = new List<ApplymentRequest.Zftcontactinfos>
                    {
                        new ApplymentRequest.Zftcontactinfos
                        {
                            Email = contactinfos.Email,
                            Id_card_no = contactinfos.Id_card_no,
                            Mobile = contactinfos.Mobile,
                            Name = contactinfos.Name,
                            Phone = contactinfos.Phone,
                            Tag = contactinfos.Tag,
                            Type = contactinfos.Type,
                        }
                    };
                        }
                    }

                    var model = new ApplymentRequest()
                    {
                        External_id = applyment.Id.ToString(),
                        Name = applyment.MerchantName,
                        Alias_name = applyment.MerchantShortName,
                        Merchant_type = "0" + ((int)applyment.MerchantType).ToString(),
                        Mcc = applyment.Mcc,
                        Cert_no = certInfo.CertNo,
                        Cert_name = certInfo.CertName,
                        Cert_type = certInfo.CertType,
                        Cert_image = certImage,
                        Cert_image_back = certImageBack,
                        Legal_name = applyment.LegalCertName,
                        Legal_cert_no = applyment.LegalCertNo,
                        Legal_cert_front_image = legalCertFrontImage,
                        Legal_cert_back_image = legalCertBackImage,
                        Business_address = addressInfo,
                        Service_phone = applyment.ServicePhone,
                        Contact_infos = concatInfos,
                        In_door_images = inDoorImages == null ? null : [inDoorImages],
                        Out_door_images = outDoorImages == null ? null : [outDoorImages],
                        License_auth_letter_image = licenseAuthLetterImage,
                        Sites = JsonConvert.DeserializeObject<List<SiteInfo>>(applyment.Sites ?? "[]"),
                        Service = JsonConvert.DeserializeObject<List<string>>(applyment.Service ?? "[]"),
                        Sign_time_with_isv = applyment.SignTimeWithIsv,
                        Alipay_logon_id = applyment.AlipayLogonId,
                        Binding_alipay_logon_id = applyment.AlipayLogonId,
                        Legal_cert_type = applyment.LegalCertType.ToString(),
                        Default_settle_rule = new DefaultSettleRule() { DefaultSettleTarget = applyment.AlipayLogonId, DefaultSettleType = "alipayAccount" }
                    };

                    #endregion

                    _logger.LogInformation($"二级商户进件请求参数：{applyment.Id}" + JsonConvert.SerializeObject(model));

                    var result = await alipayService.ApplymentService.MerchantApplyment(model);

                    _logger.LogInformation($"二级商户进件结果：{applyment.Id}" + JsonConvert.SerializeObject(result));

                    if (result.Success)
                    {
                        applyment.OrderId = result.Data?.OrderId;
                        //applyments.Status = "031";
                        applyment.RejectReason = string.Empty;
                        applyment.FlowStatus = ApplymentFlowStatusEnum.PlatformReview;
                    }
                    else
                    {
                        applyment.FlowStatus = ApplymentFlowStatusEnum.Failed;
                        applyment.RejectReason = result.Data == null ? result.Msg : result.Data.SubMsg;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"提交支付宝商户进件申请失败，applymentId: {applyment.Id}");
                    applyment.FlowStatus = ApplymentFlowStatusEnum.Failed;
                    applyment.RejectReason = ex.Message ?? string.Empty;
                }

                applyment.VerifyUserId = 0;
                applyment.VerifyTime = DateTime.UtcNow;

                // 更新明细表信息
                context.M_PaymentAlipayApplyments.Update(applyment);
                await context.SaveChangesAsync();

                //更新主表信息
                await _paymentPlatformUtil.UpdatePaymentMothodResult(applyment.PaymentOriginId, applyment.Smid == null ? string.Empty : applyment.Smid, applyment.FlowStatus);

                //提交失败发送短信通知
                if (applyment.FlowStatus == ApplymentFlowStatusEnum.Failed)
                {
                    var phone = await context.M_PaymentMethod
                        .IgnoreQueryFilters()
                        .Where(a => a.Id == applyment.PaymentOriginId)
                        .Select(a => a.Phone).FirstOrDefaultAsync();

                    if (phone != null && phone.Count() > 0)
                        await _paymentPlatformUtil.ApplymentSendSmsNotice(ApplymentSmsNoticeTypeEnum.Failed, phone, new
                        {
                            regist_number = "J" + applyment.Id,
                            failure_reason = applyment.RejectReason
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"提交支付宝商户进件申请失败，applymentId: {applyment.Id}");
            }
        }

        /// <summary>
        /// 上传文件到支付宝并返回ImageId
        /// </summary>
        /// <param name="alipayService">支付宝服务需要先初始化</param>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        private async Task<string?> UploadImageToAlipay(IAlipayService alipayService, string? fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return null;

            // 不是http地址，直接返回原值
            if (!fileUrl.Trim().StartsWith("http"))
                return fileUrl;

            try
            {
                var file = await _paymentPlatformUtil.DownloadImageToByte(fileUrl);
                var response = await alipayService.ImageUploadService.
                    ImageUpload(new UploadRequest()
                    {
                        FileName = fileUrl,
                        File = file
                    });

                if (response.Success)
                {
                    return response.Data?.ImageId;
                }
            }
            catch (Exception)
            {
                return fileUrl;
            }
            return null;
        }

        /// <summary>
        /// 根据商户类型获取证件类型及证件编号,不同的商户类型需要提供不同的证件类型和证件编号
        /// </summary>
        /// <param name="merchantType">商户类型</param>
        /// <param name="idCardNumber">证件号</param>
        /// <param name="idCardName">证件号名称</param>
        /// <param name="IdCardCopy">证件正面图片</param>
        /// <param name="IdCardNational">证件背面</param>
        /// <param name="unifiedSocialCreditCode">统一信用代码</param>
        /// <param name="businessLicenseImage">营业执照图片</param>
        /// <param name="businessLicenseName">营业执照名称</param>
        /// <returns></returns>
        /// <remarks>
        /// <para>【描述】商户类型：</para>
        /// <para>01：企业；cert_type填写201（营业执照）；cert_no填写营业执照号</para>
        /// <para>02：事业单位；cert_type填写218（事业单位法人证书）；cert_no填写事业单位法人证书编号</para>
        /// <para>03：民办非企业组织；cert_type填写204（民办非企业登记证书）；cert_no填写民办非企业登记证书编号</para>
        /// <para>04：社会团体；cert_type填写206（社会团体法人登记证书）；cert_no填写社会团体法人登记证书编号</para>
        /// <para>05：党政及国家机关；cert_type填写219（党政机关批准设立文件/行政执法主体资格证）；cert_no填写党政机关批准设立文件/行政执法主体资格证编号</para>
        /// <para>06：个人商户；cert_type填写100（个人身份证）；cert_no填写个人身份证号码</para>
        /// <para>07：个体工商户；cert_type填写201（营业执照）；cert_no填写营业执照号</para>
        /// <para>【枚举值】企业: 01，事业单位: 02，民办非企业组织: 03，社会团体: 04，党政及国家机关: 05，个人商户: 06，个体工商户: 07</para>
        /// <para>【示例值】01</para>
        /// </remarks>
        private static CertTypeDto GetCertByMerchantType(string merchantType, string idCardNumber, string idCardName, string IdCardCopy,
            string IdCardNational, string? unifiedSocialCreditCode, string? businessLicenseImage, string? businessLicenseName)
        {
            // 企业、事业单位、民办非企业组织、社会团体、党政及国家机关、个体工商户
            if (new[] { "1", "2", "3", "4", "5", "7" }.Contains(merchantType))
            {
                var certType = merchantType switch
                {
                    "1" => "201",
                    "2" => "218",
                    "3" => "204",
                    "4" => "206",
                    "5" => "219",
                    "7" => "201",
                    _ => ""
                };
                return new CertTypeDto
                {
                    CertType = certType,
                    CertNo = unifiedSocialCreditCode ?? "",
                    CertImage = businessLicenseImage ?? "",
                    CertName = businessLicenseName ?? "",
                    CertImageBack = "",
                    DomesticMerchantType = DomesticMerchantTypeEnum.Enterprise
                };
            }
            // 个人商户
            if (merchantType == "6")
            {
                return new CertTypeDto
                {
                    CertType = "100",
                    CertNo = idCardNumber,
                    CertImage = IdCardCopy,
                    CertImageBack = IdCardNational,
                    CertName = idCardName,
                    DomesticMerchantType = DomesticMerchantTypeEnum.Individual
                };
            }
            // 默认返回空对象
            return new CertTypeDto();
        }
    }
}