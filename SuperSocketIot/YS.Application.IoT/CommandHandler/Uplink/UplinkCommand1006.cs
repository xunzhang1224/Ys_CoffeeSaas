namespace YS.Application.IoT.CommandHandler.Uplink
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Jaina;
    using YS.Application.IoT.Service.ReplyCommand;
    using YS.Application.IoT.Wrapper;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities;
    using YS.Domain.IoT.Util;
    using YS.Application.IoT.CommandHandler;

    /// <summary>
    /// 上行指令 1006 的处理程序。
    /// 用于设备注册或状态上报，并根据业务逻辑返回响应。
    /// </summary>
    public class UplinkCommand1006 : CommandHandlerBase<UplinkCommand1006>
    {
        /// <summary>
        /// 初始化一个新的 UplinkCommand1006 实例。
        /// </summary>
        /// <param name="logger">日志记录器。</param>
        /// <param name="scopeFactory">服务作用域工厂，用于解析依赖项。</param>
        public UplinkCommand1006(ILogger<UplinkCommand1006> logger, IServiceScopeFactory scopeFactory)
            : base(logger, scopeFactory)
        {
        }

        /// <summary>
        /// 处理事件标识为 "1006" 和 "1006:V1" 的上行指令。
        /// </summary>
        /// <param name="context">事件执行上下文，包含消息体等信息。</param>
        /// <returns>异步操作任务。</returns>
        [EventSubscribe("1006")]
        [EventSubscribe("1006:V1")]
        public override async Task HandleAsync(EventHandlerExecutingContext context)
        {
            var payload = (MessageContext)context.Source;
            var replyCommandService = _serviceScope.ServiceProvider.GetRequiredService<IReplyCommandService>();

            // 解析请求数据
            var req = payload.Body.FromJson<UplinkEntity1006.Request>();
            if (req == null)
            {
                _logger.LogInformation($"UplinkCommand1006 请求参数解析失败");
                await replyCommandService.NoSendAsync(payload);
                return;
            }

            // 存储或更新设备信息
            var res = await ProjectUtil.StoreDeviceInfoAsync(req.Mid);
            if (!res)
            {
                // 若存储失败，则关闭会话连接
                await replyCommandService.CloseAsync(payload.SessionId);
                return;
            }

            // 如果没有事务ID，则生成一个唯一ID
            if (string.IsNullOrEmpty(req.TransId))
            {
                req.TransId = string.IsNullOrEmpty(payload.MessageId)
                    ? YitIdHelper.NextId().ToString()
                    : payload.MessageId;
            }

            try
            {
                // 调用 gRPC 服务处理业务逻辑
                if (await GrpcWrapp.Instance.GrpcCommandService.Uplink1006HandleAsync(req))
                {
                    // 构造响应并发送
                    await replyCommandService.SendAsync(
                        new UplinkEntity1006.Response
                        {
                            Mid = req.Mid,
                            TransId = req.TransId
                        },
                        payload);
                }
                else
                {
                    // 无需响应的情况
                    await replyCommandService.NoSendAsync(payload);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UplinkCommand1006 异常");
                await replyCommandService.NoSendAsync(payload);
            }
        }
    }
}