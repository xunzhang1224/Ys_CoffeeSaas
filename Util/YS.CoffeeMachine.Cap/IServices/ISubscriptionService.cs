namespace YS.CoffeeMachine.Cap.IServices
{
    /// <summary>
    /// 消息订阅服务接口
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>
        /// 启动订阅服务
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 停止订阅服务
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}
