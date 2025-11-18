namespace YS.Domain.IoT.Session
{
    using SuperSocket.Server.Abstractions.Session;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 定义用于管理物联网设备会话的核心容器接口。
    /// 提供注册、注销、查询等基础操作，支持泛型筛选。
    /// </summary>
    public interface ISuperSessionContainer
    {
        /// <summary>
        /// 异步注册一个会话到容器中。
        /// </summary>
        /// <param name="session">要注册的会话对象。</param>
        /// <returns>异步操作结果，指示是否成功注册。</returns>
        ValueTask<bool> RegisterSession(IAppSession session);

        /// <summary>
        /// 异步从容器中注销指定的会话。
        /// </summary>
        /// <param name="session">要注销的会话对象。</param>
        /// <returns>异步操作结果，指示是否成功注销。</returns>
        ValueTask<bool> UnRegisterSession(IAppSession session);

        /// <summary>
        /// 根据会话ID获取对应的会话对象。
        /// </summary>
        /// <param name="sessionID">目标会话的唯一标识。</param>
        /// <returns>匹配的会话对象，如果不存在则返回 null。</returns>
        IAppSession GetSessionByID(string sessionID);

        /// <summary>
        /// 获取当前活跃的会话总数。
        /// </summary>
        /// <returns>会话数量。</returns>
        int GetSessionCount();

        /// <summary>
        /// 获取满足条件的所有已连接会话。
        /// </summary>
        /// <param name="criteria">筛选条件，可为空表示返回所有连接中的会话。</param>
        /// <returns>符合条件的会话集合。</returns>
        IEnumerable<IAppSession> GetSessions(Predicate<IAppSession> criteria = null);

        /// <summary>
        /// 获取特定类型且满足条件的已连接会话。
        /// </summary>
        /// <typeparam name="TAppSession">期望的会话类型，必须实现 <see cref="IAppSession"/> 接口。</typeparam>
        /// <param name="criteria">筛选条件，可为空表示返回所有连接中的该类型会话。</param>
        /// <returns>符合条件的会话集合。</returns>
        IEnumerable<TAppSession> GetSessions<TAppSession>(Predicate<TAppSession> criteria = null)
            where TAppSession : IAppSession;
    }
}