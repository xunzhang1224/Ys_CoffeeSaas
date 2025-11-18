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
    public class CardDeviceAssignmentEntityConfiguration : IEntityTypeConfiguration<CardDeviceAssignment>
    {
        /// <summary>
        /// aa
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<CardDeviceAssignment> builder)
        {
            builder.ToTable("CardDeviceAssignment");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.HasIndex(a => new { a.CardId, a.DeviceId }).IsUnique();
            builder.HasIndex(a => a.CardId);
            builder.HasIndex(a => a.DeviceId);
        }
    }
}
