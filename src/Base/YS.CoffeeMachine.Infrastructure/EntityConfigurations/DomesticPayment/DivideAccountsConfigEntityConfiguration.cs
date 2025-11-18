using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.DomesticPayment
{
    /// <summary>
    /// 支付分账配置表迁移配置
    /// </summary>
    public class DivideAccountsConfigEntityConfiguration : IEntityTypeConfiguration<DivideAccountsConfig>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DivideAccountsConfig> eb)
        {
            eb.ToTable("DivideAccountsConfig"); // 请替换为实际的表名

            // 主键配置
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();

            // 商户号
            eb.Property(t => t.MerchantId)
                .HasColumnName("MerchantId")
                .HasMaxLength(100)
                .HasComment("商户号")
                .IsRequired();

            // 商户名称
            eb.Property(t => t.MerchantName)
                .HasColumnName("MerchantName")
                .HasMaxLength(100)
                .HasComment("商户名称")
                .IsRequired();

            // 分账比例%
            eb.Property(t => t.Bibliography)
                .HasColumnName("Bibliography")
                .HasComment("分账比例%")
                .HasColumnType("decimal(18,2)") // 建议指定精度
                .IsRequired();

            // 备注
            eb.Property(t => t.Remarks)
                .HasColumnName("Remarks")
                .HasMaxLength(100)
                .HasComment("备注")
                .IsRequired(false); // 允许为空

            // 微信接收方类型（枚举）
            eb.Property(t => t.type)
                .HasColumnName("type")
                .HasMaxLength(100)
                .HasComment("微信接收方类型")
                .HasConversion<string>() // 将枚举存储为字符串
                .IsRequired(false);

            // 支付宝接收方类型（枚举）
            eb.Property(t => t.AlipayRoyaltyType)
                .HasColumnName("AlipayRoyaltyType")
                .HasMaxLength(100)
                .HasComment("支付宝接收方类型")
                .IsRequired(false);

            // 接收方账号
            eb.Property(t => t.account)
                .HasColumnName("account")
                .HasMaxLength(100)
                .HasComment("接收方账号")
                .IsRequired(false);

            // 分账接收方全称
            eb.Property(t => t.name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .HasComment("分账接收方全称,分账接收方真实姓名")
                .IsRequired(false);

            // 关系类型（枚举）
            eb.Property(t => t.relation_type)
                .HasColumnName("relation_type")
                .HasMaxLength(100)
                .HasComment("与分账方的关系类型")
                .HasConversion<string>() // 将枚举存储为字符串
                .IsRequired(false);

            // 绑定的设备（数组英文逗号隔开）
            eb.Property(e => e.VendCodes)
              .HasConversion(
                  v => string.Join(',', v),
                  v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(long.Parse)
                        .ToList())
              .HasColumnType("nvarchar(max)");

            // 是否启用
            eb.Property(t => t.IsEnabled)
                .HasColumnName("IsEnabled")
                .HasDefaultValue(EnabledEnum.Enable)
                .IsRequired();
        }
    }
}