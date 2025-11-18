using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 1010
    /// </summary>
    public class UplinkCommand1010 : CommandHandlerBase<UplinkCommand1010>
    {
        /// <summary>
        /// 1010
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand1010(ILogger<UplinkCommand1010> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 1010
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("1010")]
        [EventSubscribe("1010:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var payload = (MessageContext)context.Source;
            var request = payload.Body.FromJson<UplinkEntity1010.Request>();
            try
            {
                if (request == null)
                {
                    await replyCommandService.NoSendAsync(payload);
                    return;
                }

                if (!request.CapabilityConfigure.Any())
                {
                    await replyCommandService.NoSendAsync(payload);
                    return;
                }
                var data = request.CapabilityConfigure.Select(i => new UplinkEntity1010.Request.ConfigureEntity
                {
                    Id = i.Id,
                    Content = i.Content,
                    Name = i.Name,
                    Premission = i.Premission,
                    Structure = i.Structure
                }).ToList();
                var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink1010HandleAsync(new UplinkEntity1010.Request()
                {
                    CapabilityType = request.CapabilityType,
                    CapabilityConfigure = data,
                    Mid = request.Mid,
                });

                var response = new UplinkEntity1010.Response { Mid = request.Mid };
                if (result == null || result.CapabilityConfigure == null || result.CapabilityConfigure?.Any() == false)
                {
                    response.CapabilityType = request.CapabilityType;
                    response.CapabilityConfigure = new List<UplinkEntity1010.Response.ConfigureEntity>();
                }
                else
                {
                    response.CapabilityType = result.CapabilityType;
                    response.CapabilityConfigure = result.CapabilityConfigure.Select(i => new UplinkEntity1010.Response.ConfigureEntity { Id = i.Id });
                }
                await replyCommandService.SendAsync(response, payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UplinkCommand1010发生错误:{ex.Message}");
                await replyCommandService.NoSendAsync(payload);

            }

        }
    }
}
