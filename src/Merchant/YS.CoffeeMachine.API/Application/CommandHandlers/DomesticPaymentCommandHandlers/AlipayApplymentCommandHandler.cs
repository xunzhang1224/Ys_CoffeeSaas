using Aop.Api.Domain;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DomesticPaymentCommandHandlers
{
    /// <summary>
    /// 支付宝商户进件
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_cap"></param>
    /// <param name="_user"></param>
    public class AlipayApplymentCommandHandler(CoffeeMachineDbContext context, IMapper mapper, IPublishService _cap, UserHttpContext _user) : ICommandHandler<AlipayApplymentCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(AlipayApplymentCommand request, CancellationToken cancellationToken)
        {
            if (request.input == null)
                throw ExceptionHelper.AppFriendly("参数异常");

            var input = request.input;

            // 获取二级商户支付方式
            var paymentMethodInfo = await context.M_PaymentMethod.FirstOrDefaultAsync(w => w.Id == input.PaymentOriginId);
            if (paymentMethodInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            if (input.MerchantType == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            var certInfo = GetCertByMerchantType(((int)input.MerchantType).ToString(), input.LegalCertNo, input.LegalCertName, input.LegalCertFrontImage, input.LegalCertBackImage,
                input.UnifiedSocialCreditCode, input.BusinessLicenseImage, input.BusinessLicenseName);

            paymentMethodInfo.SetDomesticMerchantType(certInfo.DomesticMerchantType);
            context.M_PaymentMethod.Update(paymentMethodInfo); // 二级商户支付方式更新商户类型

            var site = new SiteInfo
            {
                SiteType = "07",
                SiteUrl = "http://coffmer.ourvend.com/",
                SiteName = "云数信息",
                TinyAppId = "2021004100645638",
            };

            var entity = new M_PaymentAlipayApplymentsInput
            {
                PaymentOriginId = input.PaymentOriginId,
                Mcc = "B0052",
                MerchantName = certInfo.CertName, // 二级商户名称，与证件名称相同
                MerchantShortName = input.MerchantShortName,
                MerchantType = input.MerchantType,
                LegalCertNo = input.LegalCertNo, // 证件号码
                LegalCertType = (LegalCertTypeEnum)Convert.ToInt32(certInfo.CertType),//证件类型
                LegalCertFrontImage = input.LegalCertFrontImage, // 证件照正面
                LegalCertBackImage = input.LegalCertBackImage, // 证件
                Phone = input.Phone,
                LegalCertAddress = input.LegalCertAddress,
                LegalCertValidTimeBegin = input.LegalCertValidTimeBegin,
                LegalCertName = input.LegalCertName, // 证件姓名
                LegalCertValidTimeEnd = input.LegalCertValidTimeEnd,
                InDoorImages = input.InDoorImages,
                OutDoorImages = input.OutDoorImages,
                BusinessAddress = input.BusinessAddress,// JsonConvert.SerializeObject(input.BusinessAddress),
                //BusinessAddressDetail = string.IsNullOrWhiteSpace(input.BusinessAddressDetail)
                //                        ? JsonConvert.SerializeObject(input.BusinessAddress) ?? ""
                //                        : input.BusinessAddressDetail,
                BusinessAddressDetail = JsonConvert.SerializeObject(input.BusinessAddress),
                UnifiedSocialCreditCode = input.UnifiedSocialCreditCode,
                BusinessLicenseName = input.BusinessLicenseName,
                LicenseAuthLetterImage = input.LicenseAuthLetterImage,
                ServicePhone = input.ServicePhone,
                ContactInfos = JsonConvert.SerializeObject(new ContactInfo
                {
                    Name = input.LegalCertName,
                    Mobile = input.Phone,
                    Type = input.LegalCertType.ToString()
                }),
                BusinessLicenseImage = input.BusinessLicenseImage,
                Service = ["当面付"],
                SignTimeWithIsv = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                ApplyTime = DateTime.UtcNow,
                AlipayLogonId = input.AlipayLogonId,
                Sites = JsonConvert.SerializeObject(new List<SiteInfo> { site }),
                // CertName = certInfo.CertName,
                // CertImageBack = certInfo.CertImageBack,
                BindingAlipayLogonId = certInfo.CertName, // 要求与name字段相同
                                                          // LegalCertType = input.LegalCertType,
                FlowStatus = ApplymentFlowStatusEnum.Initialize,
                BusinessLicenseAddress = input.BusinessLicenseAddress,
                BusinessLicenseLegalName = input.BusinessLicenseLegalName,
                IsArtificialApplyment = ArtificialApplymentEnum.No
            };

            var isAdd = input.Id == 0;
            long nid = 0;

            if (isAdd)
            {
                // 进件信息
                var info = mapper.Map<M_PaymentAlipayApplyments>(entity);
                info.BusinessAddress = JsonConvert.SerializeObject(entity.BusinessAddress);
                info.Service = JsonConvert.SerializeObject(entity.Service);
                info.SetId(YitIdHelper.NextId());
                info.SetFlowStatus(ApplymentFlowStatusEnum.Initialize);
                info.SetArtificialApplyment(ArtificialApplymentEnum.No);
                info.EnterpriseinfoId = _user.TenantId;
                info.LastModifyTime = DateTime.Now;
                info.LastModifyUserId = _user.UserId;
                info.LastModifyUserName = _user.NickName;
                nid = info.Id;
                await context.M_PaymentAlipayApplyments.AddAsync(info);
            }
            else
            {
                var sourceInfo = await context.M_PaymentAlipayApplyments
                    .FirstAsync(w => w.Id == input.Id);

                // 将 input 的内容映射到数据库已存在的实体
                mapper.Map(entity, sourceInfo);
                sourceInfo.Service = JsonConvert.SerializeObject(entity.Service);

                // 更新进件状态
                sourceInfo.SetFlowStatus(ApplymentFlowStatusEnum.Initialize);

                // 保持一些字段不被覆盖
                sourceInfo.CreateTime = sourceInfo.CreateTime;
                sourceInfo.LastModifyTime = DateTime.Now;
                sourceInfo.LastModifyUserId = _user.UserId;
                sourceInfo.LastModifyUserName = _user.NickName;
                nid = sourceInfo.Id;

                context.M_PaymentAlipayApplyments.Update(sourceInfo);
            }

            // 发送支付宝商户进件队列消息
            await _cap.SendDelayMessage(CapConst.AlipayMerchantIncomingSubmit, nid, 2);

            return true;
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
        private static CertTypeDto GetCertByMerchantType(string merchantType, string legalCertNo, string legalCertName, string legalCertFrontImage,
            string legalCertBackImage, string? unifiedSocialCreditCode, string? businessLicenseImage, string? businessLicenseName)
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
                    _ => "0"
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
                    CertNo = legalCertNo,
                    CertImage = legalCertFrontImage,
                    CertImageBack = legalCertBackImage,
                    CertName = legalCertName,
                    DomesticMerchantType = DomesticMerchantTypeEnum.Individual
                };
            }

            // 默认返回空对象
            return new CertTypeDto();
        }
    }
}