using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.DomesticPayment
{
    /// <summary>
    /// 商户支付方式配置
    /// </summary>
    public class M_PaymentMethodBindDeviceConfiguration : IEntityTypeConfiguration<M_PaymentMethodBindDevice>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<M_PaymentMethodBindDevice> eb)
        {
            eb.ToTable("M_PaymentMethodBindDevice");

            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();

            eb.Property(t => t.PaymentMethodId).IsRequired();
            eb.Property(t => t.DeviceId).IsRequired();
        }
    }
}