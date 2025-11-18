namespace YS.Domain.IoT.Session
{
    using System;
    using System.Collections.Concurrent;
    using System.Text;
    using System.Threading.Tasks;
    using SuperSocket.Server.Abstractions.Session;

    /// <summary>
    /// 通用的会话管理器，用于管理物联网设备连接的会话对象。
    /// 支持异步操作，适用于基于 SuperSocket 的IoT通信场景。
    /// </summary>
    /// <typeparam name="TSession">表示具体的会话类型，必须实现 <see cref="IAppSession"/> 接口。</typeparam>
    public class SessionManager<TSession> where TSession : IAppSession
    {
        /// <summary>
        /// 存储当前所有活跃的会话对象，使用线程安全字典确保并发访问安全。
        /// Key 为会话标识符，Value 为对应的会话实例。
        /// </summary>
        public ConcurrentDictionary<string, TSession> Sessions { get; private set; } = new ();

        /// <summary>
        /// 获取当前存储的会话数量。
        /// </summary>
        public int Count => Sessions.Count;

        /// <summary>
        /// 初始化一个空的 SessionManager 实例。
        /// </summary>
        public SessionManager()
        {
        }

        /// <summary>
        /// 获取包含所有会话的字典副本。
        /// </summary>
        /// <returns>当前所有会话的集合。</returns>
        public ConcurrentDictionary<string, TSession> GetAllSessions()
        {
            return Sessions;
        }

        /// <summary>
        /// 异步尝试获取指定键的会话对象。
        /// </summary>
        /// <param name="key">会话的唯一标识键。</param>
        /// <returns>匹配的会话对象，如果不存在则返回 null。</returns>
        public virtual async Task<TSession?> TryGetAsync(string key)
        {
            return await Task.Run(() =>
            {
                Sessions.TryGetValue(key, out var session);
                return session;
            });
        }

        /// <summary>
        /// 异步添加或更新一个会话。
        /// 如果已存在相同键的会话，则先注销旧会话再进行替换。
        /// </summary>
        /// <param name="key">会话的唯一标识键。</param>
        /// <param name="session">要添加或更新的会话对象。</param>
        /// <returns>异步操作任务。</returns>
        public virtual async Task TryAddOrUpdateAsync(string key, TSession session)
        {
            await Task.Run(() =>
            {
                if (Sessions.TryGetValue(key, out var oldSession))
                {
                    Sessions.TryUpdate(key, session, oldSession);
                    if (oldSession != null)
                    {
                        (oldSession as SocketSession)?.LogOut(); // 登出旧会话
                    }
                }
                else
                {
                    Sessions.TryAdd(key, session);
                }
            });
        }

        /// <summary>
        /// 异步移除指定键的会话。
        /// </summary>
        /// <param name="key">要移除的会话键。</param>
        /// <returns>异步操作任务。</returns>
        public virtual async Task TryRemoveAsync(string key)
        {
            await Task.Run(() =>
            {
                Sessions.TryRemove(key, out _);
            });
        }

        /// <summary>
        /// 根据会话ID异步移除对应的会话。
        /// </summary>
        /// <param name="sessionId">目标会话的唯一ID。</param>
        /// <returns>异步操作任务。</returns>
        public virtual async Task TryRemoveBySessionIdAsync(string sessionId)
        {
            await Task.Run(() =>
            {
                foreach (var session in Sessions)
                {
                    if (session.Value.SessionID == sessionId)
                    {
                        Sessions.TryRemove(session);
                        return;
                    }
                }
            });
        }

        /// <summary>
        /// 根据会话实例异步移除对应的会话。
        /// </summary>
        /// <param name="tSession">目标会话实例。</param>
        /// <returns>异步操作任务。</returns>
        public virtual async Task TryRemoveBySessionAsync(TSession tSession)
        {
            await Task.Run(() =>
            {
                foreach (var session in Sessions)
                {
                    if (session.Value.SessionID == tSession.SessionID)
                    {
                        Sessions.TryRemove(session);
                        return;
                    }
                }
            });
        }

        /// <summary>
        /// 异步清除所有会话。
        /// </summary>
        /// <returns>异步操作任务。</returns>
        public virtual async Task TryRemoveAllAsync()
        {
            await Task.Run(() =>
            {
                Sessions.Clear();
            });
        }

        /// <summary>
        /// 向指定会话发送二进制数据。
        /// </summary>
        /// <param name="session">目标会话对象。</param>
        /// <param name="buffer">要发送的数据缓冲区。</param>
        /// <returns>异步操作任务。</returns>
        public virtual async Task SendAsync(TSession session, ReadOnlyMemory<byte> buffer)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            await session.SendAsync(buffer);
        }

        /// <summary>
        /// 向指定会话发送字符串消息（自动编码为UTF-8格式）。
        /// </summary>
        /// <param name="session">目标会话对象。</param>
        /// <param name="message">要发送的消息内容。</param>
        /// <returns>异步操作任务。</returns>
        public virtual async Task SendAsync(TSession session, string message)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            await session.SendAsync(Encoding.UTF8.GetBytes(message));
        }

        /// <summary>
        /// 根据会话键向指定会话发送字符串消息。
        /// </summary>
        /// <param name="key">会话的唯一标识键。</param>
        /// <param name="message">要发送的消息内容。</param>
        /// <returns>异步操作任务。</returns>
        public virtual async Task SendAsync(string key, string message)
        {
            var session = Sessions.FirstOrDefault(s => s.Key == key).Value;
            if (session != null)
            {
                await session.SendAsync(Encoding.UTF8.GetBytes(message));
            }
        }

        /// <summary>
        /// 根据指定会话对象查找其在管理器中的唯一标识键。
        /// </summary>
        /// <param name="session">目标会话对象。</param>
        /// <returns>匹配的会话键。</returns>
        public virtual async Task<string> FindIdBySessionAsync(TSession session)
        {
            return await Task.Run(() =>
            {
                return Sessions.First(x => x.Value.SessionID.Equals(session.SessionID)).Key;
            });
        }
    }
}