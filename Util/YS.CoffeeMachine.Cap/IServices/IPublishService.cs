namespace YS.CoffeeMachine.Cap.IServices
{
    /// <summary>
    /// 消息发布服务接口
    /// </summary>
    public interface IPublishService
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="top">消息主题</param>
        /// <param name="message">消息内容</param>
        /// <returns>异步任务</returns>
        Task SendMessage<T>(string top, T message);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="top">消息主题</param>
        /// <param name="message">消息内容</param>
        /// <param name="callbackName">回调名称</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        Task SendMessage<T>(string top, T message, string? callbackName = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 发送延迟消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="top"></param>
        /// <param name="message"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        Task SendDelayMessage<T>(string top, T message, int second);
    }
}
