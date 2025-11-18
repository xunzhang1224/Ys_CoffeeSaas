
namespace YSCore.CoffeeMachine.SignalR.Services
{
    /// <summary>
    /// SignalR服务接口
    /// </summary>
    public interface ISignalRService
    {
        /// <summary>
        /// 向所有客户端推送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        Task SendMessageToAllAsync(string message, string topic = null);

        /// <summary>
        /// 向指定组推送消息
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="message"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        Task SendMessageToGroupAsync(string groupName, string message, string topic = null);

        /// <summary>
        /// 向指定用户推送消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        Task SendMessageToUserAsync(string userId, string message, string topic = null);

        /// <summary>
        /// 添加用户到组
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task AddToGroupAsync(string userId, string groupName);

        /// <summary>
        /// 从组中移除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task RemoveFromGroupAsync(string userId, string groupName);

        /// <summary>
        /// 断开指定用户连接
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DisconnectUserAsync(string userId);
    }
}