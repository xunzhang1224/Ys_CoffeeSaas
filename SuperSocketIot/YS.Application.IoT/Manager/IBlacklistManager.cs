namespace YS.Application.IoT.Manager
{
    /// <summary>
    /// 黑名单管理
    /// </summary>
    public interface IBlacklistManager
    {
        /// <summary>
        /// 添加黑名单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        Task AddToBlacklist(string item, BlacklistTypeEnum type, int threshold = 3);

        /// <summary>
        /// 判断是否在黑名单中
        /// </summary>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<bool> IsInBlacklist(string item, BlacklistTypeEnum type);

        /// <summary>
        /// 移除黑名单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task RemoveFromBlacklist(string item, BlacklistTypeEnum type);
    }
}