namespace YS.CoffeeMachine.Provider.IServices
{
    /// <summary>
    /// reids服务接口
    /// </summary>
    public interface IRedisService
    {
        /// <summary>
        /// 获取4位自增长编码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetIncrCodeAsync(string key);

        /// <summary>
        /// 分布式锁
        /// </summary>
        /// <param name="id">判断的key</param>
        /// <param name="value"></param>
        /// <param name="timeoutSeconds">默认24小时后自动释放</param>
        /// <returns></returns>
        Task<bool> SetNxAsync<T>(string id, T value, int timeoutSeconds = 1 * 24 * 3600);

        /// <summary>
        /// CreateMachineStickerCodesAsync
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> CreateMachineStickerCodesAsync(string key);

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task DelKeyAsync(string key);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="flieId"></param>
        /// <returns></returns>
        Task DelKeyAsync(string key, string flieId = null);

        /// <summary>
        /// 获取mid
        /// </summary>
        /// <returns></returns>
        Task<string> GetMidAsync();

        /// <summary>
        /// 设置哈希缓存
        /// </summary>
        /// <returns></returns>
        Task<bool> HSetAsync<T>(string key, string fieId, T value, TimeSpan expireTime);
        /// <summary>
        /// 设置哈希缓存
        /// </summary>
        /// <returns></returns>
        Task<T> HGetAsync<T>(string key, string fieId);
        /// <summary>
        /// 设置哈希缓存
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, T>> HGetAllAsync<T>(string key);

        /// <summary>
        /// 设置字符串缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        Task SetStringAsync(string key, string value, TimeSpan expireTime);

        /// <summary>
        /// 获取字符串缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetStringAsync(string key);
    }
}
