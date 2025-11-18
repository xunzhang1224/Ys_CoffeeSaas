using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.ApplicationUsers
{
    /// <summary>
    /// 应用角色菜单配置
    /// </summary>
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<ApplicationUser> eb)
        {
            eb.ToTable("ApplicationUser");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Account).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Password).HasMaxLength(int.MaxValue).IsRequired();
            eb.Property(t => t.NickName).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.AreaCode).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.Phone).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.VerifyPhone).IsRequired();
            eb.Property(t => t.Email).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Status).HasConversion<string>().HasMaxLength(30);
            eb.Property(t => t.AccountType).HasConversion<string>().HasMaxLength(30);
            eb.Property(t => t.SysMenuType).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.IsDefault).IsRequired();
            eb.Property(t => t.Remark).HasMaxLength(128).IsRequired(false);

            //企业&用户表主外键关联
            eb.HasMany<EnterpriseUser>().WithOne(e => e.User).HasForeignKey(e => e.UserId);

            //用户&角色表主外键关联
            eb.HasMany(t => t.ApplicationUserRoles).WithOne().HasForeignKey(w => w.UserId);

            eb.HasOne(t => t.ServiceAuthorizationRecord).WithOne(x => x.ServiceUser).HasForeignKey<ServiceAuthorizationRecord>(w => w.ServiceUserId).OnDelete(DeleteBehavior.NoAction); ;

        }
    }
}