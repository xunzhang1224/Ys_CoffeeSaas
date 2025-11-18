using NLog;
using System.Text.Json;
using YS.Application.IoT.Manager;
using YS.Application.IoT.Service.Http;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.Domain.IoT;
using YS.Domain.IoT.Model;

namespace YS.Application.IoT.CommandHandler.Filter
{
    /// <summary>
    /// 消息事件AOP
    /// </summary>
    public class CommandFilter : IEventHandlerMonitor, IDisposable
    {
        private readonly ILogger<CommandFilter> _logger;
        private readonly IServiceScope _serviceScope;
        private readonly NLog.ILogger _commandlogger = LogManager.GetLogger("CommandContext");

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        public CommandFilter(
            ILogger<CommandFilter> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _serviceScope = scopeFactory.CreateScope();
        }

        /// <summary>
        /// 命令执行前
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OnExecutingAsync(EventHandlerExecutingContext context)
        {
            // 验签
            var commandContext = (MessageContext)context.Source;
            BaseCmd mainInfo = JsonSerializer.Deserialize<BaseCmd>(commandContext.Body, JsonOptions.DefaultJsonSerializerOptions);
            _logger.LogInformation($"命令执行前:{commandContext.Key}----》{commandContext.Body}");
            string signKey = string.Empty;
            if (commandContext.Key == "1000")
            {
                //未注册设备用私钥加密
                signKey = JsonSerializer.Deserialize<VendToken>(commandContext.Body, JsonOptions.DefaultJsonSerializerOptions)?.PubKey ?? string.Empty;
            }
            else
            {
                //注册后的设备用私钥加密
                var httpService = _serviceScope.ServiceProvider.GetRequiredService<IHttp>();
                var deviceInfoSecret = await httpService.GetPrivKey(commandContext.Mid);
                if (deviceInfoSecret == null)
                {
                    var blacklistManager = _serviceScope.ServiceProvider.GetRequiredService<IBlacklistManager>();
                    await blacklistManager.AddToBlacklist(commandContext.Mid, BlacklistTypeEnum.Device);
                }
                signKey = deviceInfoSecret?.PrivaKey;
            }
            string sysSign = string.Empty;
            if (!commandContext.CheckSign(mainInfo, signKey, out sysSign))
            {
                _logger.LogError($" 解码失败设备{commandContext.Mid}的{commandContext.Key}协议,安卓签名{commandContext.Sign},系统签名{sysSign}");
                throw new IotException($" 签名认证失败：设备{commandContext.Mid}的{commandContext.Key}协议,安卓签名{commandContext.Sign},系统签名{sysSign}");
            }
            commandContext.SetSignKey(signKey);
            // 如果 消息等级 必答
            //if (iotCommandContext.Level == 1)
            //{
            //}

        }

        /// <summary>
        /// 命令执行后
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task OnExecutedAsync(EventHandlerExecutedContext context)
        {
            // 记录日志
            var iotCommandContext = (MessageContext)context.Source;
            try
            {
                iotCommandContext.Exception = string.IsNullOrEmpty(iotCommandContext.Exception) ? context.Exception?.InnerException?.Message ?? string.Empty : iotCommandContext.Exception;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行异常：{EventId}", context.Source.EventId);
                iotCommandContext.Exception = ex.Message;
            }
            finally
            {
                iotCommandContext.End();
                _commandlogger.Warn("{JoviCommandContext}", iotCommandContext);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 释放服务作用域
        /// </summary>
        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}
