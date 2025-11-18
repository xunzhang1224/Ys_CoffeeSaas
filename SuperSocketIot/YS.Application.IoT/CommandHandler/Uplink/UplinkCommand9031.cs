using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 9031冲洗部件上报
    /// </summary>
    public class UplinkCommand9031 : CommandHandlerBase<UplinkCommand9031>
    {
        /// <summary>
        /// 1010
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand9031(ILogger<UplinkCommand9031> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 9031冲洗部件上报
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("9031")]
        [EventSubscribe("9031:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var payload = (MessageContext)context.Source;
            var request = payload.Body.FromJson<UplinkEntity9031.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }
            var rsp = await GrpcWrapp.Instance.GrpcCommandService.Uplink9031HandleAsync(request);

            await replyCommandService.SendAsync(rsp, payload);

        }
    }
}