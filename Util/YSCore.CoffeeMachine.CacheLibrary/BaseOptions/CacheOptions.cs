namespace YSCore.CoffeeMachine.CacheLibrary.BaseOptions
{
    /// <summary>
    /// 缓存配置类
    /// </summary>
    public class CacheOptions
    {
        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public string RedisConnectionString { get; set; } = string.Empty;
        /// <summary>
        /// 缓存默认过期时间
        /// </summary>
        public TimeSpan DefaultExpiryTime { get; set; } = TimeSpan.FromMinutes(10);
    }
}