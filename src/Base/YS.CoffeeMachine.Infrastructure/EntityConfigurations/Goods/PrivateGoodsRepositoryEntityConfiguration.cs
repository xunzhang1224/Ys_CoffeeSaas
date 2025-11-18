using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Goods;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Goods
{
    /// <summary>
    /// a
    /// </summary>
    public class PrivateGoodsRepositoryEntityConfiguration : IEntityTypeConfiguration<PrivateGoodsRepository>
    {
        /// <summary>
        /// b
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PrivateGoodsRepository> builder)
        {
            builder.ToTable("PrivateGoodsRepository");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.HasIndex(t => new { t.EnterpriseinfoId, t.Sku }).IsUnique();
        }
    }
}