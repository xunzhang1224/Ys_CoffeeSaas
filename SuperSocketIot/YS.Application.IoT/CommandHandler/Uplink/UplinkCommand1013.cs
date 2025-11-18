using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 1012
    /// </summary>
    public class UplinkCommand1013 : CommandHandlerBase<UplinkCommand1013>
    {
        /// <summary>
        /// 1013
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand1013(ILogger<UplinkCommand1013> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 1013
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("1013")]
        [EventSubscribe("1013:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            var request = payload.Body.FromJson<UplinkEntity1013.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            if (!request.Attributes.Any())
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink1013HandleAsync(request);

            if (result)
            {
                var response = new UplinkEntity1013.Response { Mid = request.Mid };

                await replyCommandService.SendAsync(response, payload);
            }
        }
    }
}
