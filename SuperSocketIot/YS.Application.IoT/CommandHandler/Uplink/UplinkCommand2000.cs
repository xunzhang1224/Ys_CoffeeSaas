using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 1210
    /// </summary>
    public class UplinkCommand2000 : CommandHandlerBase<UplinkCommand2000>
    {
        /// <summary>
        /// 1210
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand2000(ILogger<UplinkCommand2000> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 1210
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("2000")]
        [EventSubscribe("2000:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;

            try
            {
                var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

                var request = payload.Body.FromJson<UplinkEntity2000.Request>();

                //????统一拿发送的时间戳回？
                if (payload.Mid == request?.Mid && !string.IsNullOrEmpty(payload.Mid))
                {
                    // TODO:更新设备信息lld
                    var am = new UplinkEntity2000.Response
                    {
                        Mid = request.Mid,
                        TimeSp = request.TimeSp
                    };
                    //_ = await GrpcWrapp.Instance.GrpcCommandService.Uplink2000HandleAsync(request);
                    var reply = new BaseCmd { Mid = payload.Mid };
                    await replyCommandService.SendAsync(reply, payload);
                }
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理心跳出错：设备{payload.Mid}:{ex.Message}");
            }

        }
    }
}
