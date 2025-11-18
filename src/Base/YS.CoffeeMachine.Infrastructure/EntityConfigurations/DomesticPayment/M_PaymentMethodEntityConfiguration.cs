using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.DomesticPayment
{
    /// <summary>
    /// 商户支付方式配置
    /// </summary>
    public class M_PaymentMethodEntityConfiguration : IEntityTypeConfiguration<M_PaymentMethod>
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<M_PaymentMethod> eb)
        {
            eb.ToTable("M_PaymentMethod");

            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();

            // 其他属性配置
            eb.Property(t => t.SystemPaymentMethodId)
                .HasColumnName("SystemPaymentMethodId")
                .IsRequired();

            eb.Property(t => t.Phone)
                .HasColumnName("Phone")
                .HasMaxLength(20) // 根据实际需求设置长度
                .IsRequired();

            eb.Property(t => t.DomesticMerchantType)
                .HasColumnName("DomesticMerchantType")
                .IsRequired(false);

            eb.Property(t => t.PaymentMode)
                .HasColumnName("PaymentMode")
                .HasDefaultValue(PaymentModeEnum.OnlinePayment)
                .IsRequired();

            eb.Property(t => t.PaymentEntryStatus)
                .HasColumnName("PaymentEntryStatus")
                .IsRequired();

            eb.Property(t => t.BindType)
                .HasColumnName("BindType")
                .HasDefaultValue(BindTypeEnum.AllDevice)
                .IsRequired();

            eb.Property(t => t.IsEnabled)
                .HasColumnName("IsEnabled")
                .HasDefaultValue(EnabledEnum.Enable)
                .IsRequired();

            eb.Property(t => t.Remark)
                .HasColumnName("Remark")
                .HasMaxLength(500) // 根据实际需求设置长度
                .IsRequired(false); // 根据注释可为空

            eb.Property(t => t.MerchantId)
                .HasColumnName("MerchantId")
                .HasMaxLength(50) // 根据实际需求设置长度
                .IsRequired();

            eb.Property(t => t.PaymentParameters)
                .HasColumnName("PaymentParameters")
                .HasColumnType("text") // 使用文本类型存储JSON
                .IsRequired(false); // 根据注释可为空

            eb.Property(t => t.SystemPaymentServiceProviderId)
                .HasColumnName("SystemPaymentServiceProviderId")
                .IsRequired();
        }
    }
}