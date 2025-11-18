using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.Provide.Redis
{
    public static class RedisSetup
    {
        /// <summary>
        /// 从配置文件中加载默认配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisService(this IServiceCollection services, string section = "Redis")
        {
            // RedisConfiguration
            using (ServiceProvider provider = services.BuildServiceProvider())
            {
                IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
                if (configuration == null)
                {
                    throw new ArgumentNullException(nameof(IConfiguration));
                }
                IConfigurationSection section1 = configuration.GetSection(section);
                if (!section1.Exists())
                {
                    throw new Exception($"Config file not exist '{section}' section.");
                }

                string configString = section1.Value;

                var options = ConfigurationOptions.Parse(configString);

                services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(options));
                return services;
            }
        }
    }
}
