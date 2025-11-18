using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 5205
    /// </summary>
    public class UplinkCommand5205 : CommandHandlerBase<UplinkCommand5205>
    {
        /// <summary>
        /// 5205
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand5205(ILogger<UplinkCommand5205> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 5205
        /// </summary>
        [EventSubscribe("5205")]
        [EventSubscribe("5205:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            var request = payload.Body.FromJson<UplinkEntity5205.Request>();

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
                var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink5205HandleAsync(request);

                result ??= new UplinkEntity5205.Response();

                var response = new UplinkEntity5205.Response
                {
                    CounterNo = request.CounterNo,
                    Mid = request.Mid,
                    MaxSlot = request.MaxSlot,
                    TimeSp = request.TimeSp,
                    TransId = request.TransId,
                    SlotInfo = result.SlotInfo.Any() ? result.SlotInfo.Select(slot => new UplinkEntity5205.Response.SlotEntity { SlotNo = slot.SlotNo }).ToList() : new List<UplinkEntity5205.Response.SlotEntity>()
                };

                await replyCommandService.SendAsync(response, payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"错误:{ex.Message}");
            }
        }
    }
}
