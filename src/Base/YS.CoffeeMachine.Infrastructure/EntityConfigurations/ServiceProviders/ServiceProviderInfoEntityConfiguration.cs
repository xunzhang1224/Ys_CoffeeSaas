using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.ServiceProviders;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.ServiceProviders
{
    /// <summary>
    /// 服务商信息
    /// </summary>
    public class ServiceProviderInfoEntityConfiguration : IEntityTypeConfiguration<ServiceProviderInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<ServiceProviderInfo> eb)
        {
            eb.ToTable("ServiceProviderInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Tel).HasMaxLength(64).IsRequired();
        }
    }
}
