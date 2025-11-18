namespace YS.Provider.OSS.Interface.Base
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// 移除Key
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// 根据Key从缓存中获取对象
        /// </summary>
        /// <param name="key"></param>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// 缓存对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ts"></param>
        void Set<T>(string key, T value, TimeSpan ts) where T : class;
    }
}
