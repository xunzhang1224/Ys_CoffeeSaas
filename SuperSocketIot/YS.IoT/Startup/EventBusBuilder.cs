using Jaina;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YS.Application.IoT.CommandHandler.Filter;
using YS.Application.IoT.CommandHandler.Uplink;
using YS.IoT.Startup;

namespace YS.IoT.Startup
{
    /// <summary>
    /// EventBusBuilder
    /// </summary>
    public static class EventBusBuilder
    {
        /// <summary>
        /// AddEventBus
        /// </summary>
        /// <param name="eventBus"></param>
        public static void AddEventBus(this IServiceCollection eventBus)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            eventBus.AddEventBus(op =>
             {
                 // 批量注册事件订阅者
                 op.AddSubscribers(typeof(UplinkCommand1000).Assembly);
                 op.ChannelCapacity = 4000;
                 op.GCCollect = true;
                 op.AddMonitor<CommandFilter>();
                 op.UnobservedTaskExceptionHandler = (obj, args) =>
                 {
                     logger.Error(args.Exception.InnerException, args.Exception.Message);
                 };
             });
            //// 注册 ToDo 事件订阅者
            //#region Uplink
            //eventBus.AddSubscriber<UplinkCommand1000>();
            //eventBus.AddSubscriber<UplinkCommand1001>();
            //eventBus.AddSubscriber<UplinkCommand1004>();
            //eventBus.AddSubscriber<UplinkCommand1006>();
            //eventBus.AddSubscriber<UplinkCommand1008>();
            //eventBus.AddSubscriber<UplinkCommand1010>();
            //eventBus.AddSubscriber<UplinkCommand1012>();
            //eventBus.AddSubscriber<UplinkCommand1013>();
            //eventBus.AddSubscriber<UplinkCommand1014>();
            //eventBus.AddSubscriber<UplinkCommand1111>();
            //eventBus.AddSubscriber<UplinkCommand1201>();
            //eventBus.AddSubscriber<UplinkCommand1204>();
            //eventBus.AddSubscriber<UplinkCommand1205>();
            //eventBus.AddSubscriber<UplinkCommand1210>();
            //eventBus.AddSubscriber<UplinkCommand2000>();
            //eventBus.AddSubscriber<UplinkCommand2001>();
            //eventBus.AddSubscriber<UplinkCommand2009>();
            //eventBus.AddSubscriber<UplinkCommand4201>();
            //eventBus.AddSubscriber<UplinkCommand5204>();
            //eventBus.AddSubscriber<UplinkCommand5205>();
            //eventBus.AddSubscriber<UplinkCommand5206>();
            //eventBus.AddSubscriber<UplinkCommand5207>();
            //eventBus.AddSubscriber<UplinkCommand5209>();
            //eventBus.AddSubscriber<UplinkCommand5213>();
            //eventBus.AddSubscriber<UplinkCommand7200>();
            //eventBus.AddSubscriber<UplinkCommand7201>();
            //eventBus.AddSubscriber<UplinkCommand7202>();
            //eventBus.AddSubscriber<UplinkCommand7211>();
            //eventBus.AddSubscriber<UplinkCommand7212>();

            //#endregion
            //#region Downlink
            //eventBus.AddSubscriber<DownlinkCommand1011>();
            //eventBus.AddSubscriber<DownlinkCommand1203>();
            //eventBus.AddSubscriber<DownlinkCommand3201>();
            //eventBus.AddSubscriber<DownlinkCommand6002>();
            //eventBus.AddSubscriber<DownlinkCommand6200>();
            //eventBus.AddSubscriber<DownlinkCommand6216>();
            //#endregion
        }
    }
}
