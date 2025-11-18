using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.ApplicationUsers
{
    /// <summary>
    /// 企业信息表
    /// </summary>
    public class EnterpriseInfoEntityConfiguration : IEntityTypeConfiguration<EnterpriseInfo>
    {
        /// <summary>
        /// 配置表结构
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<EnterpriseInfo> eb)
        {
            eb.ToTable("EnterpriseInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Name).HasMaxLength(200).IsRequired();
            eb.Property(t => t.AreaRelationId).IsRequired(false);
            eb.Property(t => t.EnterpriseTypeId).IsRequired(false);
            eb.Property(t => t.Pid).IsRequired(false);
            eb.Property(t => t.IsDefault).IsRequired();
            eb.Property(t => t.MenuIds).HasColumnType("nvarchar(max)").IsRequired();
            eb.Property(t => t.HalfMenuIds).HasColumnType("nvarchar(max)").IsRequired(false);
            eb.Property(t => t.H5MenuIds).HasColumnType("nvarchar(max)").IsRequired();
            eb.Property(t => t.H5HalfMenuIds).HasColumnType("nvarchar(max)").IsRequired(false);
            eb.Property(t => t.Remark).IsRequired(false);

            //父子级关联
            eb.HasMany(m => m.EnterpriseInfos).WithOne().HasForeignKey(k => k.Pid);

            //企业&企业类型表主外键关联
            eb.HasOne(e => e.EnterpriseType)
                    .WithMany()
                    .HasForeignKey(e => e.EnterpriseTypeId)
                    .IsRequired();

            //企业&用户表主外键关联
            eb.HasMany(t => t.Users).WithOne().HasForeignKey(k => k.EnterpriseId);
            //企业&角色表主外键关联
            eb.HasMany(t => t.Roles).WithOne().HasForeignKey(k => k.EnterpriseId);
        }
    }
}
