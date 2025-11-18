using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 4201
    /// </summary>
    public class UplinkCommand4201 : CommandHandlerBase<UplinkCommand4201>
    {
        /// <summary>
        /// 4201
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand4201(ILogger<UplinkCommand4201> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 4201
        /// </summary>
        [EventSubscribe("4201")]
        [EventSubscribe("4201:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            var request = payload.Body.FromJson<UplinkEntity4201.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            if (!request.Orders.Any())
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }
            try
            {
                var result = await GrpcWrapp.Instance.GrpcCommandService.WithDeadline(DateTime.UtcNow.AddSeconds(30)).Uplink4201HandleAsync(request);

                if (result == null)
                {
                    await replyCommandService.NoSendAsync(payload);
                    return;
                }

                if (string.IsNullOrEmpty(result.OrderNo))
                {
                    await replyCommandService.NoSendAsync(payload);
                    return;
                }

                var response = new UplinkEntity4201.Response
                {
                    Mid = request.Mid,
                    OrderNo = request.OrderNo,
                    TransId = request.TransId,
                };
                await replyCommandService.SendAsync(response, payload);
            }
            catch (Exception ex)
            {
                _logger.LogError($"应答指令{UplinkEntity4201.KEY}，机器{request.Mid}：回复异常:{ex.ToString()}");
            }
        }
    }
}
