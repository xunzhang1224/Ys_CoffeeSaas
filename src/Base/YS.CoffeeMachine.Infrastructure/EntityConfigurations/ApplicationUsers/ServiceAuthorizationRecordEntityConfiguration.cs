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
    /// 授权记录
    /// </summary>
    public class ServiceAuthorizationRecordEntityConfiguration : IEntityTypeConfiguration<ServiceAuthorizationRecord>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<ServiceAuthorizationRecord> eb)
        {
            eb.ToTable("ServiceAuthorizationRecord");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.FounderId).IsRequired();
            eb.Property(t => t.ServiceUserAccount).IsRequired();
            eb.Property(t => t.ServiceUserId).IsRequired();
            eb.Property(t => t.AuthorizationStartTime).IsRequired();
            eb.Property(t => t.AuthorizationEndTime).IsRequired(false);
            eb.Property(t => t.State).HasConversion<string>().HasMaxLength(30);

            //授权&角色表主外键关联
            eb.HasMany(t => t.AuthorizedDevices).WithOne().HasForeignKey(w => w.ServiceAuthorizationRecordId);
        }
    }
}
