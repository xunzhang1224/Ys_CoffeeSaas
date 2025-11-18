using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 5209
    /// </summary>
    public class UplinkCommand5213 : CommandHandlerBase<UplinkCommand5213>
    {
        /// <summary>
        /// 5213
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand5213(ILogger<UplinkCommand5213> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 5213
        /// </summary>
        [EventSubscribe("5213")]
        [EventSubscribe("5213:V1")]
        public async override Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var request = payload.Body.FromJson<UplinkEntity5213.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);

                return;
            }
            var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink5213HandleAsync(request);

            if (result == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }
            await replyCommandService.SendAsync(result, payload);
        }
    }
}
