using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 废弃的 为了事件不警告 只做接收
    /// </summary>
    public class UplinkCommand2009 : CommandHandlerBase<UplinkCommand2009>
    {
        /// <summary>
        /// 2009
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand2009(ILogger<UplinkCommand2009> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 2009
        /// </summary>
        [EventSubscribe("2009")]
        [EventSubscribe("2009:V1")]
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
                _logger.LogError(ex, $"处理2009出错：设备{payload.Mid}:{ex.Message}");
            }
        }
    }
}
