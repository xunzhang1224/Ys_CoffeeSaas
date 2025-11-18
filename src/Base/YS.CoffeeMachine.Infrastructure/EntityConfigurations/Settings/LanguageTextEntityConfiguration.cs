using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Settings
{
    /// <summary>
    /// 语言文本
    /// </summary>
    public class LanguageTextEntityConfiguration : IEntityTypeConfiguration<LanguageTextEntity>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<LanguageTextEntity> eb)
        {
            eb.ToTable("SysLanguageText");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.HasIndex(t => new { t.Code, t.LangCode }).IsUnique();

            eb.Property(t => t.Code).HasMaxLength(50).IsRequired();

            eb.Property(t => t.Value).HasMaxLength(int.MaxValue);

        }
    }
}
