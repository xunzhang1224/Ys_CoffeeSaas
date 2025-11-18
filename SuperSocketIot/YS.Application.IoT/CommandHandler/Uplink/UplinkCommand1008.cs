using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 1008
    /// </summary>
    public class UplinkCommand1008 : CommandHandlerBase<UplinkCommand1008>
    {
        /// <summary>
        /// 1008
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand1008(ILogger<UplinkCommand1008> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 1008
        /// </summary>
        [EventSubscribe("1008")]
        [EventSubscribe("1008:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            var req = payload.Body.FromJson<UplinkEntity1008>();

            if (req == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            if (await GrpcWrapp.Instance.GrpcCommandService.Uplink1008HandleAsync(req))
            {
                await replyCommandService.SendAsync(new UplinkEntity1008.Response { Mid = req.Mid }, payload);
            }
        }
    }
}
