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
    /// 1010
    /// </summary>
    public class UplinkCommand9022 : CommandHandlerBase<UplinkCommand9022>
    {
        /// <summary>
        /// 1010
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand9022(ILogger<UplinkCommand9022> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 9022
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("9022")]
        [EventSubscribe("9022:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var payload = (MessageContext)context.Source;
            var request = payload.Body.FromJson<UplinkEntity9022.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            if (!request.Details.Any())
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }
            await GrpcWrapp.Instance.GrpcCommandService.Uplink9022HandleAsync(request);

            await replyCommandService.SendAsync(new UplinkEntity9022.Response { Mid = request.Mid }, payload);

        }
    }
}