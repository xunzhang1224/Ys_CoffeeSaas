using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using MagicOnion;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Extensions.Cap.Aop;
using YS.CoffeeMachine.Cap;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Iot.Api.Extensions.Cap.Subscribers;
using YS.CoffeeMachine.Iot.Api.Extensions.Http;

namespace YS.CoffeeMachine.Iot.Api.Extensions
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 添加应用程序服务
        /// </summary>
        /// <param name="builder"></param>
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddMCodeCap();

            // 注册 HttpClientFactory
            builder.Services.AddHttpClient();

            // 注册 ApiService
            builder.Services.AddScoped<HttpService>();
        }

        /// <summary>
        /// cap服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <param name="capSection"></param>
        /// <param name="rabbitMQSection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddMCodeCap(this IServiceCollection services, Action<CapOptions> configure = null, string capSection = "cap", string rabbitMQSection = "rabbitmq")
        {
            var rabbitMQOptionServer = services.Configure<RabbitMQOptions>(ServiceProviderServiceExtensions.GetRequiredService<IConfiguration>(services.BuildServiceProvider()).GetSection(rabbitMQSection));

            var rabbitMQOptions = ServiceProviderServiceExtensions.GetRequiredService<IConfiguration>(services.BuildServiceProvider()).GetSection(rabbitMQSection).Get<RabbitMQOptions>();

            if (rabbitMQOptions == null || string.IsNullOrEmpty(rabbitMQOptions.HostName))
            {
                throw new ArgumentNullException("RabbitMQ 配置错误或未设置");
            }

            var logger = ServiceProviderServiceExtensions.GetRequiredService<ILogger<CoffeeMachinePlatformDbContext>>(services.BuildServiceProvider());

            if (rabbitMQOptions == null)
            {
                throw new ArgumentNullException("rabbitmq not config.");
            }

            var capJson = ServiceProviderServiceExtensions.GetRequiredService<IConfiguration>(services.BuildServiceProvider()).GetValue<string>(capSection);

            if (string.IsNullOrEmpty(capJson))
            {
                throw new ArgumentException("cap未设置");
            }

            //services.AddDbContext<CoffeeMachinePlatformDbContext>(options => options.UseMySql(capJson, ServerVersion.AutoDetect(capJson)));

            services.AddCap(x =>
            {
                //使用RabbitMQ传输
                x.UseRabbitMQ(opt => { opt = rabbitMQOptions; });

                //使用MySQL持久化
                //x.UseMySql(capJson);

                //使用SQL server持久化
                x.UseSqlServer(capJson);

                //x.UseEntityFramework<CoffeeMachinePlatformDbContext>();

                x.UseDashboard();

                // 失败重试次数
                x.FailedRetryCount = 5;

                x.FailedMessageExpiredAfter = 3600 * 24 * 3;

                // 设置自动清理过期数据的间隔（例如每小时执行一次清理任务）
                x.SucceedMessageExpiredAfter = 3600 * 24 * 1;

                // 消费者线程并行处理消息的线程数
                x.ConsumerThreadCount = 10;
                // 发送消息的任务将由.NET线程池并行处理
                x.EnablePublishParallelSend = true;

                x.TopicNamePrefix = "CoffeeMachine";

                //失败回调，通过企业微信，短信通知人工干预
                x.FailedThresholdCallback = (e) =>
                {
                    if (e.MessageType == MessageType.Publish)
                    {
                        logger.LogError("Cap发送消息失败;" + JsonConvert.SerializeObject(e.Message));
                    }
                    else if (e.MessageType == MessageType.Subscribe)
                    {
                        logger.LogError("Cap接收消息失败;" + JsonConvert.SerializeObject(e.Message));
                    }
                };

                configure?.Invoke(x);
            }).AddSubscribeFilter<CapFilter>();

            // 自动注册所有订阅服务
            services.Scan(scan => scan
                 .FromAssemblyOf<DeviceBindSubscriber>()
                .AddClasses(c => c.AssignableTo<ICapSubscribe>())
                .AsSelf()
                .WithScopedLifetime());

            services.AddScoped<IPublishService, PublishService>();
            return services;
        }
    }
}
