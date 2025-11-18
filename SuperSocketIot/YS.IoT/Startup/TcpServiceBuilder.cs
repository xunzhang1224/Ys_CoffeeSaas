using Masuit.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket.ProtoBase;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Host;
using System.Net;
using YS.Core.IoT.Socket;
using YS.Domain.IoT.Option;
using YS.Domain.IoT.Session;
using YS.Domain.IoT.Util;
using static System.Collections.Specialized.BitVector32;

namespace YS.IoT.Startup
{
    /// <summary>
    /// TcpServiceBuilder
    /// </summary>
    public static class TcpServiceBuilder
    {
        /// <summary>
        /// AddTcpService
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection AddTcpService(this IServiceCollection services, IConfiguration configuration, string section = "SuperServerOptions")
        {
            var section1 = configuration.GetSection(section);
            if (!section1.Exists())
            {
                throw new Exception($"Config file not exist '{section}' section.");
            }
            services.Configure<SuperServerOptions>(section1);

            SessionManager<SocketSession> store = new SessionManager<SocketSession>();
            services.AddSingleton(store);

            services.AddSingleton<ISuperSessionContainer, SuperSessionContainer>();
            services.AddHostedService<SocketHostedService>();
            services.AddHostedService<ClearIdleSessionJob>();

            return services;
        }

        /// <summary>
        /// MyAppSession
        /// </summary>
        public class MyAppSession : AppSession
        {
            /// <summary>
            /// 设备id
            /// </summary>
            public string DeviceId { get; set; }

            /// <summary>
            /// OnSessionConnectedAsync
            /// </summary>
            /// <returns></returns>

            protected override async ValueTask OnSessionConnectedAsync()
            {
                await base.OnSessionConnectedAsync();
            }
        }
    }
}
