using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 外部订单上报
    /// </summary>
    public class UplinkCommand7212 : CommandHandlerBase<UplinkCommand7212>
    {
        /// <summary>
        /// 外部订单上报
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand7212(ILogger<UplinkCommand7212> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 外部订单上报
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("7212")]
        [EventSubscribe("7212:V1")]
        public async override Task HandleAsync(EventHandlerExecutingContext context)
        {
            try
            {
                var payload = (MessageContext)context.Source;
                var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
                var request = payload.Body.FromJson<UplinkEntity7212.Request>();

                if (request == null)
                {
                    await replyCommandService.NoSendAsync(payload);
                    return;
                }

                var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink7212HandleAsync(request);

                await replyCommandService.SendAsync(result, payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"订单出错:{ex.Message}");
            }
        }
    }
}
