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
    /// 企业角色
    /// </summary>
    public class EnterpriseTypesEntityConfiguration : IEntityTypeConfiguration<EnterpriseTypes>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<EnterpriseTypes> eb)
        {
            eb.ToTable("EnterpriseTypes");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Status).IsRequired();
            eb.Property(t => t.Astrict).IsRequired();
            eb.Property(t => t.IsDefault).IsRequired();
        }
    }
}
