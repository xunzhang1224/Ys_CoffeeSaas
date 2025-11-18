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
    /// 下行指令 6002 的处理程序。
    /// 负责接收IoT设备上报的特定类型请求，并通过 gRPC 调用执行业务逻辑处理。
    /// </summary>
    public class DownlinkCommand6002 : CommandHandlerBase<DownlinkCommand6002>
    {
        /// <summary>
        /// 初始化一个新的 DownlinkCommand6002 实例。
        /// </summary>
        /// <param name="logger">日志记录器。</param>
        /// <param name="scopeFactory">服务作用域工厂，用于解析依赖项。</param>
        public DownlinkCommand6002(ILogger<DownlinkCommand6002> logger, IServiceScopeFactory scopeFactory)
            : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 处理事件标识为 "6002" 和 "6002:V1" 的下行指令。
        /// </summary>
        /// <param name="context">事件执行上下文，包含消息体等信息。</param>
        /// <returns>异步操作任务。</returns>
        [EventSubscribe("6002")]
        [EventSubscribe("6002:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            // 解析下行请求数据
            var request = payload.Body.FromJson<DownlinkEntity6002.Response>();
            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            // 调用 gRPC 服务处理下行指令 6002
            var result = await GrpcWrapp.Instance.GrpcCommandService.Downlink6002HandleAsync(request);

            // 发送空响应，表示已成功接收并处理
            await replyCommandService.NoSendAsync(payload);
        }
    }
}