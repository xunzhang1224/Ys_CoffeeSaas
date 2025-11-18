using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Files
{
    /// <summary>
    /// 文件关系
    /// </summary>
    public class FileRelationConfiguration : IEntityTypeConfiguration<FileRelation>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Configure(EntityTypeBuilder<FileRelation> builder)
        {
            builder.ToTable("FileRelation");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
        }
    }
}
