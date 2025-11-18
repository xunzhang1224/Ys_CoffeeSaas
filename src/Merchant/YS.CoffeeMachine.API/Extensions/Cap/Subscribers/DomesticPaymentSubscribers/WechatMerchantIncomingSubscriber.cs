using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.Cabinet.Payment.WechatPay;
using YS.Cabinet.Payment.WechatPay.V3;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using static YS.Cabinet.Payment.WechatPay.V3.WxApplymentSmallMicroSubjectInfo.IdentityInfo;
using static YS.Cabinet.Payment.WechatPay.V3.WxApplymentSubjectInfo;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers.DomesticPaymentSubscribers
{
    /// <summary>
    /// 微信进件订阅
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_wechatMerchantService"></param>
    /// <param name="_logger"></param>
    /// <param name="_paymentPlatformUtil"></param>
    public class WechatMerchantIncomingSubscriber(CoffeeMachineDbContext context, IWechatMerchantService _wechatMerchantService, ILogger<WechatMerchantIncomingSubscriber> _logger, PaymentPlatformUtil _paymentPlatformUtil) : ICapSubscribe
    {
        /// <summary>
        /// 微信进件订阅执行方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.WechatMerchantIncomingSubmit)]
        public async Task Handle(long id)
        {
            if (id == 0)
                _logger.LogError("微信进件申请单参数异常：{0}", id);

            // 提交微信进件
            await SubmitWechatApplyment(id);
        }

        /// <summary>
        /// 提交微信进件
        /// </summary>
        /// <param name="applymentId"></param>
        /// <returns></returns>
        public async Task SubmitWechatApplyment(long applymentId)
        {
            // 获取进件信息
            var applyment = await context.M_PaymentWechatApplyments.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == applymentId);
            if (applyment == null)
                _logger.LogError("提交微信商户进件失败，未找到对应申请单");

            if (applyment == null)
            {
                _logger.LogError("提交微信商户进件申请失败，未找到对应的申请单。");
                return;
            }

            var paymentMethod = await context.M_PaymentMethod.IgnoreQueryFilters().FirstAsync(a => a.Id == applyment.PaymentOriginId);

            try
            {
                var client = _wechatMerchantService.BuildMerchant(paymentMethod.SystemPaymentServiceProviderId.ToString());

                #region (提交进件到微信)

                // 电商商户进件
                var wxEcommerceApplyment = await BuilderWxEcommerceApplymentRequestCreateRequest(applyment, client);
                var result = await client.EcommerceService.ToResponseAsync(a => a.CreateApplymentAsync(wxEcommerceApplyment));

                //WechatRestfulResponse<WxApplymentResponse> result = null;
                //if (applyment.OrganizationType == WxApplymentSubjectTypeEnum.SUBJECT_TYPE_MICRO)
                //{
                //    var merchantMicroApplyment = await BuilderWxMicroApplyment4CreateRequest(applyment, client);
                //    _logger.LogInformation("构建小微商户请求参数：" + JsonConvert.SerializeObject(merchantMicroApplyment));
                //    result = await client.Applyment4SubService.ToResponseAsync(a => a.CreateApplymentAsync(merchantMicroApplyment));
                //}
                //else
                //{
                //    var merchantApplyment = await BuilderWxApplyment4SubCreateRequest(applyment, client);
                //    _logger.LogInformation("构建特约商户请求参数：" + JsonConvert.SerializeObject(merchantApplyment));
                //    result = await client.Applyment4SubService.ToResponseAsync(a => a.CreateApplymentAsync(merchantApplyment));
                //}

                _logger.LogInformation($"微信商户进件结果：{JsonConvert.SerializeObject(result)}");

                if (result.Succeeded)
                {
                    applyment.UpdateApplymentStateInfo(ApplymentFlowStatusEnum.PlatformReview,
                        result.Data!.Applyment_Id.ToString(),
                        WxApplymentStateEnum.AUDITING,
                        string.Empty,
                        string.Empty);
                }
                else
                {
                    applyment.SetFlowStatus(ApplymentFlowStatusEnum.Failed);
                    applyment.UpdateDetailInfo((result.Data?.Message ?? result.Message) ?? string.Empty, (result.Data?.Message ?? result.Message) ?? string.Empty);
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "提交微信商户进件申请失败，applymentId: {applymentId}", applyment.Id);
                applyment.FlowStatus = ApplymentFlowStatusEnum.Failed;
                applyment.RejectReason = ex.Message ?? string.Empty;
                applyment.AuditDetail = ex.Message ?? string.Empty;
            }

            // 更新申请单状态到数据库
            applyment.UpdateVerifyTime(DateTime.UtcNow);

            context.M_PaymentWechatApplyments.Update(applyment);

            ////更新主申请单状态
            var paymentMethodInfo = await context.M_PaymentMethod.AsQueryable().IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == applyment.PaymentOriginId);
            if (paymentMethodInfo != null)
            {
                var status = _paymentPlatformUtil.ApplymentFlowStatusToOnboardingStatus(applyment.FlowStatus);
                paymentMethodInfo.UpdateApplymentStatus(applyment.SubMchId ?? string.Empty, status);
                context.M_PaymentMethod.Update(paymentMethodInfo);
            }

            await context.SaveChangesAsync();

            // 提交失败发送短信通知
            if (applyment.FlowStatus == ApplymentFlowStatusEnum.Failed)
            {
                if (paymentMethodInfo == null)
                {
                    _logger.LogError("微信商户进件申请失败，未找到对应的二级商户支付方式。");
                    return;
                }

                await _paymentPlatformUtil.ApplymentSendSmsNotice(ApplymentSmsNoticeTypeEnum.Failed, paymentMethodInfo.Phone, new
                {
                    regist_number = "J" + applyment.Id,
                    failure_reason = applyment.RejectReason
                });
            }
        }

        #region 构建电商商户请求参数

        /// <summary>
        /// 构建电商商户请求参数
        /// </summary>
        /// <param name="applyment"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task<WxEcommerceApplymentRequest> BuilderWxEcommerceApplymentRequestCreateRequest(M_PaymentWechatApplyments applyment, IWechatMerchantService client)
        {
            var idCardCopyMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.IdCardCopy);
            var idCardNationalMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.IdCardNational);
            var businessLetterCopyMadiaID = string.Empty;
            if (!string.IsNullOrEmpty(applyment.CertificateLetterCopy))
                businessLetterCopyMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.CertificateLetterCopy);
            var wxEcommerceApplyment = new WxEcommerceApplymentRequest
            {
                Out_Request_No = applyment.Id.ToString(),
                Organization_Type = GetWxApplymentOrganizationType(applyment.OrganizationType),
                Finance_Institution = false,
                Id_Holder_Type = WxApplymentContactTypeEnum.LEGAL,
                Id_Doc_Type = (WxEcommerceIdDocTypeEnum)applyment.IdDocType,
                Owner = true,
                Merchant_Shortname = applyment.MerchantShortName,
                Business_Addition_Desc = "该商户已持续从事电子商务经营活动满6个月，且期间经营收入累计超过20万元。"
            };

            #region 营业执照

            var indoorPicMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.IndoorPic);
            var storeEntrancePicMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.StoreEntrancePic);
            var businessLicenseCopyMadiaID = string.Empty;
            if (!string.IsNullOrEmpty(applyment.BusinessLicenseCopy))
                businessLicenseCopyMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.BusinessLicenseCopy);

            if (applyment.OrganizationType != WxApplymentSubjectTypeEnum.SUBJECT_TYPE_MICRO)
                wxEcommerceApplyment.Business_License_Info = new WxEcommerceBusinessLicenseInfo()
                {
                    Cert_Type = GetWxApplymentSubjectCertType(applyment.IdDocType),
                    Business_License_Copy = businessLicenseCopyMadiaID,
                    Business_License_Number = applyment.BusinessLicenseNumber,
                    Merchant_Name = applyment.MerchantName,
                    Legal_Person = string.IsNullOrEmpty(applyment.LegalPerson) ? applyment.IdCardName : applyment.LegalPerson,
                    Company_Address = applyment.LicenseAddress,
                    Business_Time = $"[\"{applyment.BusinessTimeBegin}\",\"{applyment.BusinessTimeEnd}\"]",
                };
            #endregion

            #region 金融机构许可信息（暂不填）
            #endregion

            #region 身份信息
            if (applyment.OrganizationType == WxApplymentSubjectTypeEnum.SUBJECT_TYPE_MICRO)
                wxEcommerceApplyment.Id_Card_Info = new WxEcommerceIdentityInfo()
                {
                    Id_Card_Copy = idCardCopyMadiaID,
                    Id_Card_National = idCardNationalMadiaID,
                    Id_Card_Name = client.RSAEncrypt(applyment.IdCardName),
                    Id_Card_Number = client.RSAEncrypt(applyment.IdCardNumber),
                    Id_Card_Address = client.RSAEncrypt(applyment.IdCardAddress),
                    Card_Period_Begin = applyment.IdCardValidTimeBegin,
                    Card_Period_End = applyment.IdCardValidTime
                };
            #endregion

            #region 经营者法人信息
            if (applyment.OrganizationType != WxApplymentSubjectTypeEnum.SUBJECT_TYPE_MICRO)
                wxEcommerceApplyment.Id_Doc_Info = new WxEcommerceOtherIdentityInfo()
                {
                    Id_Doc_Copy = idCardCopyMadiaID,
                    Id_Doc_Copy_Back = idCardNationalMadiaID,
                    Id_Doc_Name = client.RSAEncrypt(applyment.LegalPerson),
                    Id_Doc_Number = client.RSAEncrypt(applyment.IdCardNumber),
                    Id_Doc_Address = client.RSAEncrypt(applyment.IdCardAddress),
                    Doc_Period_Begin = applyment.IdCardValidTimeBegin,
                    Doc_Period_End = applyment.IdCardValidTime
                };
            #endregion

            #region 最终受益人信息列表
            wxEcommerceApplyment.Ubo_Info_List = new List<WxEcommerceUboInfo>()
            {
                new WxEcommerceUboInfo()
                {
                    Ubo_Id_Doc_Type = (WxEcommerceIdDocTypeEnum)applyment.IdDocType,
                    Ubo_Id_Doc_Copy = idCardCopyMadiaID,
                    Ubo_Id_Doc_Copy_Back = idCardNationalMadiaID,
                    Ubo_Id_Doc_Name = client.RSAEncrypt(applyment.LegalPerson),
                    Ubo_Id_Doc_Number = client.RSAEncrypt(applyment.IdCardNumber),
                    Ubo_Id_Doc_Address = client.RSAEncrypt(applyment.IdCardAddress),
                    Ubo_Period_Begin = applyment.IdCardValidTimeBegin,
                    Ubo_Period_End = applyment.IdCardValidTime
                }
            };
            #endregion

            #region 结算账户信息
            wxEcommerceApplyment.Account_Info = new WxEcommerceSettlementAccountInfo
            {
                Bank_Account_Type = WxApplymentSettlementAccountTypeEnum.BANK_ACCOUNT_TYPE_PERSONAL,
                Account_Name = client.RSAEncrypt(applyment.AccountName),
                Account_Bank = applyment.AccountBank,
                Bank_Address_Code = applyment.BankCityCode,
                Bank_Name = applyment.BankName,
                Bank_Branch_Id = applyment.BankBranchId,
                Account_Number = client.RSAEncrypt(applyment.AccountNumber),
            };
            #endregion

            #region 超级管理员
            wxEcommerceApplyment.Contact_Info = new WxEcommerceContactInfo
            {
                Contact_Type = WxApplymentContactTypeEnum.LEGAL,
                Contact_Name = client.RSAEncrypt(applyment.IdCardName),
                //Contact_Id_Doc_Type = (WxEcommerceIdDocTypeEnum)applyment.IdDocType,
                //Contact_Id_Card_Number = client.RSAEncrypt(applyment.IdCardNumber),
                //Contact_Id_Doc_Copy = idCardCopyMadiaID,
                //Contact_Id_Doc_Copy_Back = idCardNationalMadiaID,
                //Contact_Period_Begin = applyment.IdCardValidTimeBegin,
                //Contact_Period_End = applyment.IdCardValidTime,
                Mobile_Phone = client.RSAEncrypt(applyment.MobilePhone),
                //Business_Authorization_Letter = businessLetterCopyMadiaID
            };
            #endregion

            #region 店铺信息
            wxEcommerceApplyment.Sales_Scene_Info = new WxEcommerceSaleSceneInfo()
            {
                Store_Name = applyment.MerchantShortName,
                Store_Url = "http://coffmer.ourvend.com"
            };
            #endregion

            #region 结算规则
            wxEcommerceApplyment.Settlement_Info = new WxEcommerceSettlementInfo
            {
                Settlement_Id = applyment.OrganizationType == WxApplymentSubjectTypeEnum.SUBJECT_TYPE_MICRO ? 747 : applyment.OrganizationType == WxApplymentSubjectTypeEnum.SUBJECT_TYPE_INDIVIDUAL ? 802 : 800,
                Qualification_Type = applyment.OrganizationType == WxApplymentSubjectTypeEnum.SUBJECT_TYPE_MICRO ? null : "居民生活服务",
            };
            #endregion
            return wxEcommerceApplyment;
        }

        /// <summary>
        /// 根据主体类型，获取微信主体登记类型
        /// </summary>
        /// <param name="wxApplymentSubjectType"></param>
        /// <returns></returns>
        private WxApplymentOrganizationTypeEnum GetWxApplymentOrganizationType(WxApplymentSubjectTypeEnum wxApplymentSubjectType)
        {
            return wxApplymentSubjectType switch
            {
                //WxApplymentSubjectTypeEnum.SUBJECT_TYPE_MICRO => WxApplymentOrganizationTypeEnum.Person,
                WxApplymentSubjectTypeEnum.SUBJECT_TYPE_INDIVIDUAL => WxApplymentOrganizationTypeEnum.SUBJECT_TYPE_INDIVIDUAL,
                WxApplymentSubjectTypeEnum.SUBJECT_TYPE_ENTERPRISE => WxApplymentOrganizationTypeEnum.SUBJECT_TYPE_ENTERPRISE,
                WxApplymentSubjectTypeEnum.SUBJECT_TYPE_GOVERNMENT => WxApplymentOrganizationTypeEnum.SUBJECT_TYPE_GOVERNMENT,
                WxApplymentSubjectTypeEnum.SUBJECT_TYPE_INSTITUTIONS => WxApplymentOrganizationTypeEnum.SUBJECT_TYPE_INSTITUTIONS,
                WxApplymentSubjectTypeEnum.SUBJECT_TYPE_OTHERS => WxApplymentOrganizationTypeEnum.SUBJECT_TYPE_OTHERS,
                _ => WxApplymentOrganizationTypeEnum.Person
            };
        }

        /// <summary>
        /// 根据证件类型，获取主体登记证书类型
        /// </summary>
        private WxApplymentSubjectCertTypeEnum GetWxApplymentSubjectCertType(WxApplymentIdentityCertTypeEnum identityCertType)
        {
            return identityCertType switch
            {
                // 中国大陆居民-身份证 -> 统一社会信用代码证书
                WxApplymentIdentityCertTypeEnum.IDENTIFICATION_TYPE_IDCARD
                    => WxApplymentSubjectCertTypeEnum.CERTIFICATE_TYPE_2389,

                // 其他国家或地区居民-护照 -> 政府部门下发的其他有效证明文件
                WxApplymentIdentityCertTypeEnum.IDENTIFICATION_TYPE_OVERSEA_PASSPORT
                    => WxApplymentSubjectCertTypeEnum.CERTIFICATE_TYPE_2400,

                // 中国香港居民-来往内地通行证 -> 港澳台居民证件，也归到其他有效证明
                WxApplymentIdentityCertTypeEnum.IDENTIFICATION_TYPE_HONGKONG_PASSPORT
                    => WxApplymentSubjectCertTypeEnum.CERTIFICATE_TYPE_2400,

                // 中国澳门居民-来往内地通行证
                WxApplymentIdentityCertTypeEnum.IDENTIFICATION_TYPE_MACAO_PASSPORT
                    => WxApplymentSubjectCertTypeEnum.CERTIFICATE_TYPE_2400,

                // 中国台湾居民-来往大陆通行证
                WxApplymentIdentityCertTypeEnum.IDENTIFICATION_TYPE_TAIWAN_PASSPORT
                    => WxApplymentSubjectCertTypeEnum.CERTIFICATE_TYPE_2400,

                // 外国人居留证
                WxApplymentIdentityCertTypeEnum.IDENTIFICATION_TYPE_FOREIGN_RESIDENT
                    => WxApplymentSubjectCertTypeEnum.CERTIFICATE_TYPE_2400,

                // 港澳居民证
                WxApplymentIdentityCertTypeEnum.IDENTIFICATION_TYPE_HONGKONG_MACAO_RESIDENT
                    => WxApplymentSubjectCertTypeEnum.CERTIFICATE_TYPE_2400,

                // 台湾居民证
                WxApplymentIdentityCertTypeEnum.IDENTIFICATION_TYPE_TAIWAN_RESIDENT
                    => WxApplymentSubjectCertTypeEnum.CERTIFICATE_TYPE_2400,

                // 默认情况：其他有效证明
                _ => WxApplymentSubjectCertTypeEnum.CERTIFICATE_TYPE_2400
            };
        }

        #endregion

        #region 构建特约商户请求参数

        /// <summary>
        /// 构建微信小微商户进件请求参数
        /// </summary>
        /// <param name="applyment"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task<WxApplyment4SubSmallMicroCreateRequest> BuilderWxMicroApplyment4CreateRequest(M_PaymentWechatApplyments applyment, IWechatMerchantService client)
        {
            var merchantApplyment = new WxApplyment4SubSmallMicroCreateRequest
            {
                Business_Code = applyment.Id.ToString(),

                #region 超级管理员
                Contact_Info = new WxApplymentSmallMicroContactInfo
                {
                    Contact_Email = client.RSAEncrypt(applyment.Email),
                    Contact_Name = client.RSAEncrypt(applyment.IdCardName),
                    Mobile_Phone = client.RSAEncrypt(applyment.MobilePhone),
                    Contact_Id_Number = client.RSAEncrypt(applyment.IdCardNumber)
                },
                #endregion

                #region 结算规则
                Settlement_Info = new WxApplymentSmallMicroSettlementInfo
                {
                    Settlement_Id = WxApplymentSettlementEnum.SUBJECT_TYPE_MICRO,
                    Qualification_Type = WxApplymentQualificationTypeEnum.ResidentLive,
                },
                #endregion

                #region 经营资料
                Business_Info = new WxApplymentSmallMicroBusinessInfo
                {
                    Merchant_Shortname = applyment.MerchantShortName,
                    Service_Phone = applyment.ServicePhone,
                }
                #endregion
            };

            #region 主体信息
            var indoorPicMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.IndoorPic);
            var storeEntrancePicMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.StoreEntrancePic);

            var idCardCopyMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.IdCardCopy);
            var idCardNationalMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.IdCardNational);
            merchantApplyment.Subject_Info = new WxApplymentSmallMicroSubjectInfo
            {
                Subject_Type = applyment.OrganizationType,
                // 小微辅助证明材料
                Micro_Biz_Info = new WxApplymentSmallMicroSubjectInfo.MicroBizInfo
                {
                    Micro_Biz_Type = WxApplymentSaleScenesTypeEnum.MICRO_TYPE_STORE,
                    Micro_Store_Info = new WxApplymentSmallMicroSubjectInfo.MicroBizInfo.MicroStoreInfo
                    {
                        Micro_Name = applyment.MerchantName ?? applyment.MerchantShortName,
                        Micro_Address_Code = applyment.BizCityCode,
                        Micro_Address = applyment.BizStoreAddress,
                        Store_Entrance_Pic = storeEntrancePicMadiaID,
                        Micro_Indoor_Copy = indoorPicMadiaID,
                    }
                },
                //经营者/法人身份证件
                Identity_Info = new WxApplymentSmallMicroSubjectInfo.IdentityInfo
                {
                    Id_Doc_Type = applyment.IdDocType,
                    Id_Card_Info = new WxApplymentSmallMicroSubjectInfo.IdentityInfo.IdCardInfo
                    {
                        Id_Card_Copy = idCardCopyMadiaID,
                        Id_Card_National = idCardNationalMadiaID,
                        Id_Card_Name = client.RSAEncrypt(applyment.IdCardName),
                        Id_Card_Number = client.RSAEncrypt(applyment.IdCardNumber),
                        Card_Period_Begin = applyment.IdCardValidTimeBegin,
                        Card_Period_End = applyment.IdCardValidTime
                    }
                }
            };
            #endregion

            #region 结算银行账户
            merchantApplyment.Bank_Account_Info = new WxApplymentSmallMicroBankAccountInfo
            {
                Bank_Account_Type = WxApplymentSettlementAccountTypeEnum.BANK_ACCOUNT_TYPE_PERSONAL,
                Account_Name = client.RSAEncrypt(applyment.AccountName),
                Account_Bank = applyment.AccountBank,
                Bank_Address_Code = applyment.BankCityCode,
                Bank_Name = applyment.BankName,
                Bank_Branch_Id = applyment.BankBranchId,
                Account_Number = client.RSAEncrypt(applyment.AccountNumber),
            };
            #endregion

            #region 补充材料 -原来就注释的
            //merchantApplyment.Addition_Info = new AdditionInfo()
            //{
            //    Business_Addition_Msg = applymentInfo.ecommerce.Business_Addition_Desc,
            //    Business_Addition_Pics = JsonConvert.DeserializeObject<List<string>>(ecommerce.Business_Addition_Pics),
            //};
            #endregion

            return merchantApplyment;
        }

        /// <summary>
        /// 构建微信特约商户进件请求参数
        /// </summary>
        /// <param name="applyment"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task<WxApplyment4SubCreateRequest> BuilderWxApplyment4SubCreateRequest(M_PaymentWechatApplyments applyment, IWechatMerchantService client)
        {
            var indoorPicMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.IndoorPic);
            var storeEntrancePicMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.StoreEntrancePic);
            var merchantApplyment = new WxApplyment4SubCreateRequest();
            merchantApplyment.Business_Code = applyment.Id.ToString();

            #region 超级管理员

            merchantApplyment.Contact_Info = new WxApplymentContactInfo
            {
                //openid 跟 contact_id_number 二选一
                Contact_Type = WxApplymentContactTypeEnum.LEGAL,
                Contact_Name = client.RSAEncrypt(applyment.IdCardName),
                Contact_Email = client.RSAEncrypt(applyment.Email),
                Mobile_Phone = client.RSAEncrypt(applyment.MobilePhone),
                Contact_Id_Number = client.RSAEncrypt(applyment.IdCardNumber)
            };
            #endregion

            #region 主体信息
            var businessLicenseCopyMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.BusinessLicenseCopy);
            var businessLetterCopyMadiaID = string.Empty;
            if (!string.IsNullOrEmpty(applyment.CertificateLetterCopy))
                businessLetterCopyMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.CertificateLetterCopy);
            merchantApplyment.Subject_Info = new WxApplymentSubjectInfo
            {
                Subject_Type = applyment.OrganizationType,
                Certificate_Letter_Copy = businessLetterCopyMadiaID,
                // 营业执照
                Business_License_Info = new WxApplymentSubjectInfo.BusinessLicenseInfo
                {
                    License_Copy = businessLicenseCopyMadiaID,
                    License_Number = applyment.BusinessLicenseNumber,
                    Merchant_Name = applyment.MerchantName,
                    Legal_Person = string.IsNullOrEmpty(applyment.LegalPerson) ? applyment.IdCardName : applyment.LegalPerson,
                    License_Address = applyment.LicenseAddress
                },
                // 经营者/法人身份证件
                Identity_Info = new WxApplymentSubjectInfo.IdentityInfo
                {
                    Id_Doc_Type = applyment.IdDocType
                }
            };

            var idCardCopyMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.IdCardCopy);
            var idCardNationalMadiaID = await _paymentPlatformUtil.UploadImageToWechatMediaId(client, applyment.IdCardNational);
            if (applyment.IdDocType == WxApplymentIdentityCertTypeEnum.IDENTIFICATION_TYPE_IDCARD)
            {
                merchantApplyment.Subject_Info.Identity_Info.Id_Card_Info = new WxApplymentSubjectInfo.IdentityInfo.IdCardInfo
                {
                    Id_Card_Copy = idCardCopyMadiaID,
                    Id_Card_National = idCardNationalMadiaID,
                    Id_Card_Name = client.RSAEncrypt(applyment.LegalPerson),
                    Id_Card_Number = client.RSAEncrypt(applyment.IdCardNumber),
                    Id_Card_Address = applyment.OrganizationType == WxApplymentSubjectTypeEnum.SUBJECT_TYPE_ENTERPRISE ? client.RSAEncrypt(applyment.IdCardAddress) : "",
                    Card_Period_Begin = applyment.IdCardValidTimeBegin,
                    Card_Period_End = applyment.IdCardValidTime
                };
            }
            else
            {
                merchantApplyment.Subject_Info.Identity_Info.Id_Doc_Info = new WxApplymentSubjectInfo.IdentityInfo.IdDocInfo
                {
                    Id_Doc_Copy = idCardCopyMadiaID,
                    Id_Doc_Copy_Back = idCardNationalMadiaID,
                    Id_Doc_Name = client.RSAEncrypt(applyment.LegalPerson),
                    Id_Doc_Numbe = client.RSAEncrypt(applyment.IdCardNumber),
                    Id_Doc_Address = applyment.OrganizationType == WxApplymentSubjectTypeEnum.SUBJECT_TYPE_ENTERPRISE ? client.RSAEncrypt(applyment.IdCardAddress) : "",
                    Doc_Period_Begin = applyment.IdCardValidTimeBegin,
                    Doc_Period_End = applyment.IdCardValidTime
                };
            }
            if (applyment.OrganizationType == WxApplymentSubjectTypeEnum.SUBJECT_TYPE_ENTERPRISE)
            {
                merchantApplyment.Subject_Info.Identity_Info.Owner = true;
                merchantApplyment.Subject_Info.Ubo_Info_List = new List<WxApplymentSubjectInfo.UboInfo>();
            }
            #endregion

            #region 经营资料
            merchantApplyment.Business_Info = new WxApplymentBusinessInfo
            {
                Merchant_Shortname = applyment.MerchantShortName,
                Service_Phone = applyment.ServicePhone,
                Sales_Info = new WxApplymentBusinessInfo.SalesInfo
                {
                    Sales_Scenes_Type = new List<string>() { WxApplymentSaleScenesTypeEnum.SALES_SCENES_STORE.ToString() },
                    Biz_Store_Info = new WxApplymentBusinessInfo.SalesInfo.BizStoreInfo
                    {
                        Biz_Store_Name = applyment.MerchantShortName,
                        Biz_Address_Code = applyment.BizCityCode,
                        Biz_Store_Address = applyment.BizStoreAddress,
                        Indoor_Pic = new List<string>() { indoorPicMadiaID },
                        Store_Entrance_Pic = new List<string>() { storeEntrancePicMadiaID }
                    }
                }
            };
            #endregion

            #region 结算规则
            merchantApplyment.Settlement_Info = new WxApplymentSettlementInfo
            {
                Settlement_Id = applyment.OrganizationType == WxApplymentSubjectTypeEnum.SUBJECT_TYPE_INDIVIDUAL ?
                     WxApplymentSettlementEnum.SUBJECT_TYPE_INDIVIDUAL
                     : WxApplymentSettlementEnum.SUBJECT_TYPE_ENTERPRISE,
                Qualification_Type = WxApplymentQualificationTypeEnum.ResidentLive,
            };
            #endregion

            #region 结算银行账户
            merchantApplyment.Bank_Account_Info = new WxApplymentBankAccountInfo
            {
                Bank_Account_Type = WxApplymentSettlementAccountTypeEnum.BANK_ACCOUNT_TYPE_CORPORATE,
                Account_Name = client.RSAEncrypt(applyment.AccountName),
                Account_Bank = applyment.AccountBank,
                Bank_Address_Code = applyment.BankCityCode,
                Bank_Branch_Id = applyment.BankBranchId,
                Bank_Name = applyment.BankName,
                Account_Number = client.RSAEncrypt(applyment.AccountNumber),
            };
            #endregion

            return merchantApplyment;
        }
        #endregion
    }
}