using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.DomesticPayment
{
    /// <summary>
    /// 系统支付方式配置
    /// </summary>
    public class SystemPaymentMethodEntityConfiguration : IEntityTypeConfiguration<SystemPaymentMethod>
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<SystemPaymentMethod> eb)
        {
            eb.ToTable("SystemPaymentMethod");

            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();

            // 其他属性配置
            eb.Property(t => t.Name)
                .HasColumnName("Name")
                .HasMaxLength(100) // 建议设置合适的长度
                .IsRequired();

            eb.Property(t => t.FatherId)
                .HasColumnName("FatherId")
                .HasDefaultValue(0)
                .IsRequired();

            eb.Property(t => t.PaymentImage)
                .HasColumnName("PaymentImage")
                .HasMaxLength(500) // 建议设置合适的长度
                .IsRequired();

            eb.Property(t => t.OnlinePayment)
                .HasColumnName("OnlinePayment")
                .IsRequired();

            eb.Property(t => t.OfflinePayment)
                .HasColumnName("OfflinePayment")
                .IsRequired();

            eb.Property(t => t.Country)
                .HasColumnName("Country")
                .HasMaxLength(500) // 根据实际需求调整长度
                .IsRequired();

            eb.Property(t => t.LanguageTextCode)
                .HasColumnName("LanguageTextCode")
                .HasMaxLength(100) // 建议设置合适的长度
                .IsRequired();

            eb.Property(t => t.IsEnabled)
                .HasColumnName("IsEnabled")
                .HasDefaultValue(EnabledEnum.Enable) // 根据实际情况设置默认值
                .IsRequired();

            eb.Property(t => t.PaymentPlatformId)
                .HasColumnName("PaymentPlatformId")
                .HasDefaultValue(0)
                .IsRequired();
        }
    }
}