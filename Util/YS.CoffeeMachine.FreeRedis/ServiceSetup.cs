namespace YS.CoffeeMachine.Provider
{
    using FreeRedis;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using YS.CoffeeMachine.Provider.IServices;

    /// <summary>
    /// ServiceSetup
    /// </summary>
    public static class ServiceSetup
    {
        /// <summary>
        /// 注册FreeRedis
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFreeRedis(this IServiceCollection services)
        {
            using (ServiceProvider provider = services.BuildServiceProvider())
            {
                IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
                var redisConfig = configuration.GetSection("FreeRedis").Get<FreeRedisOpions>();

                if (redisConfig != null)
                {
                    services.AddSingleton<IRedisClient, RedisClient>(delegate
                    {
                        if (redisConfig.OpenRedisClusterNodes)
                        {
                            ConnectionStringBuilder connectionStringBuilder = new ConnectionStringBuilder
                            {
                                Host = redisConfig.RedisClusterNodes[0],
                            };
                            ConnectionStringBuilder[] clusterConnectionStrings = new ConnectionStringBuilder[1] { connectionStringBuilder };
                            return new RedisClient(clusterConnectionStrings)
                            {
                                Serialize = (object obj) => JsonConvert.SerializeObject(obj),
                                Deserialize = (string json, System.Type type) => JsonConvert.DeserializeObject(json, type)
                            };
                        }

                        return new RedisClient(redisConfig.Redis)
                        {
                            Serialize = (object obj) => JsonConvert.SerializeObject(obj),
                            Deserialize = (string json, System.Type type) => JsonConvert.DeserializeObject(json, type)
                        };

                    });
                }

                services.AddScoped<IRedisService, RedisService>();
            }

            return services;
        }

        private static IEnumerable<string> GetNodes(IEnumerable<IConfigurationSection> sections)
        {
            foreach (var item in sections)
            {
                yield return item.Value;
            }
        }
    }
}
