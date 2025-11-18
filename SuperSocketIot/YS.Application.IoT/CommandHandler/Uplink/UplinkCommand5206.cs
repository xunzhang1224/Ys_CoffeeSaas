using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 5205
    /// </summary>
    public class UplinkCommand5206 : CommandHandlerBase<UplinkCommand5206>
    {
        /// <summary>
        /// 5205
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand5206(ILogger<UplinkCommand5206> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 5205
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("5206")]
        [EventSubscribe("5206:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var request = payload.Body.FromJson<UplinkEntity5206.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);

                return;
            }
            if (request.Details == null || request?.Details?.Count() == 0)
            {
                await replyCommandService.NoSendAsync(payload);

                return;
            }
            try
            {
                var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink5206HandleAsync(request);

                await replyCommandService.SendAsync(new BaseCmd { Mid = payload.Mid }, payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"错误:{ex.Message}");
            }
        }
    }
}
