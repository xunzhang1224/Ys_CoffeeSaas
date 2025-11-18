using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 7202
    /// </summary>
    public class UplinkCommand7211 : CommandHandlerBase<UplinkCommand7211>
    {
        /// <summary>
        /// 7202
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand7211(ILogger<UplinkCommand7211> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 7202
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("7211")]
        [EventSubscribe("7211:V1")]
        public async override Task HandleAsync(EventHandlerExecutingContext context)
        {
            try
            {
                var payload = (MessageContext)context.Source;
                var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
                var request = payload.Body.FromJson<UplinkEntity7211.Request>();

                if (request == null)
                {
                    await replyCommandService.NoSendAsync(payload);
                    return;
                }

                var response = new UplinkEntity7211.Response { Mid = request.Mid, TransId = request.TransId, Status = 1, };

                var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink7211HandleAsync(request);

                if (result == null)
                {
                    response.Status = 1;
                    response.Description = "服务调用失败";
                    response.Amount = 0;
                }
                else
                {
                    response = new UplinkEntity7211.Response
                    {
                        Mid = request.Mid,
                        TransId = request.TransId,
                        Amount = result.Amount,
                        OrderNo = result.OrderNo,
                        Result = result.Result,
                        Status = result.Status,
                        Description = result.Description,
                    };
                }

                await replyCommandService.SendAsync(response, payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"订单出错:{ex.Message}");
            }

        }
    }
}
