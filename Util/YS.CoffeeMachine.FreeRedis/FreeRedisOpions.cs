namespace YS.CoffeeMachine.Provider
{
    /// <summary>
    /// freeRedis配置
    /// </summary>
    public class FreeRedisOpions
    {
        /// <summary>
        /// 是否打开OpenRedisCluster节点
        /// </summary>
        public bool OpenRedisClusterNodes { get; set; }

        /// <summary>
        /// Redis
        /// </summary>
        public string Redis { get; set; }

        /// <summary>
        /// RedisCluster节点
        /// </summary>
        public List<string> RedisClusterNodes { get; set; }
    }
}