using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 4201:V1
    /// </summary>
    public class UplinkCommand5204 : CommandHandlerBase<UplinkCommand5204>
    {
        /// <summary>
        /// 5204:V1
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand5204(ILogger<UplinkCommand5204> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 5204:V1
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("5204")]
        [EventSubscribe("5204:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            var request = payload.Body.FromJson<UplinkEntity5204.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            if (!request.Info.Any())
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink5204HandleAsync(request);

            if (result)
                await replyCommandService.SendAsync(new UplinkEntity5204.Response { Mid = request.Mid }, payload);
        }
    }
}
