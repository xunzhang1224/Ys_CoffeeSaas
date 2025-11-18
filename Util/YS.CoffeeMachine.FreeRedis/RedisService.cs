namespace YS.CoffeeMachine.Provider
{
    using FreeRedis;
    using YS.CoffeeMachine.Provider.IServices;

    /// <summary>
    /// Redis服务
    /// </summary>
    /// <param name="redisClient"></param>
    public class RedisService(IRedisClient redisClient) : IRedisService
    {
        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task DelKeyAsync(string key)
        {
            await redisClient.DelAsync(key);
        }

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param
        /// <param name="flieId"></param>
        /// <returns></returns>
        public async Task DelKeyAsync(string key, string flieId = null)
        {
            if (string.IsNullOrWhiteSpace(flieId))
                await redisClient.DelAsync(key);
            else
                await redisClient.HDelAsync(key, flieId);
        }

        /// <summary>
        /// 获取incrCode
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetIncrCodeAsync(string key)
        {
            string dateStr = DateTime.Now.ToString("yyyyMMdd");
            key += dateStr;
            long id = await redisClient.IncrAsync(key);
            await redisClient.ExpireAsync(key, TimeSpan.FromDays(2));
            return $"{key}{id:D4}";
        }

        /// <summary>
        /// 获取mid
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMidAsync()
        {
            string key = DateTime.Now.ToString("yyyyMM");
            long id = await redisClient.IncrAsync(key);
            await redisClient.ExpireAsync(key, TimeSpan.FromDays(31));
            return $"{key}{id:D4}";
        }

        /// <summary>
        /// 获取设备StickerCodes
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> CreateMachineStickerCodesAsync(string key)
        {
            long id = await redisClient.IncrAsync(key);
            await redisClient.ExpireAsync(key, TimeSpan.FromDays(31));
            return $"{key}{id:D6}";
        }

        /// <summary>
        /// 设置Nx
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<bool> SetNxAsync<T>(string id, T value, int timeoutSeconds = 1 * 24 * 3600)
        {
            return await redisClient.SetNxAsync(id, value, timeoutSeconds);
        }

        /// <summary>
        /// HSetAsync
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public async Task<bool> HSetAsync<T>(string key, string fieId, T value, TimeSpan expireTime)
        {
            await redisClient.HSetAsync(key, fieId, value);
            return await redisClient.ExpireAsync(key, expireTime);
        }

        /// <summary>
        /// HGetAsync
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key"></param>
        /// <param name="fieId"></param>
        /// <returns></returns>
        public async Task<T> HGetAsync<T>(string key, string fieId)
        {
            return await redisClient.HGetAsync<T>(key, fieId);
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> HGetAllAsync<T>(string key)
        {
            return await redisClient.HGetAllAsync<T>(key);
        }

        /// <summary>
        /// 设置string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public async Task SetStringAsync(string key, string value, TimeSpan expireTime)
        {
            await redisClient.SetAsync(key, value, expireTime);
        }

        /// <summary>
        /// 获取string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string key)
        {
            return await redisClient.GetAsync<string>(key);
        }
    }
}