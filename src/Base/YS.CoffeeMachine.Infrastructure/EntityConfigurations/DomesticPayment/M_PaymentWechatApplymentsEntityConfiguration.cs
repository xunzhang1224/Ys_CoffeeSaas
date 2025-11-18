using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.DomesticPayment
{
    /// <summary>
    /// 微信进件申请配置
    /// </summary>
    public class M_PaymentWechatApplymentsEntityConfiguration : IEntityTypeConfiguration<M_PaymentWechatApplyments>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<M_PaymentWechatApplyments> eb)
        {
            eb.ToTable("M_PaymentWechatApplyments");

            // 主键配置
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();

            // 其他属性配置
            eb.Property(t => t.PaymentOriginId)
                .HasColumnName("PaymentOriginId")
                .IsRequired();

            eb.Property(t => t.IdDocType)
                .HasColumnName("IdDocType")
                .IsRequired();

            eb.Property(t => t.IdCardName)
                .HasColumnName("IdCardName")
                .HasMaxLength(50)
                .IsRequired();

            eb.Property(t => t.IdCardNumber)
                .HasColumnName("IdCardNumber")
                .HasMaxLength(30)
                .IsRequired();

            eb.Property(t => t.IdCardAddress)
                .HasColumnName("IdCardAddress")
                .HasMaxLength(255)
                .IsRequired();

            eb.Property(t => t.IdCardValidTimeBegin)
                .HasColumnName("IdCardValidTimeBegin")
                .HasMaxLength(20)
                .IsRequired();

            eb.Property(t => t.IdCardValidTime)
                .HasColumnName("IdCardValidTime")
                .HasMaxLength(20)
                .IsRequired();

            eb.Property(t => t.IdCardCopy)
                .HasColumnName("IdCardCopy")
                .HasMaxLength(255)
                .IsRequired();

            eb.Property(t => t.IdCardNational)
                .HasColumnName("IdCardNational")
                .HasMaxLength(255)
                .IsRequired();

            eb.Property(t => t.MobilePhone)
                .HasColumnName("MobilePhone")
                .HasMaxLength(20)
                .IsRequired();

            eb.Property(t => t.Email)
                .HasColumnName("Email")
                .HasMaxLength(50)
                .IsRequired();

            eb.Property(t => t.MerchantShortName)
                .HasColumnName("MerchantShortName")
                .HasMaxLength(50)
                .IsRequired();

            eb.Property(t => t.ServicePhone)
                .HasColumnName("ServicePhone")
                .HasMaxLength(20)
                .IsRequired();

            eb.Property(t => t.BizProvinceCode)
                .HasColumnName("BizProvinceCode")
                .HasMaxLength(10)
                .IsRequired(false);

            eb.Property(t => t.BizCityCode)
                .HasColumnName("BizCityCode")
                .HasMaxLength(10)
                .IsRequired(false);

            eb.Property(t => t.BizStoreAddress)
                .HasColumnName("BizStoreAddress")
                .HasMaxLength(200)
                .IsRequired(false);

            eb.Property(t => t.StoreEntrancePic)
                .HasColumnName("StoreEntrancePic")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.IndoorPic)
                .HasColumnName("IndoorPic")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.OrganizationType)
                .HasColumnName("OrganizationType")
                .IsRequired();

            eb.Property(t => t.BusinessLicenseCopy)
                .HasColumnName("BusinessLicenseCopy")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.BusinessLicenseNumber)
                .HasColumnName("BusinessLicenseNumber")
                .HasMaxLength(18)
                .IsRequired(false);

            eb.Property(t => t.BusinessTimeBegin)
                .HasColumnName("BusinessTimeBegin")
                .HasMaxLength(20)
                .IsRequired(false);

            eb.Property(t => t.BusinessTimeEnd)
                .HasColumnName("BusinessTimeEnd")
                .HasMaxLength(20)
                .IsRequired(false);

            eb.Property(t => t.MerchantName)
                .HasColumnName("MerchantName")
                .HasMaxLength(100)
                .IsRequired(false);

            eb.Property(t => t.LegalPerson)
                .HasColumnName("LegalPerson")
                .HasMaxLength(100)
                .IsRequired(false);

            eb.Property(t => t.LicenseAddress)
                .HasColumnName("LicenseAddress")
                .HasMaxLength(200)
                .IsRequired(false);

            eb.Property(t => t.CertificateLetterCopy)
                .HasColumnName("CertificateLetterCopy")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.AccountName)
                .HasColumnName("AccountName")
                .HasMaxLength(50)
                .IsRequired();

            eb.Property(t => t.AccountBank)
                .HasColumnName("AccountBank")
                .HasMaxLength(100)
                .IsRequired();

            eb.Property(t => t.BankAliasCode)
                .HasColumnName("BankAliasCode")
                .HasMaxLength(50)
                .IsRequired(false);

            eb.Property(t => t.AccountNumber)
                .HasColumnName("AccountNumber")
                .HasMaxLength(50)
                .IsRequired(false);

            eb.Property(t => t.BankProvinceCode)
                .HasColumnName("BankProvinceCode")
                .HasMaxLength(10)
                .IsRequired(false);

            eb.Property(t => t.BankCityCode)
                .HasColumnName("BankCityCode")
                .HasMaxLength(10)
                .IsRequired(false);

            eb.Property(t => t.BankBranchId)
                .HasColumnName("BankBranchId")
                .HasMaxLength(50)
                .IsRequired(false);

            eb.Property(t => t.BankName)
                .HasColumnName("BankName")
                .HasMaxLength(200)
                .IsRequired(false);

            eb.Property(t => t.ApplymentState)
                .HasColumnName("ApplymentState")
                .IsRequired(false);

            eb.Property(t => t.ApplymentStateDesc)
                .HasColumnName("ApplymentStateDesc")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.SignState)
                .HasColumnName("SignState")
                .HasMaxLength(16)
                .IsRequired(false);

            eb.Property(t => t.SignUrl)
                .HasColumnName("SignUrl")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.SubMchId)
                .HasColumnName("SubMchId")
                .HasMaxLength(32)
                .IsRequired(false);

            eb.Property(t => t.FAccountName)
                .HasColumnName("FAccountName")
                .HasMaxLength(100)
                .IsRequired(false);

            eb.Property(t => t.FAccountNo)
                .HasColumnName("FAccountNo")
                .HasMaxLength(50)
                .IsRequired(false);

            eb.Property(t => t.PayAmount)
                .HasColumnName("PayAmount")
                .IsRequired(false);

            eb.Property(t => t.DestinationAccountNumber)
                .HasColumnName("DestinationAccountNumber")
                .HasMaxLength(50)
                .IsRequired(false);

            eb.Property(t => t.DestinationAccountName)
                .HasColumnName("DestinationAccountName")
                .HasMaxLength(100)
                .IsRequired(false);

            eb.Property(t => t.DestinationAccountBank)
                .HasColumnName("DestinationAccountBank")
                .HasMaxLength(200)
                .IsRequired(false);

            eb.Property(t => t.DestinationCity)
                .HasColumnName("DestinationCity")
                .HasMaxLength(100)
                .IsRequired(false);

            eb.Property(t => t.DestinationRemark)
                .HasColumnName("DestinationRemark")
                .HasMaxLength(100)
                .IsRequired(false);

            eb.Property(t => t.Deadline)
                .HasColumnName("Deadline")
                .HasMaxLength(20)
                .IsRequired(false);

            eb.Property(t => t.LegalValidationUrl)
                .HasColumnName("LegalValidationUrl")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.AuditDetail)
                .HasColumnName("AuditDetail")
                .HasColumnType("text") // 使用文本类型存储长文本
                .IsRequired(false);

            eb.Property(t => t.ApplyState)
                .HasColumnName("ApplyState")
                .HasMaxLength(32)
                .IsRequired(false);

            eb.Property(t => t.RejectReason)
                .HasColumnName("RejectReason")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.FlowStatus)
                .HasColumnName("FlowStatus")
                .IsRequired();

            eb.Property(t => t.VerifyUserId)
                .HasColumnName("VerifyUserId")
                .IsRequired(false);

            eb.Property(t => t.VerifyTime)
                .HasColumnName("VerifyTime")
                .IsRequired(false);

            eb.Property(t => t.CreateUserId)
                .HasColumnName("CreateUserId")
                .IsRequired();

            eb.Property(t => t.CreatedOnUtc)
                .HasColumnName("CreatedOnUtc")
                .IsRequired();

            eb.Property(t => t.UpdateUserId)
                .HasColumnName("UpdateUserId")
                .IsRequired(false);

            eb.Property(t => t.UpdatedOnUtc)
                .HasColumnName("UpdatedOnUtc")
                .IsRequired(false);

            eb.Property(t => t.IsArtificialApplyment)
                .HasColumnName("IsArtificialApplyment")
                .HasDefaultValue(ArtificialApplymentEnum.No)
                .IsRequired(false);
        }
    }
}
