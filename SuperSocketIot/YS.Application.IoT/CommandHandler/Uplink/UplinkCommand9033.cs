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
    /// 9033冲洗部件使用
    /// </summary>
    public class UplinkCommand9033 : CommandHandlerBase<UplinkCommand9033>
    {
        /// <summary>
        /// 1010
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand9033(ILogger<UplinkCommand9033> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 9033冲洗部件使用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("9033")]
        [EventSubscribe("9033:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var payload = (MessageContext)context.Source;
            var request = payload.Body.FromJson<UplinkEntity9033.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }
            var rsp = await GrpcWrapp.Instance.GrpcCommandService.Uplink9033HandleAsync(request);

            await replyCommandService.SendAsync(rsp, payload);

        }
    }
}