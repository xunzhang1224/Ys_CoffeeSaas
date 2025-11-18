namespace YS.CoffeeMachine.Cap
{
    using DotNetCore.CAP;
    using YS.CoffeeMachine.Cap.IServices;

    /// <summary>
    /// 消息发布服务实现
    /// </summary>
    public class PublishService(ICapPublisher _capPublisher) : IPublishService
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="top">消息主题</param>
        /// <param name="message">消息内容</param>
        /// <returns>异步任务</returns>
        public async Task SendMessage<T>(string top, T message)
        {
            await _capPublisher.PublishAsync(top, message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="top">消息主题</param>
        /// <param name="message">消息内容</param>
        /// <param name="callbackName">回调名称</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        public async Task SendMessage<T>(string top, T message, string? callbackName = null, CancellationToken cancellationToken = default)
        {
            await _capPublisher.PublishAsync(top, message, callbackName);
        }

        /// <summary>
        /// 发送延迟消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="delayTime"></param>
        /// <param name="top"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendDelayMessage<T>(string top, T message, int delayTime)
        {
            await _capPublisher.PublishDelayAsync(TimeSpan.FromSeconds(delayTime), top, message);
        }
    }
}