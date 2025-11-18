using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 1210
    /// </summary>
    public class UplinkCommand1210 : CommandHandlerBase<UplinkCommand1210>
    {
        /// <summary>
        /// 1210
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand1210(ILogger<UplinkCommand1210> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 1210
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("1210")]
        [EventSubscribe("1210:V1")]
        public async override Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            var request = payload.Body.FromJson<UplinkEntity1210.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            var response = await GrpcWrapp.Instance.GrpcCommandService.Uplink1210HandleAsync(request);

            response ??= new UplinkEntity1210.Response
            {
                Mid = request.Mid,
                Token = string.Empty
            };
            await replyCommandService.SendAsync(response, payload);
        }
    }
}
