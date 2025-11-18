using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Beverages
{
    public class P_FormulaInfoEntityConfiguration : IEntityTypeConfiguration<P_FormulaInfo>
    {
        public void Configure(EntityTypeBuilder<P_FormulaInfo> eb)
        {
            eb.ToTable("P_FormulaInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.BeverageInfoId).IsRequired();
            eb.Property(t => t.MaterialBoxId).IsRequired(false);
            eb.Property(t => t.FormulaType).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.MaterialBoxName).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.SpecsString).HasColumnType("nvarchar(max)").IsRequired();

            //关联饮品
            eb.HasOne<P_BeverageInfo>()
            .WithMany(e => e.FormulaInfos)
            .HasForeignKey(e => e.BeverageInfoId)
            .IsRequired();
        }
    }
}
