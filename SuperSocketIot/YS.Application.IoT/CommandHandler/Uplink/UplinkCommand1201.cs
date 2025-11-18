using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 设备对时
    /// </summary>
    public class UplinkCommand1201 : CommandHandlerBase<UplinkCommand1201>
    {
        /// <summary>
        /// 设备对时
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand1201(ILogger<UplinkCommand1201> logger, IServiceScopeFactory scopeFactory)
         : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 设备对时
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("1201")]
        [EventSubscribe("1201:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {

            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var req = payload.Body.FromJson<BaseCmd>();
            if (req != null)
            {
                UplinkEntity1201.Response response = new UplinkEntity1201.Response()
                {
                    TimeSp = req.TimeSp,
                    ServerTimeSp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Mid = payload.Mid,
                };
                await replyCommandService.SendAsync(response, payload);
                return;
            }
            await replyCommandService.NoSendAsync(payload);
        }
    }
}
