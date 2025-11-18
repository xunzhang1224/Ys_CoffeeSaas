using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Dictionary;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Settings
{
    /// <summary>
    /// 字典
    /// </summary>
    public class DictionaryConfiguration : IEntityTypeConfiguration<DictionaryEntity>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DictionaryEntity> eb)
        {
            //eb.ToTable("DictionaryEntity");
            //eb.HasKey(t => t.Key);
            //eb.HasIndex(t => t.IsEnabled);
            //eb.HasIndex(x => x.Value)
            //    .IsUnique();
            //eb.HasOne(x => x.Parent)
            //    .WithMany(x => x.DictionarySubs)
            //    .HasForeignKey(x => x.ParentKey)
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Restrict);

            eb.ToTable("DictionaryEntity");
            eb.HasKey(t => t.Key);
            eb.Property(t => t.Key).IsRequired();
            eb.Property(t => t.Value).IsRequired();
            eb.Property(t => t.ParentKey).IsRequired(false);
            eb.Property(t => t.IsEnabled).HasConversion<string>().HasMaxLength(30).IsRequired();

            ////父子级关联
            //eb.HasMany(m => m.DictionarySubs).WithOne(e => e.Parent).HasForeignKey(k => k.ParentKey).OnDelete(DeleteBehavior.Restrict);
            eb.HasMany(m => m.DictionarySubs)
                .WithOne(e => e.Parent)
                .HasForeignKey(k => k.ParentKey) // 外键是 ParentKey
                .HasPrincipalKey(d => d.Key)   // 关联到主表的主键 Key
                .OnDelete(DeleteBehavior.Restrict);// 避免循环级联删除
        }
    }
}
