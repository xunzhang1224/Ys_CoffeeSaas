using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure.EntityConfigurations.Strategy;
using YSCore.Provider.EntityFrameworkCore;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure
{
    /// <summary>
    /// 时间序列数据库
    /// </summary>
    public class CoffeeMachineTimescaleDBContext : AppDbContextBase
    {
        private readonly UserHttpContext _user;

        /// <summary>
        /// 时间序列数据库
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mediator"></param>
        /// <param name="provider"></param>
        public CoffeeMachineTimescaleDBContext(DbContextOptions<CoffeeMachineTimescaleDBContext> options, IMediator mediator, IServiceProvider provider) : base(options, mediator, provider)
        {
            _user = provider.GetRequiredService<UserHttpContext>();
        }

        //public DbSet<DeviceEventLog> DeviceEventLog { get; set; }
        //public DbSet<DrinkMakeLog> DrinkMakeLog { get; set; }

        /// <summary>
        /// 设备运行日志
        /// </summary>
        public DbSet<DeviceOnlineLog> DeviceOnlineLog { get; set; }

        /// <summary>
        /// 设备运行日志
        /// </summary>
        public DbSet<DeviceMetricsLog> DeviceMetricsLog { get; set; }

        /// <summary>
        /// 设备运行日志
        /// </summary>
        public DbSet<PlatformOperationLog> PlatformOperationLog { get; set; }

        /// <summary>
        /// 设备运行日志
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DeviceOnlineLogEntityConfiguration()); // 加这一行，应用配置
            modelBuilder.ApplyConfiguration(new DeviceMetricsLogEntityConfiguration()); // 加这一行，应用配置
            modelBuilder.ApplyConfiguration(new PlatformOperationLogEntityConfiguration()); // 加这一行，应用配置
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            if (_user.TenantId > 0)
            {
                //// 租户 过滤器
                modelBuilder.AddQueryFilter<IEnterpriseFilter>(x => x.EnterpriseinfoId == _user.TenantId);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}