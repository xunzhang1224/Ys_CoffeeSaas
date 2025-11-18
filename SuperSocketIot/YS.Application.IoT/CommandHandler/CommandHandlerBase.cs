namespace YS.Application.IoT.CommandHandler
{
    using System;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Jaina;

    /// <summary>
    /// 命令处理程序基类，用于构建IoT命令处理器的统一模板。
    /// 提供日志记录、依赖注入作用域及JSON序列化支持。
    /// </summary>
    /// <typeparam name="T">继承此类的具体命令处理器类型。</typeparam>
    public abstract class CommandHandlerBase<T> : IEventSubscriber, IDisposable
    {
        /// <summary>
        /// 日志记录器，用于记录运行时信息、异常等。
        /// </summary>
        public readonly ILogger _logger;

        /// <summary>
        /// 服务作用域，用于在命令处理过程中解析依赖项。
        /// </summary>
        protected readonly IServiceScope _serviceScope;

        /// <summary>
        /// JSON序列化配置选项，使用宽松的编码器以避免转义特殊字符。
        /// </summary>
        private static readonly JsonSerializerOptions _jsonSerializerOptions =
           new JsonSerializerOptions
           {
               Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
           };

        /// <summary>
        /// 初始化一个新的 CommandHandlerBase 实例。
        /// </summary>
        /// <param name="logger">日志记录器实例。</param>
        /// <param name="scopeFactory">服务作用域工厂，用于创建独立的服务作用域。</param>
        public CommandHandlerBase(ILogger<T> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _serviceScope = scopeFactory.CreateScope();
        }

        /// <summary>
        /// 处理事件上下文的方法，必须在派生类中重写。
        /// </summary>
        /// <param name="context">事件执行上下文。</param>
        /// <returns>异步操作任务。</returns>
        public virtual Task HandleAsync(EventHandlerExecutingContext context)
        {
            throw new Exception("必须重写该方法");
        }

        /// <summary>
        /// 释放资源，包括服务作用域。
        /// </summary>
        public void Dispose()
        {
            _serviceScope.Dispose();
        }

        /// <summary>
        /// 将对象序列化为JSON字符串。
        /// </summary>
        /// <typeparam name="TU">要序列化的对象类型。</typeparam>
        /// <param name="tU">要序列化的对象。</param>
        /// <param name="encoder">可选的JavaScript编码器，用于控制特殊字符转义方式。</param>
        /// <returns>序列化后的JSON字符串。</returns>
        public string MapString<TU>(TU tU, System.Text.Encodings.Web.JavaScriptEncoder? encoder = null)
        {
            return JsonSerializer.Serialize(tU, _jsonSerializerOptions);
        }
    }
}