using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 8002:无发送
    /// </summary>
    public class UplinkCommand8002 : CommandHandlerBase<UplinkCommand8002>
    {
        /// <summary>
        /// 8002:无发送
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand8002(ILogger<UplinkCommand8002> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 8002:无发送
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("8002")]
        [EventSubscribe("8002:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            await replyCommandService.NoSendAsync(payload);
        }
    }
}
