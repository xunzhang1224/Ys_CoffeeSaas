using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 1014
    /// </summary>
    public class UplinkCommand1014 : CommandHandlerBase<UplinkCommand1014>
    {
        /// <summary>
        /// 1014
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand1014(ILogger<UplinkCommand1014> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 1014
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("1014")]
        [EventSubscribe("1014:V1")]
        public async override Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            var request = payload.Body.FromJson<UplinkEntity1014.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink1014HandleAsync(request);

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            await replyCommandService.SendAsync(
                new UplinkEntity1014.Response
            {
                Mid = request.Mid,
                TransId = request.TransId,
                CapabilityType = request.CapabilityType,
                CapabilityConfigure = result.CapabilityConfigure.Select(i => new UplinkEntity1014.Response.ConfigureEntity
                {
                    Id = i.Id,
                    Name = i.Name,
                    Content = i.Content,
                    Premission = i.Premission,
                    Structure = i.Structure,
                }).ToList()
            }, payload);
        }
    }
}
