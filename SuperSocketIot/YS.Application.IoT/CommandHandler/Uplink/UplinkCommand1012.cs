using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 1012
    /// </summary>
    public class UplinkCommand1012 : CommandHandlerBase<UplinkCommand1012>
    {
        /// <summary>
        /// 1012
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand1012(ILogger<UplinkCommand1012> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 1012
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("1012")]
        [EventSubscribe("1012:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            var request = payload.Body.FromJson<UplinkEntity1012.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            if (!request.Metrics.Any())
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }
            await GrpcWrapp.Instance.GrpcCommandService.Uplink1012HandleAsync(request);

            await replyCommandService.SendAsync(new UplinkEntity1012.Response { Mid = request.Mid }, payload);
        }
    }
}
