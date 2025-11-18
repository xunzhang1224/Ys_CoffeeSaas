using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Settings
{
    /// <summary>
    /// 语言信息
    /// </summary>
    public class LanguageInfoEntityConfiguration : IEntityTypeConfiguration<LanguageInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<LanguageInfo> eb)
        {
            eb.ToTable("LanguageInfo");
            eb.HasKey(t => t.Code);

            eb.HasMany(ur => ur.LanguageTextEntitys)
               .WithOne(x => x.Lang)
               .HasForeignKey(ur => ur.LangCode)
               //.OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
        }
    }
}
