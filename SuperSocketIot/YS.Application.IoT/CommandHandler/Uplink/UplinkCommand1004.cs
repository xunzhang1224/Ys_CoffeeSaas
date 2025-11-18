namespace YS.Application.IoT.CommandHandler.Uplink
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Jaina;
    using YS.Application.IoT.Service.ReplyCommand;
    using YS.Application.IoT.CommandHandler;

    /// <summary>
    /// 上行指令 1004 的处理程序。
    /// 当前为废弃状态，仅用于接收事件以避免系统警告。
    /// </summary>
    public class UplinkCommand1004 : CommandHandlerBase<UplinkCommand1004>
    {
        /// <summary>
        /// 初始化一个新的 UplinkCommand1004 实例。
        /// </summary>
        /// <param name="logger">日志记录器。</param>
        /// <param name="scopeFactory">服务作用域工厂，用于解析依赖项。</param>
        public UplinkCommand1004(ILogger<UplinkCommand1004> logger, IServiceScopeFactory scopeFactory)
            : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 处理事件标识为 "1004" 和 "1004:V1" 的上行指令。
        /// 当前实现仅为接收事件，不进行实际业务处理。
        /// </summary>
        /// <param name="context">事件执行上下文，包含消息体等信息。</param>
        /// <returns>异步操作任务。</returns>
        [EventSubscribe("1004")]
        [EventSubscribe("1004:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;

            try
            {
                var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
                await replyCommandService.NoSendAsync(payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理1004出错：设备{payload.Mid}|:{ex.Message}");
            }
        }
    }
}