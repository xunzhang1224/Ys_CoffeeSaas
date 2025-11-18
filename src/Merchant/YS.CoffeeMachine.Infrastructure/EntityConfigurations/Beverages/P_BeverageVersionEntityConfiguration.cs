using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Beverages
{
    public class P_BeverageVersionEntityConfiguration : IEntityTypeConfiguration<P_BeverageVersion>
    {
        public void Configure(EntityTypeBuilder<P_BeverageVersion> eb)
        {
            eb.ToTable("P_BeverageVersion");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.BeverageInfoId).IsRequired();
            eb.Property(t => t.VersionType).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.BeverageInfoDataString).HasColumnType("NVARCHAR(MAX)").IsRequired();//MEDIUMTEXT

            //关联饮品
            eb.HasOne<P_BeverageInfo>()
            .WithMany(e => e.BeverageVersions)
            .HasForeignKey(e => e.BeverageInfoId)
            .IsRequired();
        }
    }
}
