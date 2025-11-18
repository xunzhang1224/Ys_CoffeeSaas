using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 7200
    /// </summary>
    public class UplinkCommand7200 : CommandHandlerBase<UplinkCommand7200>
    {
        /// <summary>
        /// 7200
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand7200(ILogger<UplinkCommand7200> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 7200
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("7200")]
        [EventSubscribe("7200:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var request = payload.Body.FromJson<UplinkEntity7200.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink7200HandleAsync(request);

            var response = new UplinkEntity7200.Response
            {
                Mid = request.Mid,
                TransId = request.TransId,
            };

            if (result != null)
            {
                response.Status = result.Status;
                response.Description = result.Description;
                response.Result = result.Result;
            }
            await replyCommandService.SendAsync(response, payload);

        }
    }
}
