using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 5207
    /// </summary>
    public class UplinkCommand5207 : CommandHandlerBase<UplinkCommand5207>
    {
        /// <summary>
        /// 5207
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand5207(ILogger<UplinkCommand5207> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 5207
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("5207")]
        [EventSubscribe("5207:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var request = payload.Body.FromJson<UplinkEntity5207.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);

                return;
            }
            if (request.SlotInfo == null || request.SlotInfo.Count == 0)
            {
                await replyCommandService.NoSendAsync(payload);

                return;
            }
            try
            {
                var response = await GrpcWrapp.Instance.GrpcCommandService.Uplink5207HandleAsync(request);

                response ??= new UplinkEntity5207.Response()
                {
                    Mid = request.Mid,
                    TransId = request.TransId,
                };

                await replyCommandService.SendAsync(response, payload);
            }
            catch (Exception ex)
            {
                _logger.LogError($"错误:{ex.Message}");
            }
        }
    }
}
