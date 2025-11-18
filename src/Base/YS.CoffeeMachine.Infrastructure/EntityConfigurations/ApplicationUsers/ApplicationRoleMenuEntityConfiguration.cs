using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.ApplicationUsers
{
    /// <summary>
    /// 应用角色菜单配置
    /// </summary>
    public class ApplicationRoleMenuEntityConfiguration : IEntityTypeConfiguration<ApplicationRoleMenu>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<ApplicationRoleMenu> eb)
        {
            eb.ToTable("ApplicationRoleMenu");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.RoleId).IsRequired();
            eb.Property(t => t.MenuId).IsRequired();
        }
    }
}
