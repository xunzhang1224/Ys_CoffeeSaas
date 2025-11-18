namespace YS.Domain.IoT.Session
{
    using SuperSocket.Server.Abstractions.Session;

    /// <summary>
    /// 依赖于会话容器的服务类。
    /// 提供对同步与异步会话容器的访问能力，用于管理物联网设备连接会话。
    /// </summary>
    public class SessionContainerDependentService
    {
        /// <summary>
        /// 获取异步会话容器，用于异步操作设备会话。
        /// </summary>
        public IAsyncSessionContainer AsyncSessionContainer { get; private set; }

        /// <summary>
        /// 获取同步会话容器，用于同步操作设备会话。
        /// </summary>
        public ISessionContainer SessionContainer { get; private set; }

        /// <summary>
        /// 使用指定的会话容器初始化此类的新实例。
        /// </summary>
        /// <param name="sessionContainer">同步会话容器。</param>
        /// <param name="asyncSessionContainer">异步会话容器。</param>
        public SessionContainerDependentService(
            ISessionContainer sessionContainer,
            IAsyncSessionContainer asyncSessionContainer)
        {
            SessionContainer = sessionContainer;
            AsyncSessionContainer = asyncSessionContainer;
        }
    }
}