using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.ApplicationUsers
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class ApplicationMenuEntityConfiguration : IEntityTypeConfiguration<ApplicationMenu>
    {
        /// <summary>
        /// 菜单
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<ApplicationMenu> eb)
        {
            eb.ToTable("ApplicationMenu");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.ParentId).IsRequired(false);
            eb.Property(t => t.SysMenuType).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.MenuType).HasConversion<string>().HasMaxLength(30);
            eb.Property(t => t.Title).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Path).HasMaxLength(128).IsRequired();
            eb.Property(t => t.Component).HasMaxLength(128).IsRequired(false);
            eb.Property(t => t.Redirect).HasMaxLength(128).IsRequired(false);
            eb.Property(t => t.Icon).HasMaxLength(128).IsRequired(false);
            eb.Property(t => t.ExtraIcon).HasMaxLength(128).IsRequired(false);
            eb.Property(t => t.EnterTransition).HasMaxLength(128).IsRequired(false);
            eb.Property(t => t.LeaveTransition).HasMaxLength(128).IsRequired(false);
            eb.Property(t => t.Auths).HasMaxLength(128).IsRequired(false);
            eb.Property(t => t.FrameSrc).HasMaxLength(256).IsRequired(false);
            eb.Property(t => t.FrameLoading).IsRequired();
            eb.Property(t => t.KeepAlive).IsRequired();
            eb.Property(t => t.HiddenTag).IsRequired();
            eb.Property(t => t.FixedTag).IsRequired();
            eb.Property(t => t.ShowLink).IsRequired();
            eb.Property(t => t.ShowParent).IsRequired();
            eb.Property(t => t.IsDefault).IsRequired();
            eb.Property(t => t.Remark).HasMaxLength(256).IsRequired(false);
            eb.Property(t => t.ActivePath).HasMaxLength(128).IsRequired(false);

            //父子级关联
            eb.HasMany(m => m.ApplicationMenus).WithOne().HasForeignKey(k => k.ParentId);

            //角色&菜单表主外键关联
            eb.HasMany<ApplicationRoleMenu>().WithOne(e => e.Menu).HasForeignKey(e => e.MenuId).IsRequired();
        }
    }
}
