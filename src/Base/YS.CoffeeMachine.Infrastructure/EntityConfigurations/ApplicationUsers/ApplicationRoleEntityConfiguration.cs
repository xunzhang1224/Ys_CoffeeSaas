using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.ApplicationUsers
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class ApplicationRoleEntityConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<ApplicationRole> eb)
        {
            eb.ToTable("ApplicationRole");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            //eb.Property(t => t.Code).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Status).HasConversion<string>().HasMaxLength(30);
            eb.Property(t => t.Type).HasConversion<string>().HasMaxLength(30);
            eb.Property(t => t.SysMenuType).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.IsDefault).IsRequired();
            eb.Property(t => t.HasSuperAdmin).IsRequired(false);
            eb.Property(t => t.Sort).IsRequired();
            eb.Property(t => t.Remark).HasMaxLength(128).IsRequired(false);

            //设置Code唯一
            //eb.HasIndex(i => i.Code).IsUnique();

            //用户&角色表主外键关联
            eb.HasMany<ApplicationUserRole>().WithOne(e => e.Role).HasForeignKey(e => e.RoleId).IsRequired();

            //企业&角色表主外键关联
            eb.HasMany<EnterpriseRole>().WithOne(e => e.Role).HasForeignKey(e => e.RoleId).IsRequired();

            //角色&菜单表主外键关联
            eb.HasMany(t => t.ApplicationRoleMenus).WithOne().HasForeignKey(w => w.RoleId);
        }
    }
}