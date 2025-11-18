using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 1014
    /// </summary>
    public class UplinkCommand1204 : CommandHandlerBase<UplinkCommand1204>
    {
        /// <summary>
        /// 1204
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand1204(ILogger<UplinkCommand1204> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 1204
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("1204")]
        [EventSubscribe("1204:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            var request = payload.Body.FromJson<UplinkEntity1204.Request>();

            if (request == null)
            {
                await replyCommandService.NoSendAsync(payload);

                return;
            }

            if (!request.Releases.Any())
            {
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink1204HandleAsync(request);

            var response = new UplinkEntity1204.Response
            {
                Mid = request.Mid,
                Releases = Array.Empty<UplinkEntity1204.Response.Release>()
            };

            if (result != null && result.Releases != null)
            {
                response.Releases = result.Releases.Select(i => new UplinkEntity1204.Response.Release
                {
                    Type = i.Type,
                    Name = i.Name,
                    VersionName = i.VersionName,
                    VersionType = i.VersionType,
                    Description = i.Description,
                    DownLoadUrl = i.DownLoadUrl
                });
            }
            await replyCommandService.SendAsync(response, payload);
        }
    }
}
