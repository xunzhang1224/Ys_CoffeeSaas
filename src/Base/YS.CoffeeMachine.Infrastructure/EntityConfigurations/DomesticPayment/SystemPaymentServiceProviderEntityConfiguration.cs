using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.DomesticPayment
{
    /// <summary>
    /// 支付的服务商表迁移配置
    /// </summary>
    public class SystemPaymentServiceProviderEntityConfiguration : IEntityTypeConfiguration<SystemPaymentServiceProvider>
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<SystemPaymentServiceProvider> eb)
        {
            eb.ToTable("SystemPaymentServiceProvider");

            // 主键配置
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();

            // 其他属性配置
            eb.Property(t => t.SpMerchantId)
                .HasColumnName("SpMerchantId")
                .HasMaxLength(20)
                .IsRequired();

            eb.Property(t => t.Name)
                .HasColumnName("Name")
                .HasMaxLength(50)
                .IsRequired();

            eb.Property(t => t.AppletAppID)
                .HasColumnName("AppletAppID")
                .HasMaxLength(50)
                .IsRequired(false);

            eb.Property(t => t.AppKey)
                .HasColumnName("AppKey")
                .HasColumnType("text") // 对应 SQL Server 的 text 类型或其他数据库的等价类型
                .IsRequired(false);

            eb.Property(t => t.ApiV3Key)
                .HasColumnName("ApiV3Key")
                .HasColumnType("text")
                .IsRequired(false);

            eb.Property(t => t.NotifyUrl)
                .HasColumnName("NotifyUrl")
                .HasMaxLength(100)
                .IsRequired(false);

            eb.Property(t => t.IsDefault)
                .HasColumnName("IsDefault")
                .IsRequired();

            eb.Property(t => t.PaymentPlatformType)
                .HasColumnName("PaymentPlatformType")
                .HasDefaultValue(PaymentPlatformTypeEnum.Wechat) // 根据实际情况设置默认值
                .IsRequired();

            // 微信支付特有字段
            eb.Property(t => t.CretFileUrl)
                .HasColumnName("CretFileUrl")
                .HasMaxLength(255) // 建议使用更合理的长度
                .IsRequired(false);

            eb.Property(t => t.CretPassWrod)
                .HasColumnName("CretPassWrod")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.PlatformSerialNumber)
                .HasColumnName("PlatformSerialNumber")
                .HasMaxLength(255)
                .IsRequired(false);

            eb.Property(t => t.PlatformPublicKey)
                .HasColumnName("PlatformPublicKey")
                .HasColumnType("text")
                .IsRequired(false);
        }
    }
}