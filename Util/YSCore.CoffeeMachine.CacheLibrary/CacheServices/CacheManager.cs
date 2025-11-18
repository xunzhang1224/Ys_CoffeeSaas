using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using YSCore.CoffeeMachine.CacheLibrary.BaseOptions;

namespace YSCore.CoffeeMachine.CacheLibrary.CacheServices
{
    /// <summary>
    /// 缓存管理类
    /// </summary>
    public class CacheManager : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;
        /// <summary>
        /// Redis数据库
        /// </summary>
        private readonly IDatabase _redisDatabase;
        /// <summary>
        /// 信号量，用于控制缓存击穿问题
        /// </summary>
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        /// <summary>
        /// 缓存配置
        /// </summary>
        private readonly CacheOptions _cacheOptions;
        /// <summary>
        /// 内存缓存空值标记
        /// </summary>
        public static readonly object _cacheEmpty = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memoryCache"></param>
        /// <param name="cacheOptions"></param>
        public CacheManager(IMemoryCache memoryCache, CacheOptions cacheOptions)
        {
            // 内存缓存
            _memoryCache = memoryCache;
            // 缓存配置
            _cacheOptions = cacheOptions;
            // 连接 Redis 数据库
            _redisDatabase = ConnectionMultiplexer.Connect(_cacheOptions.RedisConnectionString).GetDatabase();
        }
        /// <summary>
        /// 获取缓存，如果缓存不存在，则从数据库加载
        /// </summary>
        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> getFromDatabase, TimeSpan? expiryTime = null)
        {
            // 1. 查找内存缓存
            if (_memoryCache.TryGetValue(key, out T cachedValue))
            {
                // 返回数据
                return cachedValue;
            }

            // 2. 查找 Redis 缓存
            var redisValue = await _redisDatabase.StringGetAsync(key);
            if (redisValue.HasValue)
            {
                // 反序列化数据
                cachedValue = Deserialize<T>(redisValue);
                // 设置内存缓存
                _memoryCache.Set(key, cachedValue, expiryTime ?? _cacheOptions.DefaultExpiryTime);
                // 返回数据
                return cachedValue;
            }

            // 3. 查询数据库并更新缓存
            cachedValue = await LoadDataWithCacheMissHandling(key, getFromDatabase);
            if (cachedValue != null)
            {
                // 序列化数据
                var serializedValue = Serialize(cachedValue);
                // 更新 Redis 缓存
                await _redisDatabase.StringSetAsync(key, serializedValue, expiryTime ?? _cacheOptions.DefaultExpiryTime);
                // 更新内存缓存
                _memoryCache.Set(key, cachedValue, expiryTime ?? _cacheOptions.DefaultExpiryTime);
            }

            // 返回数据
            return cachedValue;
        }

        /// <summary>
        /// 手动删除缓存
        /// </summary>
        public async Task RemoveAsync(string key)
        {
            // 删除 Redis 缓存
            await _redisDatabase.KeyDeleteAsync(key);
            // 删除内存缓存
            _memoryCache.Remove(key);
        }

        /// <summary>
        /// 检查缓存中是否存在指定的键
        /// </summary>
        public async Task<bool> ExistsAsync(string key)
        {
            // 先检查内存缓存
            if (_memoryCache.TryGetValue(key, out _))
                return true;
            // 再检查 Redis 缓存
            var existsInRedis = await _redisDatabase.KeyExistsAsync(key);
            // 返回结果
            return existsInRedis;
        }

        /// <summary>
        /// 删除多个缓存
        /// </summary>
        public async Task RemoveMultipleAsync(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                await RemoveAsync(key);
            }
        }

        /// <summary>
        /// 数据库未命中时的缓存击穿处理
        /// </summary>
        private async Task<T> LoadDataWithCacheMissHandling<T>(string key, Func<Task<T>> getFromDatabase)
        {
            // 控制缓存击穿问题，保证同一时刻只有一个请求去数据库加载数据
            await _semaphore.WaitAsync();

            // 不管成功与否都释放信号量，避免死锁
            try
            {
                // 再次检查缓存，避免多个线程同时去数据库加载
                if (_memoryCache.TryGetValue(key, out T cachedValue))
                {
                    return cachedValue;
                }

                // 再次检查 Redis 缓存
                var redisValue = await _redisDatabase.StringGetAsync(key);
                // 如果缓存中有数据，直接返回
                if (redisValue.HasValue)
                {
                    // 反序列化数据
                    cachedValue = Deserialize<T>(redisValue);
                    // 设置内存缓存
                    _memoryCache.Set(key, cachedValue, _cacheOptions.DefaultExpiryTime);
                    // 返回数据
                    return cachedValue;
                }

                // 如果缓存中也没有，去数据库加载
                cachedValue = await getFromDatabase();

                if (cachedValue == null)
                {
                    // 如果数据库返回 null，则缓存一个空值（防止缓存击穿）
                    var emptyValue = _cacheEmpty;  // 使用空值标识对象
                    // 序列化空值
                    var serializedEmptyValue = Serialize(emptyValue);
                    // 更新 Redis 缓存
                    await _redisDatabase.StringSetAsync(key, serializedEmptyValue, _cacheOptions.DefaultExpiryTime);
                    // 更新内存缓存
                    _memoryCache.Set(key, emptyValue, _cacheOptions.DefaultExpiryTime);
                    // 返回空值或 null
                    return default;
                }

                // 序列化数据
                var serializedValue = Serialize(cachedValue);
                // 更新 Redis 缓存
                await _redisDatabase.StringSetAsync(key, serializedValue, _cacheOptions.DefaultExpiryTime);
                // 更新内存缓存
                _memoryCache.Set(key, cachedValue, _cacheOptions.DefaultExpiryTime);

                // 返回数据
                return cachedValue;
            }
            finally
            {
                // 释放信号量
                _semaphore.Release();
            }
        }

        /// <summary>
        /// 序列化泛型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private byte[] Serialize<T>(T obj)
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        /// <summary>
        /// 反序列化泛型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private T Deserialize<T>(byte[] bytes)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(bytes);
        }
    }
}