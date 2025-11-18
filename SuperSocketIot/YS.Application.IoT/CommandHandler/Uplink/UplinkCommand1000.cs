namespace YS.Application.IoT.CommandHandler.Uplink
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Jaina;
    using YS.Application.IoT.Service.ReplyCommand;
    using YS.Application.IoT.Wrapper;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities;
    using YS.Application.IoT.CommandHandler;

    /// <summary>
    /// 上行指令 1000 的处理程序。
    /// 负责接收IoT设备上报的请求，并通过 gRPC 调用业务逻辑进行处理，然后返回加密响应。
    /// </summary>
    public class UplinkCommand1000 : CommandHandlerBase<UplinkCommand1000>
    {
        /// <summary>
        /// 初始化一个新的 UplinkCommand1000 实例。
        /// </summary>
        /// <param name="logger">日志记录器。</param>
        /// <param name="scopeFactory">服务作用域工厂，用于解析依赖项。</param>
        public UplinkCommand1000(ILogger<UplinkCommand1000> logger, IServiceScopeFactory scopeFactory)
            : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 处理事件标识为 "1000" 和 "1000:V1" 的上行指令。
        /// </summary>
        /// <param name="context">事件执行上下文，包含消息体等信息。</param>
        /// <returns>异步操作任务。</returns>
        [EventSubscribe("1000")]
        [EventSubscribe("1000:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var request = (MessageContext)context.Source;

            // 解析上行请求数据
            var req = request.Body.FromJson<UplinkEntity1000.Request>();

            // 调用 gRPC 服务处理上行指令 1000
            var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink1000HandleAsync(req);

            // 获取回复服务并发送加密响应
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            await replyCommandService.SendAsync(result, request, req.PubKey);
        }
    }
}