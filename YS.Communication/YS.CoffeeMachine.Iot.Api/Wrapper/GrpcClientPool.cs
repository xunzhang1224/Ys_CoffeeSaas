// YS.K12
using FreeRedis;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using Polly;
using System.Collections.Concurrent;
using System.Diagnostics;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Iot.Application.DownSend.GRPC;
using YSCore.Base.App;
using YSCore.Base.DependencyInjection.Dependencies;

namespace YS.CoffeeMachine.Platform.API.Extensions.TaskSchedulingBase.Wrapper;

/// <summary>
/// GrpcClientPool
/// </summary>
/// <param name="log"></param>
/// <param name="_cache"></param>
public class GrpcClientPool(ILogger<GrpcClientPool> log,IRedisClient _cache, IConfiguration _configuration)
{
    private readonly ConcurrentDictionary<string, GrpcChannel> _grpcChannels = new ConcurrentDictionary<string, GrpcChannel>();
    private readonly ConcurrentDictionary<string, ICommandSender> _commandSenders = new ConcurrentDictionary<string, ICommandSender>();

    /// <summary>
    /// 获取或创建针对特定设备的GRPC客户端
    /// </summary>
    /// <param name="mid">设备号</param>
    /// <returns></returns>
    public async Task<ICommandSender> GetOrCreatePubClientAsync(string mid)
    {
        // 动态获取最新的 GrpcServerAddr
        var grpcServerAddr = GetGrpcServerAddrFromRedisAsync(mid);
        if (string.IsNullOrEmpty(grpcServerAddr))
        {
            throw new InvalidOperationException("无法从 Redis 获取有效的 GrpcServerAddr");
        }

        // 如果该地址已有客户端，则直接返回
        if (_commandSenders.TryGetValue(grpcServerAddr, out var commandSender))
        {
            return commandSender;
        }

        // 否则创建新的客户端
        var channel = _grpcChannels.GetOrAdd(grpcServerAddr, addr => GrpcChannel.ForAddress(addr));
        commandSender = MagicOnionClient.Create<ICommandSender>(channel, new IClientFilter[] { new RetryFilter(log) });

        // 更新命令发送者
        _commandSenders[grpcServerAddr] = commandSender;

        return commandSender;
    }

    /// <summary>
    /// 根据指定的设备获取Grpc地址
    /// </summary>
    /// <param name="mid">设备号</param>
    /// <returns></returns>
    private string GetGrpcServerAddrFromRedisAsync(string mid)
    {
        //var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        //if (environmentName == "Development")
        //{
            ////暂时单机Tcp只有一个地址
            return _configuration["GrpcForAddress"];
        //}
        // 使用 SysCacheService 从 Redis获取 GrpcServerAddr
        //var cacheKey = string.Format(CacheConst.VendChannelKey, mid);
        // 创建重试策略
        //var retryPolicy = Policy
        //    .HandleResult<string>(result => result == null || string.IsNullOrWhiteSpace(result))
        //    .WaitAndRetry(2, retryAttempt =>
        //        TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)), // 指数退避算法
        //        onRetry: (outcome, timespan, retryCount, context) =>
        //        {
        //            log.LogWarning($"第 {retryCount} 次重试：未找到 {mid}的GrpcServerAddr");
        //        }
        //    );
        //// 使用重试策略执行从 Redis 获取数据的操作
        //string grpcServerAddr = retryPolicy.Execute(() =>
        //{
        //    return _cache.HGet<string>(cacheKey, "GrpcServerAddr");
        //});
        // 如果仍然没有获取到数据，则返回默认地址
        //if (grpcServerAddr == null || string.IsNullOrWhiteSpace(grpcServerAddr))
        //{
            //return _configuration["GrpcForAddress"];
        //}
        //else
        //{
        //    return $"http://{grpcServerAddr}";
        //}
    }

    /// <summary>
    /// GetCommandSenderClientsAsync
    /// </summary>
    /// <returns></returns>
    public async Task<ConcurrentDictionary<string, ICommandSender>> GetCommandSenderClientsAsync()
    {
        return _commandSenders;
    }

    /// <summary>
    /// 失败重试 用于处理切换发布时候的请求丢失问题
    /// </summary>
    /// <param name="log"></param>
    public class RetryFilter(ILogger<GrpcClientPool> log) : IClientFilter
    {
        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async ValueTask<ResponseContext> SendAsync(RequestContext context, Func<RequestContext, ValueTask<ResponseContext>> next)
        {
            var header = context.CallOptions.Headers;
            if (!header.Any(x => x.Key == "ys-router"))
            {
                header.Add("ys-router", "mqttlistener2jovi");
            }
            if (!header.Any(x => x.Key == "ys-client"))
            {
                header.Add("ys-client", "localhost");
            }
            Exception lastException = null;
            var retryCount = 0;
            while (retryCount < 3)
            {
                try
                {
                    // using same CallOptions so be careful to add duplicate headers or etc.
                    return await next(context);
                }
                catch (RpcException ex)
                {
                    if (ex.Status.Detail == "Bad gRPC response. HTTP status code: 504")
                    {
                        retryCount = 10; //超时时间很长 超时不重试
                    }
                    else if (ex.Status.Detail == "Bad gRPC response. HTTP status code: 502")
                    {
                        retryCount = 12; //超时第二个情况
                    }
                    log.LogError(ex + $"Grpc excetion on path:{context.MethodPath} {ex.Message}");
                }
                catch (OperationCanceledException ex)
                {
                    retryCount = 10; //cancel不重试
                    log.LogError(ex + $"Grpc excetion on path:{context.MethodPath} {ex.Message}");
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    log.LogError(ex + $"Grpc excetion on path:{context.MethodPath} {ex.Message}");
                }
                retryCount++;
            }
            log.LogError($"Retry failed  on path:{context.MethodPath} with {lastException?.Message}", lastException);
            throw new Exception($"Retry failed  on path:{context.MethodPath} with {lastException?.Message}", lastException);
        }

        private void HandleRpcException(RpcException ex, ref int retryCount)
        {
            if (ex.StatusCode == StatusCode.Unknown)
            {
                if (ex.Status.Detail == "Bad gRPC response. HTTP status code: 504")
                {
                    retryCount = 10; //服务无响应超时不重试
                }
                else if (ex.Status.Detail == "Bad gRPC response. HTTP status code: 502")
                {
                    retryCount = 12; //服务无响应超时不重试
                }
            }
            else if (ex.StatusCode == StatusCode.DeadlineExceeded)
            {
                retryCount = 13; //客户端超时 不重试
            }
            log.LogError($"{ex.Message} 在路径:{ex.Trailers}");
            //Log.Error($"获取加密信息失败: {ex.Message}");

        }
    }

    /// <summary>
    /// LoggingFilter
    /// </summary>
    /// <param name="log"></param>
    public class LoggingFilter(ILogger<LoggingFilter> log) : IClientFilter
    {
        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async ValueTask<ResponseContext> SendAsync(RequestContext context, Func<RequestContext, ValueTask<ResponseContext>> next)
        {
            log.LogInformation("Request Begin:" + context.MethodPath); // Debug.Log in Unity.

            var sw = Stopwatch.StartNew();
            var response = await next(context);
            sw.Stop();

            log.LogInformation("Request Completed:" + context.MethodPath + ", Elapsed:" + sw.Elapsed.TotalMilliseconds + "ms");

            return response;
        }
    }
}