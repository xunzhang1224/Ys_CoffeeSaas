namespace YS.CoffeeMachine.API.Extensions.Cap.Aop
{
    using DotNetCore.CAP.Filter;
    using FreeRedis;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// CAP消息订阅过滤器
    /// </summary>
    public class CapFilter(ILogger<CapFilter> logger, IRedisClient _redis) : SubscribeFilter
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string msgId = string.Empty;

        /// <summary>
        /// 订阅方法执行前
        /// </summary>
        /// <param name="context">执行上下文</param>
        /// <returns>异步任务</returns>
        public override async Task OnSubscribeExecutingAsync(ExecutingContext context)
        {
            msgId = context.DeliverMessage.Headers["cap-msg-id"];
            var value = context.DeliverMessage.Value.ToString();
            var key = $"Cap:MsgHeaderId:{msgId}";
            if (!await _redis.SetNxAsync(key, 0))
                throw new Exception($"重复消息:{key};消息：{value}");
            else
                await _redis.ExpireAsync(key, TimeSpan.FromHours(1));
        }

        /// <summary>
        /// 订阅方法执行后
        /// </summary>
        /// <param name="context">执行上下文</param>
        /// <returns>异步任务</returns>
        public override Task OnSubscribeExecutedAsync(ExecutedContext context)
        {
            // 订阅方法执行后
            return Task.CompletedTask;
        }

        /// <summary>
        /// 订阅方法执行异常
        /// </summary>
        /// <param name="context">异常上下文</param>
        /// <returns>异步任务</returns>
        public async override Task OnSubscribeExceptionAsync(ExceptionContext context)
        {
            if (context.Exception.Message.Contains("重复消息"))
            {
                context.ExceptionHandled = true;
            }
            else
            {
                await _redis.DelAsync(msgId);
            }
            logger.LogError("Cap 异常: {0}", context.Exception.Message);
        }
    }
}
