using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 5209
    /// </summary>
    public class UplinkCommand5209 : CommandHandlerBase<UplinkCommand5209>
    {
        /// <summary>
        /// 5209
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand5209(ILogger<UplinkCommand5209> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 5209
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("5209")]
        [EventSubscribe("5209:V1")]
        public async override Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var request = payload.Body.FromJson<UplinkEntity5209.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink5209HandleAsync(request);
            if (result == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }
            await replyCommandService.SendAsync(result, payload);

        }
    }

}
