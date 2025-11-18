using Microsoft.Extensions.Caching.Memory;

namespace YS.Application.IoT.Manager;

/// <summary>
/// 黑名单类型
/// </summary>
public enum BlacklistTypeEnum
{
    IP,
    Device
}

/// <summary>
/// 黑名单管理
/// </summary>
public class BlacklistManager : IBlacklistManager
{
    /// <summary>
    /// 内存缓存（使用内存缓存减少Redis访问）
    /// </summary>
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// 缓存过期时间（2小时）
    /// </summary>
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromHours(2);
    // 计数器过期时间
    private readonly TimeSpan _counterExpiration = TimeSpan.FromHours(24);

    /// <summary>
    /// 黑名单管理
    /// </summary>
    /// <param name="memoryCache"></param>
    public BlacklistManager(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    #region Public Methods

    /// <summary>
    /// 添加到黑名单
    /// </summary>
    /// <param name="item"></param>
    /// <param name="type"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task AddToBlacklist(string item, BlacklistTypeEnum type, int threshold = 3)
    {
        if (string.IsNullOrEmpty(item))
            throw new ArgumentException("Item cannot be null or empty.", nameof(item));
        // 先检查是否已在黑名单（内存缓存优先）
        string blacklistKey = GetKey(type);
        if (await IsInBlacklist(item, type))
        {
            return; // 已在黑名单则直接退出
        }
        string counterKey = GetCounterKey(item, type);
        string lockKey = $"lock:{counterKey}";

        // 使用Redis锁防止并发问题
        using (var redLock = RedisHelper.Lock(lockKey, 5)) // 5秒锁超时
        {
            if (redLock != null)
            {
                // 原子递增计数器
                long count = await RedisHelper.IncrByAsync(counterKey);
                await RedisHelper.ExpireAsync(counterKey, _counterExpiration);

                // 达到阈值后加入黑名单
                if (count >= threshold)
                {
                    await RedisHelper.SAddAsync(blacklistKey, item);
                    _memoryCache.Remove(blacklistKey);
                    //清除计数器
                    await RedisHelper.DelAsync(counterKey);
                }
            }
            else
            {
                throw new ArgumentException("获取分布式锁失败");
            }
        }
    }

    /// <summary>
    /// 检查是否在黑名单中
    /// </summary>
    public async Task<bool> IsInBlacklist(string item, BlacklistTypeEnum type)
    {
        try
        {
            if (string.IsNullOrEmpty(item)) return false;

            string key = GetKey(type);

            // 从内存缓存获取
            if (!_memoryCache.TryGetValue<HashSet<string>>(key, out var blacklist))
            {
                // 缓存不存在时从Redis加载
                blacklist = await UpdateCacheAsync(key);
            }

            return blacklist?.Contains(item) ?? false;
        }
        catch (Exception)
        {
            return false;
        }

    }

    /// <summary>
    /// 从黑名单中移除
    /// </summary>
    public async Task RemoveFromBlacklist(string item, BlacklistTypeEnum type)
    {
        if (string.IsNullOrEmpty(item)) return;

        string key = GetKey(type);
        // 使用Set集合删除，SREM命令时间复杂度O(1)
        await RedisHelper.SRemAsync(key, item);

        // 使本地缓存失效
        _memoryCache.Remove(key);
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// 更新本地缓存
    /// </summary>
    private async Task<HashSet<string>> UpdateCacheAsync(string key)
    {
        // 使用SMEMBERS命令获取全部成员（时间复杂度O(N)）
        var members = await RedisHelper.SMembersAsync(key);
        var set = new HashSet<string>(members ?? Array.Empty<string>());

        // 设置内存缓存（含过期时间）
        return _memoryCache.Set(key, set, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheExpiration
        });
    }

    /// <summary>
    /// 获取Redis键名（优化类型转换）
    /// </summary>
    private static string GetKey(BlacklistTypeEnum type)
    {
        return $"blacklist:{type.ToString().ToLower()}";
    }

    private string GetCounterKey(string item, BlacklistTypeEnum type)
     => $"blacklist_counter:{type.ToString().ToLower()}:{item}";
    #endregion
}