//using Grpc.Core;
//using Grpc.Net.Client;
//using MagicOnion.Client;
//using NLog;
//using System.Threading.Channels;
//using YS.Domain.IoT.Option;
//using YS.K12.Backstage.Shared.ItomModule.Commands;

//namespace YS.K12.Application.IoT.Wrapper
//{
//    public class GrpcWrapp
//    {
//        private static readonly Lazy<GrpcWrapp> _instance = new Lazy<GrpcWrapp>(() => new GrpcWrapp());
//        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
//        private readonly Channel<GrpcChannel> _clients;
//        public IGrpcCommandService GrpcCommandService => GetGrpcClient<IGrpcCommandService>();

//        private readonly string _urlString;

//        public readonly string _localhost;

//        private int _limitcount = 20;
//        public static GrpcWrapp Instance => _instance.Value;
//        public GrpcWrapp()
//        {
//            _urlString = AppSettingsHelper.GetContent("K12Link:GrpcGate");
//            _localhost = Environment.GetEnvironmentVariable("LocalIp") ?? "localhost";
//            _clients = System.Threading.Channels.Channel.CreateBounded<GrpcChannel>(new BoundedChannelOptions(50)
//            {
//                FullMode = BoundedChannelFullMode.DropOldest
//            });
//            for (int i = 0; i < 10; i++)
//            {
//                _clients.Writer.TryWrite(CreateGrpcChannel(_urlString));
//            }
//        }
//        private T GetGrpcClient<T>() where T : class, MagicOnion.IService<T>
//        {
//            var channel = GetGrpcChannel();
//            var client = MagicOnionClient.Create<T>(channel, new IClientFilter[] { new RetryFilter() });
//            if (channel != null)
//            {
//                _clients.Writer.TryWrite(channel);
//            }
//            return client;
//        }
//        private GrpcChannel GetGrpcChannel()
//        {
//            if (_clients.Reader.TryRead(out var channel))
//            {
//                return channel;
//            }
//            else if (_limitcount > 0)
//            {
//                var newChannel = CreateGrpcChannel(_urlString);
//                _clients.Writer.TryWrite(newChannel);
//                _limitcount--;
//                return newChannel;
//            }
//            else
//            {
//                throw new Exception("超出最大连接数限制");
//            }
//        }
//        private GrpcChannel CreateGrpcChannel(string url)
//        {
//            var handler = new SocketsHttpHandler
//            {
//                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
//                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
//                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
//                EnableMultipleHttp2Connections = true
//            };
//            return GrpcChannel.ForAddress(url, new GrpcChannelOptions
//            {
//                HttpHandler = handler
//            });
//        }
//        public class RetryFilter : IClientFilter
//        {
//            public async ValueTask<ResponseContext> SendAsync(RequestContext context, Func<RequestContext, ValueTask<ResponseContext>> next)
//            {
//                var header = context.CallOptions.Headers;

//                if (header == null)
//                    throw new ArgumentNullException(nameof(header));

//                if (!header.Any(x => x.Key == "ys-router"))
//                {
//                    header.Add("ys-router", "backstagegrpcjovi");
//                }
//                if (!header.Any(x => x.Key == "ys-client"))
//                {
//                    header.Add("ys-client", GrpcWrapp.Instance._localhost);
//                }

//                Exception lastException = new Exception();
//                int retryCount = 0;

//                while (retryCount < 1)
//                {
//                    try
//                    {
//                        return await next(context);
//                    }
//                    catch (RpcException ex)
//                    {
//                        HandleRpcException(ex, ref retryCount);
//                    }
//                    catch (OperationCanceledException ex)
//                    {
//                        retryCount = 10;
//                        _logger.Error($"{ex.Message} on path:{context.MethodPath}");
//                    }
//                    catch (Exception ex)
//                    {
//                        lastException = ex;
//                        _logger.Error($"{ex.Message} on path:{context.MethodPath}");
//                    }
//                    retryCount++;
//                }

//                throw new Exception($"重试路径失败:{context.MethodPath}", lastException);
//            }

//            private void HandleRpcException(RpcException ex, ref int retryCount)
//            {
//                if (ex.StatusCode == StatusCode.Unknown)
//                {
//                    if (ex.Status.Detail == "Bad gRPC response. HTTP status code: 504")
//                    {
//                        retryCount = 10; //服务无响应超时不重试
//                    }
//                    else if (ex.Status.Detail == "Bad gRPC response. HTTP status code: 502")
//                    {
//                        retryCount = 12; //服务无响应超时不重试
//                    }
//                }
//                else if (ex.StatusCode == StatusCode.DeadlineExceeded)
//                {
//                    retryCount = 13; //客户端超时 不重试
//                }
//                _logger.Error($"{ex.Message} 在路径:{ex.Trailers}");
//                //Log.Error($"获取加密信息失败: {ex.Message}");

//            }
//        }

//    }
//}
