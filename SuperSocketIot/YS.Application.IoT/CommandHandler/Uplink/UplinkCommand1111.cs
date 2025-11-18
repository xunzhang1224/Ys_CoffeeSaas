

using Nacos;
using YS.Application.IoT.CommandHandler;
using YS.Application.IoT.Service.Http;
using YS.Application.IoT.Service.Http.Dto;
using YS.Application.IoT.Service.ReplyCommand;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.Domain.IoT.Util;

namespace YS.Application.IoT.CommandHandler.Uplink
{
    /// <summary>
    /// 登录
    /// </summary>
    public class UplinkCommand1111 : CommandHandlerBase<UplinkCommand1111>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public UplinkCommand1111(ILogger<UplinkCommand1111> logger, IServiceScopeFactory scopeFactory)
              : base(logger, scopeFactory)
        {

        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("1111")]
        [EventSubscribe("1111:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var request = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();
            var http = _serviceScope.ServiceProvider.GetRequiredService<IHttp>();
            //获取服务器密钥
            var rs = await http.GetPrivKey(request.Mid);
            if (rs != null)
            {
                //var res = await ProjectUtil.StoreDeviceInfoAsync(request.Mid);
                //if (res == false) await replyCommandService.CloseAsync(request.SessionId);

                // 登录立即上报指标，设置在线状态
                //_ = await http.SetVendLineStatus(new VendLineStatusInput()
                //{
                //    LineStatus = true,
                //    VendCode = request.Mid,
                //});
                var reply = new BaseCmd { Mid = request.Mid };

                await replyCommandService.SendAsync(reply, request);
                _logger.LogInformation($" 客户端 {request.Mid}登录成功");
            }
            else
            {
                _logger.LogError($"未获取登录密钥{request.Mid}");
                await replyCommandService.CloseAsync(request.SessionId);
            }
            return;
        }

    }

}
