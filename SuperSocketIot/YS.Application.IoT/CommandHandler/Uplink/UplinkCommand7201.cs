using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 7200
    /// </summary>
    public class UplinkCommand7201 : CommandHandlerBase<UplinkCommand7201>
    {
        /// <summary>
        /// 7201
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand7201(ILogger<UplinkCommand7201> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 7201
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("7201")]
        [EventSubscribe("7201:V1")]
        public async override Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var request = payload.Body.FromJson<UplinkEntity7201.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink7201HandleAsync(request);
            await replyCommandService.SendAsync(result, payload);
        }
    }
}
