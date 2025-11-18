using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Settings
{
    /// <summary>
    /// 界面样式
    /// </summary>
    public class MaterialBoxEntityConfiguration : IEntityTypeConfiguration<MaterialBox>
    {
        /// <summary>
        /// 配置
        /// </summary>
        public void Configure(EntityTypeBuilder<MaterialBox> eb)
        {
            eb.ToTable("MaterialBox");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.SettingInfoId).IsRequired();
            eb.Property(t => t.Name).IsRequired();
            eb.Property(t => t.MinMeasureWarning).IsRequired();
            eb.Property(t => t.MinQuantityWarning).IsRequired();
            eb.Property(t => t.IsActive).IsRequired();

            //eb.HasOne<SettingInfo>().WithMany(m => m.MaterialBoxs).HasForeignKey(k => k.SettingInfoId).OnDelete(DeleteBehavior.Restrict).IsRequired();
        }
    }
}
