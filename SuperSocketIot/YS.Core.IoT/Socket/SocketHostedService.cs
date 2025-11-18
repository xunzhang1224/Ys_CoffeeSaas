using Jaina;
using Masuit.Tools;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Host;
using YS.Application.IoT.Manager;
using YS.Application.IoT.Service.Http;
using YS.Application.IoT.Service.Http.Dto;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.Domain.IoT.Option;
using YS.Domain.IoT.Receive;
using YS.Domain.IoT.Session;

namespace YS.Core.IoT.Socket;

/// <summary>
/// SocketHostedService
/// </summary>
public class SocketHostedService : IHostedService
{
    private readonly ILogger<SocketHostedService> _logger;

    /// <summary>
    /// host
    /// </summary>
    public IHost host { get; }
    private readonly IEventPublisher _eventPublisher;
    private readonly ISuperSessionContainer _sessionContainer;
    private readonly SuperServerOptions _serverOptions;
    private readonly IBlacklistManager _blacklistManager;
    private readonly IHttp _http;
    private readonly SessionManager<SocketSession> _sessionManager;

    /// <summary>
    /// SocketHostedService
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventPublisher"></param>
    /// <param name="sessionContainer"></param>
    /// <param name="serverOptions"></param>
    /// <param name="sessionManager"></param>
    /// <param name="blacklistManager"></param>
    /// <param name="http"></param>
    public SocketHostedService(
        ILogger<SocketHostedService> logger,
        IEventPublisher eventPublisher,
        ISuperSessionContainer sessionContainer,
        IOptions<SuperServerOptions> serverOptions,
        SessionManager<SocketSession> sessionManager,
        IBlacklistManager blacklistManager,
        IHttp http)
    {
        _logger = logger;
        _eventPublisher = eventPublisher;
        _sessionContainer = sessionContainer;
        _http = http;
        _serverOptions = serverOptions.Value;
        _sessionManager = sessionManager;
        _blacklistManager = blacklistManager;

        Console.WriteLine($"serverOptions：{serverOptions.ToJsonString()}");
        host = SuperSocketHostBuilder
                .Create<MessageContext, IotCommandPackageFilter>()
                 .ConfigureSuperSocket(options =>
                 {
                     options.Name = _serverOptions.Name;
                     options.IdleSessionTimeOut = _serverOptions.IdleSessionTimeOut;
                     options.ClearIdleSessionInterval = _serverOptions.ClearIdleSessionInterval;
                     options.PackageHandlingTimeOut = _serverOptions.PackageHandlingTimeOut;
                     foreach (var listener in _serverOptions.Listeners)
                     {
                         options.AddListener(new ListenOptions()
                         {
                             Ip = listener.Ip,
                             Port = listener.Port
                         });
                     }
                 })
                .UseSession<SocketSession>()
                //.UseClearIdleSession()
                .UseSessionHandler(
                onConnected: async (session) =>
                {
                    _logger.LogInformation("建立连接 {SessionID} 远程地址:{RemoteEndPoint} 本地地址:{LocalEndPoint} ", session.SessionID, session.RemoteEndPoint, session.LocalEndPoint);

                    await _sessionContainer.RegisterSession(session);
                },
                onClosed: async (session, closeArgs) =>
                {
                    try
                    {
                        // Session管理
                        _logger.LogInformation("连接断开 {SessionID} 远程地址:{RemoteEndPoint},最后活跃时间：{LastActiveTime},断开原因：{Reason} ", session.SessionID, session.RemoteEndPoint,
                            session.LastActiveTime, closeArgs.Reason);

                        await _sessionContainer.UnRegisterSession(session);
                        if (session != null && session is SocketSession)
                        {
                            var socketSession = session as SocketSession;
                            var mid = socketSession?.Mid;
                            if (mid == null) return;
                            //if (closeArgs.Reason != SuperSocket.Connection.CloseReason.Unknown && mid != CacheConst.MidDead)
                            //{
                            //    //设备离线
                            //    await _http.SetVendLineStatus(new VendLineStatusInput()
                            //    {
                            //        LineStatus = false,
                            //        VendCode = mid,
                            //    });
                            //}
                            await _sessionManager.TryRemoveAsync(mid);
                        }
                    }
                    catch
                    {
                    }
                })
                .UsePackageHandler(async (session, package) =>
                {
                    _logger.LogInformation("UsePackageHandler - {Key} - {Mid}", package.Key, package.Mid);
                    if (session != null && session is SocketSession socket)
                    {
                        if (socket.IsTmp)
                        {
                            await session.CloseAsync(SuperSocket.Connection.CloseReason.LocalClosing);
                            return;
                        }
                        if (package.Key != "1000" && package.Key != "1111" && !socket.IsLogin)
                        {
                            await session.CloseAsync(SuperSocket.Connection.CloseReason.ApplicationError);
                            return;
                        }
                        //bool isInblack =await _blacklistManager.IsInBlacklist(package.Mid, BlacklistTypeEnum.Device);
                        //if (isInblack)
                        //{
                        //    await session.CloseAsync(SuperSocket.Connection.CloseReason.RemoteClosing);

                        //    _logger.LogInformation("黑名单拦截Device 远程地址:{RemoteEndPoint} 设备号:{Mid} ", session.RemoteEndPoint, package.Mid);

                        //    return;
                        //}
                    }

                    if (package != null)
                    {
                        package.SetSessionId(session.SessionID);
                        // 处理消息
                        await _eventPublisher.PublishAsync(package);
                    }
                })
                //.UseInProcSessionContainer()
                .ConfigureLogging((hostCtx, loggingBuilder) =>
                {
                    _logger.LogInformation("ConfigureLogging");
                    loggingBuilder.AddConsole();
                })
                .Build();
    }

    /// <summary>
    /// StartAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Socket 后台任务启动");
        await host.StartAsync(cancellationToken);
    }

    /// <summary>
    /// StopAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Socket 后台任务停止");
        return host.StopAsync(cancellationToken);
    }

    /// <summary>
    /// GetServices
    /// </summary>
    /// <returns></returns>
    public IServiceProvider GetServices()
    {
        return host.Services;
    }
}
