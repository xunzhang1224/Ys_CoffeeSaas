namespace YS.Application.IoT.CommandHandler.Downlink
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Jaina;
    using YS.Application.IoT.Service.ReplyCommand;
    using YS.Application.IoT.Wrapper;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
    using YS.Application.IoT.CommandHandler;

    /// <summary>
    /// 下行指令 1011 的处理程序。
    /// 用于接收并处理来自IoT设备的配置请求，并通过 gRPC 调用业务逻辑进行处理。
    /// </summary>
    public class DownlinkCommand1011 : CommandHandlerBase<DownlinkCommand1011>
    {
        /// <summary>
        /// 初始化一个新的 DownlinkCommand1011 实例。
        /// </summary>
        /// <param name="logger">日志记录器。</param>
        /// <param name="scopeFactory">服务作用域工厂，用于解析依赖项。</param>
        public DownlinkCommand1011(ILogger<DownlinkCommand1011> logger, IServiceScopeFactory scopeFactory)
            : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 处理事件标识为 "1011" 和 "1011:V1" 的下行指令。
        /// </summary>
        /// <param name="context">事件执行上下文，包含消息体等信息。</param>
        /// <returns>异步操作任务。</returns>
        [EventSubscribe("1011")]
        [EventSubscribe("1011:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            // 解析请求数据
            var request = payload.Body.FromJson<DownlinkEntity1011.Response>();
            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            // 调用 gRPC 服务处理下行指令
            await GrpcWrapp.Instance.GrpcCommandService.Downlink1011Handle(request);

            // 发送空响应（无需实际返回内容）
            await replyCommandService.NoSendAsync(payload);
        }
    }
}