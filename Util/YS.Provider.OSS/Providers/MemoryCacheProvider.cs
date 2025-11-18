namespace YS.Provider.OSS.Providers
{
    using Microsoft.Extensions.Caching.Memory;
    using YS.Provider.OSS.Interface.Base;

    /// <summary>
    /// MemoryCacheProvider
    /// </summary>
    internal class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;

        /// <summary>
        /// MemoryCacheProvider
        /// </summary>
        /// <param name="cache"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MemoryCacheProvider(IMemoryCache cache)
        {
            this._cache = cache ?? throw new ArgumentNullException(nameof(IMemoryCache));
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            return _cache.Get<T>(key);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ts"></param>
        public void Set<T>(string key, T value, TimeSpan ts) where T : class
        {
            _cache.Set(key, value, ts);
        }
    }
}
