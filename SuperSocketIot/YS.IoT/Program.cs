
using CSRedis;
using Masuit.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.Net;
using Yitter.IdGenerator;
using YS.Domain.IoT.Option;
using YS.Domain.IoT.Util;
using YS.IoT.Startup;

namespace YS.IoT
{
    /// <summary>
    /// Program
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // 注册 CodePagesEncodingProvider 以便使用 GB2312 编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile(ProjectUtil.GetEnvironmentConfigurationPath(), optional: true, reloadOnChange: true);

            //var cfg = builder.Configuration.GetSection("NacosConfig");
            //// 调试：输出所有配置项
            //foreach (var item in cfg.AsEnumerable())
            //{
            //    Console.WriteLine($"{item.Key} = {item.Value}");
            //}
            //builder.Configuration.AddNacosV2Configuration(cfg);

            // 确保所有配置都已加载后，重新初始化AppSettingsHelper
            AppSettingsHelper.Initialize(builder.Configuration);

            var remoteIPHost = AppSettingsHelper.GetContent("IHttp", "Vend", "Address");
            Console.WriteLine($"Iot-Redis-Main：{AppSettingsHelper.GetContent("Iot-Redis-Main")}||{remoteIPHost}");
            Console.WriteLine($"SuperServer读取的配置{AppSettingsHelper.GetComplexContent<SuperServerOptions>("SuperServerOptions").ToJsonString()}");

            // var builder = Host.CreateApplicationBuilder(args);
            builder.Logging.ClearProviders();// 清除默认的日志提供程序
            builder.Logging.SetMinimumLevel(LogLevel.Debug);// 设置最小日志级别
            builder.Host.UseNLog();
            // 配置 gRPC 和 MagicOnion
            builder.WebHost.ConfigureKestrel(options =>
            {
                var port = AppSettingsHelper.GetContent("GrpcHttp2Listen", "Port").ToInt32();
                var ip = AppSettingsHelper.GetContent("GrpcHttp2Listen", "IP");
                options.Listen(IPAddress.Any, port, listenOptions =>
                {
                    //listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });
            // 设置雪花id的workerId，确保每个实例workerId都应不同
            var workerId = AppSettingsHelper.GetContent("SnowId", "WorkerId");

            YitIdHelper.SetIdGenerator(new IdGeneratorOptions { WorkerId = ushort.Parse(workerId) });
            //builder.Services.AddHostedService<Worker>();//后台工作
            //时间总线
            builder.Services.AddEventBus();
            //// 注册健康检查服务
            //builder.Services.AddHealthChecks();
            //服务依赖注入
            builder.Services.AddInjection();
            builder.Services.AddMemoryCache();
            //注册缓存
            RedisHelper.Initialization(new CSRedisClient(AppSettingsHelper.GetContent("Iot-Redis-Main")));

            //添加TcpService
            builder.Services.AddTcpService(builder.Configuration);
            //增加Grpc
            builder.Services.AddGrpc();
            builder.Services.AddMagicOnion();
            var app = builder.Build();
            //// 添加健康检查端点
            //app.MapHealthChecks("/health");
            app.MapMagicOnionService();
            app.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            });
            app.Run();

        }
    }
}
