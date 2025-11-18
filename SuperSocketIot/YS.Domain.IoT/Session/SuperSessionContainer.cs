namespace YS.Domain.IoT.Session
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using SuperSocket.Server.Abstractions;
    using SuperSocket.Server.Abstractions.Session;

    /// <summary>
    /// 自定义会话容器，实现 ISuperSessionContainer 接口。
    /// 用于管理物联网设备的连接会话，支持注册、注销、查询等操作。
    /// </summary>
    public class SuperSessionContainer : ISuperSessionContainer
    {
        /// <summary>
        /// 存储当前所有活跃的会话对象，使用线程安全字典确保并发访问安全。
        /// Key 为会话ID，Value 为对应的会话实例。
        /// </summary>
        private readonly ConcurrentDictionary<string, IAppSession> _sessions;

        /// <summary>
        /// 初始化一个新的 SuperSessionContainer 实例。
        /// 使用忽略大小写的字符串比较器来存储会话。
        /// </summary>
        public SuperSessionContainer()
        {
            _sessions = new ConcurrentDictionary<string, IAppSession>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 根据指定的会话ID获取对应的会话对象。
        /// </summary>
        /// <param name="sessionID">目标会话的唯一标识。</param>
        /// <returns>匹配的会话对象，如果不存在则返回 null。</returns>
        public IAppSession GetSessionByID(string sessionID)
        {
            _sessions.TryGetValue(sessionID, out IAppSession session);
            return session;
        }

        /// <summary>
        /// 获取当前活跃的会话数量。
        /// </summary>
        /// <returns>会话总数。</returns>
        public int GetSessionCount()
        {
            return _sessions.Count;
        }

        /// <summary>
        /// 获取满足条件的所有已连接会话。
        /// </summary>
        /// <param name="criteria">筛选条件，可为空表示返回所有连接中的会话。</param>
        /// <returns>符合条件的会话集合。</returns>
        public IEnumerable<IAppSession> GetSessions(Predicate<IAppSession> criteria = null)
        {
            var enumerator = _sessions.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var s = enumerator.Current.Value;

                if (s.State != SessionState.Connected)
                    continue;

                if (criteria == null || criteria(s))
                    yield return s;
            }
        }

        /// <summary>
        /// 获取特定类型且满足条件的已连接会话。
        /// </summary>
        /// <typeparam name="TAppSession">期望的会话类型，必须实现 <see cref="IAppSession"/> 接口。</typeparam>
        /// <param name="criteria">筛选条件，可为空表示返回所有连接中的该类型会话。</param>
        /// <returns>符合条件的会话集合。</returns>
        public IEnumerable<TAppSession> GetSessions<TAppSession>(Predicate<TAppSession> criteria = null)
            where TAppSession : IAppSession
        {
            var enumerator = _sessions.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value is TAppSession s)
                {
                    if (s.State != SessionState.Connected)
                        continue;

                    if (criteria == null || criteria(s))
                        yield return s;
                }
            }
        }

        /// <summary>
        /// 注册一个新会话到容器中。
        /// 如果会话需要握手且尚未完成，则不会注册。
        /// </summary>
        /// <param name="session">要注册的会话对象。</param>
        /// <returns>异步操作结果，指示是否成功注册。</returns>
        public ValueTask<bool> RegisterSession(IAppSession session)
        {
            try
            {

                Console.WriteLine($"RegisterSession注册会话:{session is IHandshakeRequiredSession}");
                if (session is IHandshakeRequiredSession handshakeSession && !handshakeSession.Handshaked)
                {
                    Console.WriteLine($"RegisterSession注册会话进入if:{session is IHandshakeRequiredSession}&&{!handshakeSession.Handshaked}");
                    return new ValueTask<bool>(true); // 未完成握手，不注册
                }
                _sessions.TryAdd(session.SessionID, session);
                Console.WriteLine($"RegisterSession注册会话方法完成:{session is IHandshakeRequiredSession}");
                return new ValueTask<bool>(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RegisterSession注册会话方法报错:{ex.Message}----------------------------->{ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// 从容器中注销指定会话。
        /// </summary>
        /// <param name="session">要注销的会话对象。</param>
        /// <returns>异步操作结果，指示是否成功注销。</returns>
        public ValueTask<bool> UnRegisterSession(IAppSession session)
        {
            _sessions.TryRemove(session.SessionID, out _);
            return new ValueTask<bool>(true);
        }
    }
}