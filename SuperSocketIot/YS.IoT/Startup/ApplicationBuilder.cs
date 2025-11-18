
using Microsoft.Extensions.DependencyInjection;
using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YS.Domain.IoT.Option;
using Flurl.Http;
using YS.CoffeeMachine.Iot.Application.DownSend.GRPC;
using YS.Application.IoT.Manager;
using YS.Application.IoT.Service.GRPC;
using YS.Application.IoT.Service.Http;
using YS.Application.IoT.Service.ReplyCommand;

namespace YS.IoT.Startup
{
    /// <summary>
    /// ApplicationBuilder
    /// </summary>
    public static class ApplicationBuilder
    {
        /// <summary>
        /// AddInjection
        /// </summary>
        /// <param name="services"></param>
        public static void AddInjection(this IServiceCollection services)
        {
            // 注册 FlurlClient 或 IFlurlClient 到服务容器中
            var remoteIPHost = AppSettingsHelper.GetContent("IHttp", "Vend", "Address");
            services.AddSingleton(
            sp =>
            new FlurlClientCache()
            .Add("CoreIntegration", remoteIPHost)
            );
            // 注册 IHttpb
            services.AddTransient<IHttp, HttpService>();
            //服务依赖注入
            services.AddTransient<IReplyCommandService, ReplyCommandService>();
            services.AddSingleton<ICommandSender, CommandSender>();
            services.AddSingleton<ISocketGrpcService, SocketGrpcService>();
            services.AddSingleton<IBlacklistManager, BlacklistManager>();
        }
    }
}
