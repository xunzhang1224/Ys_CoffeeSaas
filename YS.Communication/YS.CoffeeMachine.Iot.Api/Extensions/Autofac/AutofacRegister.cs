
using Autofac;
using Autofac.Extras.DynamicProxy;
using YS.CoffeeMachine.Iot.Api.Extensions.Autofac.Aop;
using YS.CoffeeMachine.Iot.Api.Iot.CommandHandler;
using YS.CoffeeMachine.Iot.Api.Iot.Interfaces;
using YS.CoffeeMachine.Iot.Application.DownSend.GRPC;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands;

namespace YS.CoffeeMachine.Iot.Api.Extensions.Autofac
{
    /// <summary>
    /// cap消费者注册aop绑定拓展
    /// </summary>
    public class AutofacRegister : Module
    {
        /// <summary>
        /// 下发服务注册
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandSender>().As<ICommandSenderService>().InstancePerLifetimeScope();
            // 注册拦截器
            builder.RegisterType<IotSeedServerInterceptor>();

            builder.RegisterType<CommandSender>()
                   .As<ICommandSenderService>()
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(IotSeedServerInterceptor));

        }
    }
}
