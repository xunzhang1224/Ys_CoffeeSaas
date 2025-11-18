using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.ReplyCommand;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 1204: 获取设备升级包列表
    /// </summary>
    public class UplinkCommand1205 : CommandHandlerBase<UplinkCommand1205>
    {
        /// <summary>
        ///  1204: 获取设备升级包列表
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand1205(ILogger<UplinkCommand1205> logger, IServiceScopeFactory scopeFactory)
             : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 1204: 获取设备升级包列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("1205")]
        [EventSubscribe("1205:V1")]
        public async override Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            var request = payload.Body.FromJson<UplinkEntity1205.Request>();

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
            var result = await GrpcWrapp.Instance.GrpcCommandService.Uplink1205HandleAsync(request);

            await replyCommandService.SendAsync(new UplinkEntity1205.Response
            {
                Mid = request.Mid,
                TransId = request.TransId
            }, payload);
        }
    }
}
