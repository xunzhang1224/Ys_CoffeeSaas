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
    /// 下行指令 1203 的处理程序。
    /// 用于接收并处理IoT设备的特定下行请求，并通过 gRPC 调用执行相关业务逻辑。
    /// </summary>
    public class DownlinkCommand1203 : CommandHandlerBase<DownlinkCommand1203>
    {
        /// <summary>
        /// 初始化一个新的 DownlinkCommand1203 实例。
        /// </summary>
        /// <param name="logger">日志记录器。</param>
        /// <param name="scopeFactory">服务作用域工厂，用于解析依赖项。</param>
        public DownlinkCommand1203(ILogger<DownlinkCommand1203> logger, IServiceScopeFactory scopeFactory)
            : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 处理事件标识为 "1203" 和 "1203:V1" 的下行指令。
        /// </summary>
        /// <param name="context">事件执行上下文，包含消息体等信息。</param>
        /// <returns>异步操作任务。</returns>
        [EventSubscribe("1203")]
        [EventSubscribe("1203:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            // 解析请求数据
            var request = payload.Body.FromJson<DownlinkEntity1203.Response>();
            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            // 调用 gRPC 服务处理下行指令 1203
            await GrpcWrapp.Instance.GrpcCommandService.Downlink1203HandleAsync(request);

            // 发送空响应（无需实际返回内容）
            await replyCommandService.NoSendAsync(payload);
        }
    }
}