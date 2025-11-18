using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Devices
{
    /// <summary>
    /// 分组
    /// </summary>
    public class GroupsEntityConfiguration : IEntityTypeConfiguration<Groups>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<Groups> eb)
        {
            eb.ToTable("Groups");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.EnterpriseInfoId).IsRequired();
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.PId).IsRequired(false);
            eb.Property(t => t.Remark).HasMaxLength(128).IsRequired(false);
            eb.Property(t => t.Path).HasColumnType("nvarchar(max)").IsRequired(false);

            //分组关联用户
            eb.HasMany(t => t.Users).WithOne().HasForeignKey(k => k.GroupsId);
            //分组关联设备
            eb.HasMany(t => t.Devices).WithOne().HasForeignKey(k => k.GroupsId);
        }
    }
}
