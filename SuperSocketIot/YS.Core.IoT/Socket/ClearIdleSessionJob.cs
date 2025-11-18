using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperSocket.Connection;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Session;
using YS.Domain.IoT.Option;
using YS.Domain.IoT.Session;
namespace YS.Core.IoT.Socket
{
    /// <summary>
    /// ClearIdleSessionJob
    /// </summary>
    public class ClearIdleSessionJob : BackgroundService
    {
        private ISuperSessionContainer _sessionContainer;
        private readonly SessionManager<SocketSession> _sessionManager;
        private SuperServerOptions _serverOptions;
        private Timer _timer;

        private ILogger<ClearIdleSessionJob> _logger;

        /// <summary>
        /// ClearIdleSessionJob
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="serverOptions"></param>
        /// <param name="logger"></param>
        /// <param name="sessionManager"></param>
        /// <exception cref="Exception"></exception>
        public ClearIdleSessionJob(IServiceProvider serviceProvider,
            IOptions<SuperServerOptions> serverOptions,
            ILogger<ClearIdleSessionJob> logger,
            SessionManager<SocketSession> sessionManager)
        {
            _sessionContainer = serviceProvider.GetService<ISuperSessionContainer>();

            if (_sessionContainer == null)
                throw new Exception($"{nameof(ClearIdleSessionJob)} needs a middleware of {nameof(ISessionContainer)}");

            _serverOptions = serverOptions.Value;
            _logger = logger;
            _sessionManager = sessionManager;
        }

        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(OnTimerCallback, null, _serverOptions.ClearIdleSessionInterval * 1000, _serverOptions.ClearIdleSessionInterval * 1000);
            return Task.CompletedTask;
        }
        private void OnTimerCallback(object state)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            try
            {
                var timeoutTime = DateTimeOffset.Now.AddSeconds(0 - _serverOptions.IdleSessionTimeOut);

                foreach (var s in _sessionContainer.GetSessions())
                {
                    //_logger.LogDebug($"当前连接: {s.Connection.LocalEndPoint}。");
                    if (s.LastActiveTime <= timeoutTime)
                    {
                        try
                        {
                            s.Connection.CloseAsync(CloseReason.TimeOut);
                            _logger.LogWarning($"已关闭空闲会话 {s.SessionID}，其最后活动时间为 {s.LastActiveTime}。");
                        }
                        catch (Exception exc)
                        {
                            _logger.LogError(exc, $"关闭会话 {s.SessionID} 时发生错误，因该会话长时间未活动。");
                        }
                    }
                }
                foreach (var s in _sessionManager.GetAllSessions())
                {
                    if (s.Value.LastActiveTime <= timeoutTime)
                    {
                        try
                        {
                            _sessionManager.TryRemoveBySessionIdAsync(s.Value.SessionID).ConfigureAwait(false);
                        }
                        catch (Exception exc)
                        {
                            _logger.LogError(exc, $"Error SessionManager happened when close the session {s.Value.SessionID} for inactive for a while.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "清除空闲会话时发生错误。");
            }

            _timer.Change(_serverOptions.ClearIdleSessionInterval * 1000, _serverOptions.ClearIdleSessionInterval * 1000);
        }

        /// <summary>
        /// StopAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _timer.Dispose();
            _timer = null;
            return base.StopAsync(cancellationToken);
        }
    }
}
