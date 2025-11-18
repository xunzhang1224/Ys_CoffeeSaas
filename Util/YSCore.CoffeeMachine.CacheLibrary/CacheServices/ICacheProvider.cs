namespace YSCore.CoffeeMachine.CacheLibrary.CacheServices
{
    /// <summary>
    /// 缓存提供者接口
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="getFromDatabase"></param>
        /// <param name="expiryTime"></param>
        /// <returns></returns>
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> getFromDatabase, TimeSpan? expiryTime = null);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(string key);

        /// <summary>
        /// 检查key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task RemoveMultipleAsync(IEnumerable<string> keys);
    }
}