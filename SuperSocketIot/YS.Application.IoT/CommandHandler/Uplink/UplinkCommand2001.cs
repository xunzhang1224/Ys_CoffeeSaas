using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.Http;
using YS.Application.IoT.Service.Http.Dto;
using YS.Application.IoT.Service.ReplyCommand;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 2001
    /// </summary>
    public class UplinkCommand2001 : CommandHandlerBase<UplinkCommand2001>
    {
        /// <summary>
        /// 2001
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand2001(ILogger<UplinkCommand2001> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 2001
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("2001")]
        [EventSubscribe("2001:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var http = _serviceScope.ServiceProvider.GetRequiredService<IHttp>();

            var request = payload.Body.FromJson<UplinkEntity2001.Request>();

            //????统一拿发送的时间戳回？
            if (payload.Mid == request?.Mid && !string.IsNullOrEmpty(payload.Mid))
            {
                _ = await http.SetVendLineStatus(new VendLineStatusInput()
                {
                    LineStatus = true,
                    VendCode = payload.Mid,
                });
                var reply = new BaseCmd { Mid = payload.Mid, TimeSp = request.TimeSp };
                await replyCommandService.SendAsync(request, payload);
            }
            return;
        }
    }
}
