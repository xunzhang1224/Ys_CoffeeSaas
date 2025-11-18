namespace YS.Application.IoT.CommandHandler.Uplink
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Jaina;
    using YS.Application.IoT.Service.ReplyCommand;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities;
    using YS.Application.IoT.CommandHandler;

    /// <summary>
    /// 上行指令 1001 的处理程序 —— 设备对时功能。
    /// 负责接收设备发起的时间同步请求，并返回服务器当前时间戳。
    /// </summary>
    public class UplinkCommand1001 : CommandHandlerBase<UplinkCommand1001>
    {
        /// <summary>
        /// 初始化一个新的 UplinkCommand1001 实例。
        /// </summary>
        /// <param name="logger">日志记录器。</param>
        /// <param name="scopeFactory">服务作用域工厂，用于解析依赖项。</param>
        public UplinkCommand1001(ILogger<UplinkCommand1001> logger, IServiceScopeFactory scopeFactory)
            : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 处理事件标识为 "1001" 和 "1001:V1" 的上行指令（设备对时请求）。
        /// </summary>
        /// <param name="context">事件执行上下文，包含消息体等信息。</param>
        /// <returns>异步操作任务。</returns>
        [EventSubscribe("1001")]
        [EventSubscribe("1001:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            // 解析请求数据
            var req = payload.Body.FromJson<UplinkEntity1001.Request>();
            if (req != null)
            {
                // 构造响应对象，返回客户端时间戳 + 当前服务器时间戳
                UplinkEntity1001.Response response = new UplinkEntity1001.Response()
                {
                    TimeSp = req.TimeSp,
                    TimeSpValue = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    Mid = payload.Mid
                };

                // 发送响应给设备
                await replyCommandService.SendAsync(response, payload);
                return;
            }

            // 若解析失败，发送空响应
            await replyCommandService.NoSendAsync(payload);
        }
    }
}