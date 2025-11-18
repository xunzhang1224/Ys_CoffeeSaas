using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.DomesticPayment
{
    /// <summary>
    /// 支付宝进件申请配置
    /// </summary>
    public class M_PaymentAlipayApplymentsEntityConfiguration : IEntityTypeConfiguration<M_PaymentAlipayApplyments>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<M_PaymentAlipayApplyments> eb)
        {
            eb.ToTable("M_PaymentAlipayApplyments");

            // 主键配置
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();

            // 其他属性配置
            eb.Property(t => t.PaymentOriginId)
                .HasColumnName("PaymentOriginId")
                .IsRequired();

            eb.Property(t => t.OrderId)
                .HasColumnName("OrderId")
                .HasMaxLength(64)
                .IsRequired(false);

            eb.Property(t => t.MerchantType)
                .HasColumnName("MerchantType")
                .HasMaxLength(20)
                .IsRequired();

            eb.Property(t => t.Mcc)
                .HasColumnName("Mcc")
                .HasMaxLength(10)
                .IsRequired();

            eb.Property(t => t.LegalCertType)
                .HasColumnName("LegalCertType")
                .HasMaxLength(16)
                .IsRequired();

            eb.Property(t => t.LegalCertFrontImage)
                .HasColumnName("LegalCertFrontImage")
                .HasMaxLength(255)
                .IsRequired();

            eb.Property(t => t.LegalCertBackImage)
                .HasColumnName("LegalCertBackImage")
                .HasMaxLength(255)
                .IsRequired();

            eb.Property(t => t.LegalCertName)
                .HasColumnName("LegalCertName")
                .HasMaxLength(64)
                .IsRequired();

            eb.Property(t => t.Phone)
                .HasColumnName("Phone")
                .HasMaxLength(20)
                .IsRequired();

            eb.Property(t => t.LegalCertNo)
                .HasColumnName("LegalCertNo")
                .HasMaxLength(64)
                .IsRequired();

            eb.Property(t => t.LegalCertAddress)
                .HasColumnName("LegalCertAddress")
                .HasMaxLength(100)
                .IsRequired();

            eb.Property(t => t.LegalCertValidTimeBegin)
                .HasColumnName("LegalCertValidTimeBegin")
                .HasMaxLength(50)
                .IsRequired(false);

            eb.Property(t => t.LegalCertValidTimeEnd)
                .HasColumnName("LegalCertValidTimeEnd")
                .HasMaxLength(50)
                .IsRequired(false);

            eb.Property(t => t.MerchantShortName)
                .HasColumnName("MerchantShortName")
                .HasMaxLength(128)
                .IsRequired();

            eb.Property(t => t.BusinessAddress)
                .HasColumnName("BusinessAddress")
                .HasMaxLength(1000)
                .IsRequired();

            eb.Property(t => t.ServicePhone)
                .HasColumnName("ServicePhone")
                .HasMaxLength(20)
                .IsRequired();

            eb.Property(t => t.BusinessAddressDetail)
                .HasColumnName("BusinessAddressDetail")
                .HasMaxLength(256)
                .IsRequired();

            eb.Property(t => t.InDoorImages)
                .HasColumnName("InDoorImages")
                .HasMaxLength(256)
                .IsRequired();

            eb.Property(t => t.OutDoorImages)
                .HasColumnName("OutDoorImages")
                .HasMaxLength(256)
                .IsRequired();

            eb.Property(t => t.BusinessLicenseImage)
                .HasColumnName("BusinessLicenseImage")
                .HasMaxLength(256)
                .IsRequired(false);

            eb.Property(t => t.BusinessTimeBegin)
                .HasColumnName("BusinessTimeBegin")
                .HasMaxLength(20)
                .IsRequired(false);

            eb.Property(t => t.BusinessTimeEnd)
                .HasColumnName("BusinessTimeEnd")
                .HasMaxLength(20)
                .IsRequired(false);

            eb.Property(t => t.AlipayLogonId)
                .HasColumnName("AlipayLogonId")
                .HasMaxLength(64)
                .IsRequired();

            eb.Property(t => t.UnifiedSocialCreditCode)
                .HasColumnName("UnifiedSocialCreditCode")
                .HasMaxLength(32)
                .IsRequired(false);

            eb.Property(t => t.BusinessLicenseName)
                .HasColumnName("BusinessLicenseName")
                .HasMaxLength(64)
                .IsRequired(false);

            eb.Property(t => t.BusinessLicenseLegalName)
                .HasColumnName("BusinessLicenseLegalName")
                .HasMaxLength(64)
                .IsRequired(false);

            eb.Property(t => t.BusinessLicenseAddress)
                .HasColumnName("BusinessLicenseAddress")
                .HasMaxLength(256)
                .IsRequired(false);

            eb.Property(t => t.ContactInfos)
                .HasColumnName("ContactInfos")
                .HasMaxLength(1000)
                .IsRequired(false);

            eb.Property(t => t.BizCards)
                .HasColumnName("BizCards")
                .HasMaxLength(2000)
                .IsRequired(false);

            eb.Property(t => t.LicenseAuthLetterImage)
                .HasColumnName("LicenseAuthLetterImage")
                .HasMaxLength(256)
                .IsRequired(false);

            eb.Property(t => t.Service)
                .HasColumnName("Service")
                .HasMaxLength(100)
                .IsRequired();

            eb.Property(t => t.SignTimeWithIsv)
                .HasColumnName("SignTimeWithIsv")
                .HasMaxLength(20)
                .IsRequired(false);

            eb.Property(t => t.Sites)
                .HasColumnName("Sites")
                .HasMaxLength(2000)
                .IsRequired(false);

            eb.Property(t => t.InvoiceInfo)
                .HasColumnName("InvoiceInfo")
                .HasMaxLength(2000)
                .IsRequired(false);

            eb.Property(t => t.BindingAlipayLogonId)
                .HasColumnName("BindingAlipayLogonId")
                .HasMaxLength(64)
                .IsRequired();

            eb.Property(t => t.Addtime)
                .HasColumnName("Addtime")
                .IsRequired(false);

            eb.Property(t => t.MerchantName)
                .HasColumnName("MerchantName")
                .HasMaxLength(128)
                .IsRequired();

            eb.Property(t => t.ApplyTime)
                .HasColumnName("ApplyTime")
                .IsRequired(false);

            eb.Property(t => t.ExtInfo)
                .HasColumnName("ExtInfo")
                .HasMaxLength(2048)
                .IsRequired(false);

            eb.Property(t => t.Smid)
                .HasColumnName("Smid")
                .HasMaxLength(50)
                .IsRequired(false);

            eb.Property(t => t.CardAliasNo)
                .HasColumnName("CardAliasNo")
                .HasMaxLength(50)
                .IsRequired(false);

            eb.Property(t => t.Memo)
                .HasColumnName("Memo")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.FlowStatus)
                .HasColumnName("Flowstatus")
                .IsRequired();

            eb.Property(t => t.RejectReason)
                .HasColumnName("RejectReason")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.AppAuthToken)
                .HasColumnName("AppAuthToken")
                .HasMaxLength(64)
                .IsRequired(false);

            eb.Property(t => t.VerifyUserId)
                .HasColumnName("VerifyUserId")
                .IsRequired(false);

            eb.Property(t => t.VerifyTime)
                .HasColumnName("VerifyTime")
                .IsRequired(false);

            eb.Property(t => t.IsArtificialApplyment)
                .HasColumnName("IsArtificialApplyment")
                .HasDefaultValue(ArtificialApplymentEnum.No)
                .IsRequired(false);
        }
    }
}
