using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 7202
    /// </summary>
    public class UplinkCommand7202 : CommandHandlerBase<UplinkCommand7202>
    {
        /// <summary>
        /// 7202
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand7202(ILogger<UplinkCommand7202> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 7202
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("7202")]
        [EventSubscribe("7202:V1")]
        public async override Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var request = payload.Body.FromJson<UplinkEntity7202.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            var response = await GrpcWrapp.Instance.GrpcCommandService.Uplink7202HandleAsync(request);

            await replyCommandService.SendAsync(response, payload);

        }
    }
}
