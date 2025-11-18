using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.ApplicationUsers
{
    /// <summary>
    /// 企业资质信息配置
    /// </summary>
    public class EnterpriseQualificationInfoCongifuration : IEntityTypeConfiguration<EnterpriseQualificationInfo>
    {
        /// <summary>
        /// 配置表结构
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<EnterpriseQualificationInfo> builder)
        {
            builder.ToTable("EnterpriseQualificationInfo");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.Property(t => t.LegalPersonName).HasMaxLength(64).IsRequired(false);
            builder.Property(t => t.CustomerServiceEmail).HasMaxLength(100).IsRequired(false);
            builder.Property(t => t.StoreAddress).HasMaxLength(500).IsRequired(false);
            builder.Property(t => t.LegalPersonIdCardNumber).HasMaxLength(64).IsRequired(false);
            builder.Property(t => t.FrontImageUrl).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(t => t.BackImageUrl).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(t => t.BusinessLicenseUrl).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(t => t.Othercertificate).HasColumnType("nvarchar(max)").IsRequired(false);

            // 企业资质信息&企业表主外键关联
            builder.HasOne(e => e.EnterpriseInfo)
                .WithMany()
                .HasForeignKey(e => e.EnterpriseId)
                .IsRequired();
        }
    }
}