using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Card;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DatabaseAccessor;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Card
{
    /// <summary>
    /// a
    /// </summary>
    public class CardInfoEntityConfiguration : IEntityTypeConfiguration<CardInfo>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Configure(EntityTypeBuilder<CardInfo> builder)
        {
            builder.ToTable("CardInfo");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.Property(t => t.CardNumber).IsRequired();
            builder.HasIndex(t => t.CardNumber).IsUnique();

            builder.HasMany(c => c.Assignments)
                  .WithOne(a => a.Card)
                  .HasForeignKey(a => a.CardId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
