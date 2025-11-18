namespace YS.Application.IoT.Service.GRPC
{
    using MagicOnion;
    using MagicOnion.Server;
    using Microsoft.Extensions.Logging;
    using YS.Application.IoT.Service.Http;
    using YS.Application.IoT.Service.ReplyCommand;
    using YS.Application.IoT.Wrapper;
    using YS.CoffeeMachine.Iot.Application.DownSend.GRPC;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
    using YS.Domain.IoT.Session;

    /// <summary>
    /// gRPC 命令发送服务实现类。
    /// 提供下行指令下发、设备在线状态查询等基础功能。
    /// </summary>
    public class CommandSender : ServiceBase<ICommandSender>, ICommandSender
    {
        private readonly ILogger<CommandSender> _logger;
        private readonly IReplyCommandService _replyCommandService;
        private readonly IHttp _http;
        private readonly SessionManager<SocketSession> _sessionManager;

        /// <summary>
        /// 初始化一个新的 CommandSender 实例。
        /// </summary>
        /// <param name="logger">日志记录器。</param>
        /// <param name="replyCommandService">用于发送命令的回复服务。</param>
        /// <param name="sessionManager">会话管理器，用于检查设备是否在线。</param>
        /// <param name="http">HTTP 服务，用于健康检查。</param>
        public CommandSender(
            ILogger<CommandSender> logger,
            IReplyCommandService replyCommandService,
            SessionManager<SocketSession> sessionManager,
            IHttp http)
        {
            _logger = logger;
            _replyCommandService = replyCommandService;
            _sessionManager = sessionManager;
            _http = http;
        }

        /// <summary>
        /// 发送下行指令 1009 至指定设备。
        /// </summary>
        /// <param name="request">1009 指令请求数据。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<bool> Downlink1009SendAsync(DownlinkEntity1009 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity1009.KEY.ToString());
        }

        /// <summary>
        /// 发送下行指令 1011 至指定设备。
        /// </summary>
        /// <param name="request">1011 指令请求数据。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<bool> Downlink1011SendAsync(DownlinkEntity1011 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity1011.KEY.ToString());
        }

        /// <summary>
        /// 9026
        /// </summary>
        /// <param name="request">1011 指令请求数据。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<bool> Downlink9026SendAsync(DownlinkEntity9026 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity9026.KEY.ToString());
        }

        /// <summary>
        /// 9028
        /// </summary>
        /// <param name="request">1011 指令请求数据。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<bool> Downlink9028SendAsync(DownlinkEntity9028 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity9028.KEY.ToString());
        }

        /// <summary>
        /// 9029
        /// </summary>
        /// <param name="request">1011 指令请求数据。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<bool> Downlink9029SendAsync(DownlinkEntity9029 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity9029.KEY.ToString());
        }

        /// <summary>
        /// 5212
        /// </summary>
        /// <param name="request">1011 指令请求数据。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<bool> Downlink5212SendAsync(DownlinkEntity5212 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity5212.KEY.ToString());
        }

        /// <summary>
        /// 发送下行指令 1203 至指定设备。
        /// </summary>
        /// <param name="request">1203 指令请求数据。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<bool> Downlink1203SendAsync(DownlinkEntity1203 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity1203.KEY.ToString());
        }

        /// <summary>
        /// 发送下行指令 6200 至指定设备。
        /// </summary>
        /// <param name="request">6200 指令请求数据。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<bool> Downlink6200SendAsync(DownlinkEntity6200 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity6200.KEY.ToString());
        }

        /// <summary>
        /// 发送下行指令 6216 至指定设备。
        /// </summary>
        /// <param name="request">6216 指令请求数据。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<bool> Downlink6216SendAsync(DownlinkEntity6216 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity6216.KEY.ToString());
        }

        /// <summary>
        /// 9021
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async UnaryResult<bool> Downlink9021SendAsync(DownlinkEntity9021 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity9021.KEY.ToString());
        }

        /// <summary>
        /// 执行服务健康检查，测试 HTTP 与 gRPC 是否可用。
        /// </summary>
        /// <param name="content">内容标识，仅支持 "ServiceHealthCheck"。</param>
        /// <returns>包含服务状态的结果对象。</returns>
        public async UnaryResult<CommandSendResult> DownlinkSendAsync(string content)
        {
            if (content == "ServiceHealthCheck")
            {
                var testHttp = await _http.TestAsync();
                bool success = false;
                string messageId = "Ok";

                if (testHttp != null)
                {
                    success = true;
                    Console.WriteLine($"Http连接成功: {testHttp}");
                }
                else
                {
                    messageId = $"Http连接失败";
                }

                var result = await GrpcWrapp.Instance.GrpcCommandService.TestAsync("0000");
                if (result != null)
                {
                    success = true;
                    Console.WriteLine($"Grpc连接成功: {result}");
                }
                else
                {
                    messageId = "Grpc连接失败";
                }

                return new CommandSendResult()
                {
                    Success = success,
                    MessageId = messageId
                };
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 查询指定设备是否在线。
        /// </summary>
        /// <param name="mid">设备唯一标识。</param>
        /// <returns>异步操作结果，指示设备是否在线。</returns>
        public async UnaryResult<bool> IsOnline(string mid)
        {
            var m = await _sessionManager.TryGetAsync(mid);
            Console.WriteLine($"{mid}设备在线状态：{m != null}");
            return m != null;
        }

        /// <summary>
        /// 查询多个设备的在线状态。
        /// </summary>
        /// <param name="mids">设备ID集合。</param>
        /// <returns>异步操作返回设备ID与其在线状态的键值对列表。</returns>
        public async UnaryResult<List<KeyValuePair<string, bool>>> IsOnlines(string[] mids)
        {
            var list = new List<KeyValuePair<string, bool>>();

            foreach (var mid in mids)
            {
                var result = false;

                try
                {
                    result = await _sessionManager.TryGetAsync(mid) != null;
                    Console.WriteLine($"{mid}设备在线状态：{result}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{mid}IsOnlines异常");
                }

                list.Add(new KeyValuePair<string, bool>(mid, result));
            }

            return list;
        }

        /// <summary>
        /// 获取当前所有在线设备及其最后活跃时间。
        /// </summary>
        /// <returns>异步操作返回设备ID与其最后活跃时间的键值对列表。</returns>
        public async UnaryResult<List<KeyValuePair<string, string>>> OnlineVends()
        {
            var enumerators = _sessionManager.GetAllSessions();
            var t = new List<KeyValuePair<string, string>>();

            foreach (var item in enumerators)
            {
                var client = item;
                t.Add(new KeyValuePair<string, string>(
                    client.Key,
                    $"LastSendTime:{item.Value.LastActiveTime.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss")}"));
            }

            return t;
        }

        /// <summary>
        /// 发送下行指令 1100 至指定设备。
        /// </summary>
        /// <param name="request">1100 指令请求数据。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<bool> Downlink1100SendAsync(DownlinkEntity1100 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity1100.KEY.ToString());
        }

        /// <summary>
        /// 发送下行指令 3201 至指定设备。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async UnaryResult<bool> Downlink3201SendAsync(DownlinkEntity3201 request)
        {
            return await _replyCommandService.SendAsync(request, DownlinkEntity3201.KEY.ToString());
        }

        #region 未实现方法

        /// <summary>
        /// 发送下行指令 6002 至指定设备。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public UnaryResult<bool> Downlink6002SendAsync(DownlinkEntity6002 request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送下行指令 10025 至指定设备。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public UnaryResult<bool> Downlink10025SendAsync(DownlinkEntity10025 request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送下行指令 10011 至指定设备。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public UnaryResult<bool> Downlink10011SendAsync(DownlinkEntity10011.Request request)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}