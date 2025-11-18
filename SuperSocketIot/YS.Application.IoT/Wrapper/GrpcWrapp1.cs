using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using NLog;
using System.Net;
using System.Reflection;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands;

namespace YS.Application.IoT.Wrapper
{

    /// <summary>
    /// 主要优化点：
    /// 单一 GrpcChannel
    /// gRPC 官方推荐将 GrpcChannel 作为长连接复用，减少内存和资源占用。
    /// 单一 MagicOnionClient
    /// 每种服务只创建一个客户端实例，避免频繁 new/dispose 带来的性能损耗。
    /// 优雅关闭
    /// 实现 IAsyncDisposable，在应用退出或不再需要时，调用 DisposeAsync() 释放底层连接。
    /// 重试逻辑集中
    /// 将重试策略封装在 RetryFilter 中，可灵活扩展，并统一记录日志。
    /// 这样改造后，你的服务在高并发场景下能更好地复用连接、控制内存占用，并且在关闭时能正确释放资源。
    /// GrpcWrapp：单例封装类，负责管理 gRPC Channel 和 MagicOnion 客户端
    /// 实现了 IAsyncDisposable，应用停止时可优雅地关闭 Channel
    /// </summary>
    public sealed class GrpcWrapp : IAsyncDisposable
    {
        // Lazy 单例，确保线程安全且延迟初始化
        private static readonly Lazy<GrpcWrapp> _instance = new Lazy<GrpcWrapp>(() => new GrpcWrapp());
        // NLog 日志实例
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        // 唯一的 GrpcChannel 实例，长生命周期
        private readonly GrpcChannel _channel;
        // 唯一的 MagicOnion 客户端实例
        private readonly IGrpcCommandService _commandClient;
        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static GrpcWrapp Instance => _instance.Value;
        /// <summary>
        /// 私有构造函数：初始化 Channel 和客户端
        /// </summary>
        private GrpcWrapp()
        {
            // 从配置中读取 gRPC 网关地址
            var url = AppSettingsHelper.GetContent("K12Link:GrpcGate");
            //var url = "http://127.0.0.1:6100";
            // 获取本地 IP，用于在请求头中标识客户端
            var localIp = Environment.GetEnvironmentVariable("LocalIp") ?? "localhost";
            // 配置 HTTP/2 Handler，确保连接可复用且长连接
            var handler = new SocketsHttpHandler
            {
                // 连接空闲不超时
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                // 定期发送 ping 保活
                KeepAlivePingDelay = TimeSpan.FromSeconds(30),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(10),
                // 允许多路复用多个 HTTP/2 连接
                EnableMultipleHttp2Connections = true
            };
            // 创建全局唯一的 GrpcChannel
            _channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions
            {
                HttpHandler = handler,
                // 新增：连接故障时快速失败
                //ThrowOperationCanceledOnCancellation = true
                // 如有需要，可在此处调整最大消息大小等选项
            });
            // 创建全局唯一的 MagicOnion 客户端，并注入重试过滤器
            _commandClient = MagicOnionClient.Create<IGrpcCommandService>(
                _channel,
                new IClientFilter[] { new RetryFilter(localIp) }
            );
        }
        /// 对外暴露的 gRPC 命令服务客户端
        /// </summary>
        public IGrpcCommandService GrpcCommandService => _commandClient;
        /// <summary>
        /// 应用关闭或不再使用时，优雅地关闭 GrpcChannel
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            try
            {
                await _channel.ShutdownAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "关闭 gRPC Channel 时发生异常");
            }
        }
        /// <summary>
        /// 内部重试过滤器：为每次请求添加必要的 header，并在部分错误时进行重试
        /// </summary>
        private class RetryFilter : IClientFilter
        {
            private readonly string _localhost;
            private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
            /// <param name="localhost">本机 IP，用于请求头标识</param>
            public RetryFilter(string localhost)
            {
                _localhost = localhost;
            }
            /// <summary>
            /// 拦截并处理每一次请求，添加 header，并在 RpcException 等错误时重试
            /// </summary>
            public async ValueTask<ResponseContext> SendAsync(
                RequestContext context,
                Func<RequestContext, ValueTask<ResponseContext>> next)
            {
                // 确保 Header 不为 null
                var headers = context.CallOptions.Headers ?? throw new ArgumentNullException(nameof(context.CallOptions.Headers));
                // 添加自定义路由 header（如果不存在）
                if (!headers.Any(x => x.Key == "ys-router"))
                    headers.Add("ys-router", "backstagegrpcjovi");
                // 添加客户端标识 header（如果不存在）
                if (!headers.Any(x => x.Key == "ys-client"))
                    headers.Add("ys-client", _localhost);

                var newCallOptions = context.CallOptions
                    .WithHeaders(headers)
                    .WithDeadline(DateTime.UtcNow.AddSeconds(15));

                // 4. 替换上下文（context.CallOptions 是只读的）
                // 🔧 反射替换 context 中的 CallOptions（因为它是只读的）
                var callOptionsField = typeof(RequestContext).GetField("<CallOptions>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                if (callOptionsField is not null)
                {
                    callOptionsField.SetValue(context, newCallOptions);
                }

                Exception lastEx = null;
                const int maxRetries = 3;  // 最多重试次数
                for (int attempt = 0; attempt <= maxRetries; attempt++)
                {
                    try
                    {
                        // 执行实际的 gRPC 调用
                        return await next(context).ConfigureAwait(false);
                    }
                    catch (RpcException rpcEx)
                    {
                        lastEx = rpcEx;
                        // 指数退避 + 随机抖动
                        var delay = TimeSpan.FromMilliseconds(
                            Math.Min(1000, 50 * Math.Pow(2, attempt))
                            + Random.Shared.Next(0, 50)
                        );
                        await Task.Delay(delay);
                        HandleRpcException(rpcEx, ref attempt);
                    }
                    catch (OperationCanceledException cancelEx)
                    {
                        lastEx = cancelEx;
                        _logger.Error(cancelEx, $"调用被取消：{context.MethodPath}");
                        break; // 取消不重试
                    }
                    catch (Exception ex)
                    {
                        lastEx = ex;
                        _logger.Error(ex, $"未知异常，方法：{context.MethodPath}");
                    }
                }
                _logger.Error($"gRPC 调用在重试后仍失败：{context.MethodPath},{lastEx.ToString()}");
                // 如果所有重试都失败，则抛出异常
                throw new Exception($"gRPC 调用在重试后仍失败：{context.MethodPath}", lastEx);
            }
            /// <summary>
            /// 根据不同的 gRPC 状态码决定是否继续重试
            /// </summary>
            private void HandleRpcException(RpcException ex, ref int attempt)
            {
                _logger.Error(ex, $"gRPC 错误，状态码：{ex.StatusCode}，详情：{ ex.ToJsonString()}");
                // 对特定的 HTTP 转换错误，不再重试
                if (ex.StatusCode == StatusCode.Unknown &&
                    (ex.Status.Detail.Contains("504") || ex.Status.Detail.Contains("502")))
                {
                    attempt = int.MaxValue; // 标记不再重试
                }
                // 客户端超时也不重试
                else if (ex.StatusCode == StatusCode.DeadlineExceeded)
                {
                    attempt = int.MaxValue;
                }
            }
        }
    }
}