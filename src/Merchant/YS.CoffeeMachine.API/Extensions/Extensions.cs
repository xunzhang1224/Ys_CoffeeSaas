using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using FluentValidation;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using YS.Cabinet.Payment.Alipay;
using YS.Cabinet.Payment.WechatPay;
using YS.CoffeeMachine.API.Application.Services;
using YS.CoffeeMachine.API.Application.Validations.TaskSchedulingInfoCommandValidators;
using YS.CoffeeMachine.API.Controllers.Aop;
using YS.CoffeeMachine.API.Extensions.BackTask;
using YS.CoffeeMachine.API.Extensions.Cap.Aop;
using YS.CoffeeMachine.API.Extensions.Cap.Subscribers;
using YS.CoffeeMachine.API.Extensions.IExecl;
using YS.CoffeeMachine.API.Extensions.TaskSchedulingBase.BackgroundJobs;
using YS.CoffeeMachine.API.Queries.AdvertisementQueries;
using YS.CoffeeMachine.API.Queries.ApplicationInfoQueries;
using YS.CoffeeMachine.API.Queries.BasicQueries.FaultCodeInfo;
using YS.CoffeeMachine.API.Queries.BasicQueries.Language;
using YS.CoffeeMachine.API.Queries.BeverageQueries;
using YS.CoffeeMachine.API.Queries.BeverageQueries.BeverageCollectionQueries;
using YS.CoffeeMachine.API.Queries.BeverageQueries.BeverageInfoTemplateQueries;
using YS.CoffeeMachine.API.Queries.CardQueries;
using YS.CoffeeMachine.API.Queries.Consumer.MarketingActivitys;
using YS.CoffeeMachine.API.Queries.CountryAndRegionQueries;
using YS.CoffeeMachine.API.Queries.DevicesQueries;
using YS.CoffeeMachine.API.Queries.DomesticPaymentQueries;
using YS.CoffeeMachine.API.Queries.FileResourceQueries;
using YS.CoffeeMachine.API.Queries.GoodsQueries;
using YS.CoffeeMachine.API.Queries.IOperationLog;
using YS.CoffeeMachine.API.Queries.IotCardInfoQueries;
using YS.CoffeeMachine.API.Queries.OperationLog;
using YS.CoffeeMachine.API.Queries.OrderQueries;
using YS.CoffeeMachine.API.Queries.PaymentInfoQueries;
using YS.CoffeeMachine.API.Queries.ProductCategoryQueries;
using YS.CoffeeMachine.API.Queries.ReportsQueries;
using YS.CoffeeMachine.API.Queries.ServiceProviderQueries;
using YS.CoffeeMachine.API.Queries.ServiceQueries;
using YS.CoffeeMachine.API.Queries.SettingQueries;
using YS.CoffeeMachine.API.Queries.TaskSchedulingInfoQueries;
using YS.CoffeeMachine.API.Services;
using YS.CoffeeMachine.API.Services.DeviceServices;
using YS.CoffeeMachine.API.Services.DomesticPaymentServices;
using YS.CoffeeMachine.API.Services.EmailServices;
using YS.CoffeeMachine.API.Services.LotCartService;
using YS.CoffeeMachine.API.Services.OrderInfoServices;
using YS.CoffeeMachine.API.Services.SmsServices;
using YS.CoffeeMachine.API.Services.TimezoneService;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.IServices.ILotCartService;
using YS.CoffeeMachine.Application.Queries.BasicQueries.FaultCode;
using YS.CoffeeMachine.Application.Queries.BasicQueries.Language;
using YS.CoffeeMachine.Application.Queries.CardQueries;
using YS.CoffeeMachine.Application.Queries.Consumer;
using YS.CoffeeMachine.Application.Queries.FileResource;
using YS.CoffeeMachine.Application.Queries.FileResourceQueries;
using YS.CoffeeMachine.Application.Queries.IAdvertisementQueries;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries.IBeverageCollectionQueries;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries.IBeverageInfoTemplateQueries;
using YS.CoffeeMachine.Application.Queries.ICountryAndRegionQueries;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Application.Queries.IDomesticPaymentQueries;
using YS.CoffeeMachine.Application.Queries.IGoods;
using YS.CoffeeMachine.Application.Queries.IIotCardInfoQueries;
using YS.CoffeeMachine.Application.Queries.IOrderQueries;
using YS.CoffeeMachine.Application.Queries.IPaymentInfoQueries;
using YS.CoffeeMachine.Application.Queries.IProductCategoryQueries;
using YS.CoffeeMachine.Application.Queries.IReportsQueries;
using YS.CoffeeMachine.Application.Queries.IServiceProviderInfoQueries;
using YS.CoffeeMachine.Application.Queries.IServiceQueries;
using YS.CoffeeMachine.Application.Queries.ISettingQueries;
using YS.CoffeeMachine.Application.Queries.ITaskSchedulingInfoQueries;
using YS.CoffeeMachine.Cap;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Platform.API.Application.Services.Basic.Docment;
using YS.CoffeeMachine.Provider;
using YS.CoffeeMachine.Provider.IServices;
using YS.Provider.OSS;
using YS.Provider.Snowflake;
using YSCore.Base.DataValidation.Extensions;
using YSCore.CoffeeMachine.SignalR.Services;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.API.Extensions
{
    /// <summary>
    /// 启动程序拓展类
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 添加应用程序服务
        /// </summary>
        /// <param name="builder"></param>
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "wRJZCm}]jrtaXh+AnK{z8Vp)5W3GgM#;6[Pekx7'9SQ/cD(uN$"))
            };

            // 将 TokenValidationParameters 注册为服务
            builder.Services.AddSingleton(tokenValidationParameters);

            // 注册 JwtTokenService 和接口
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            // 其他服务注册
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                // 如果需要允许跨域请求的连接
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var path = context.HttpContext.Request.Path;
                        if (path.StartsWithSegments("/signalrhub")) // SignalR Hub 地址
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            SnowflakeOptions? SnowflakeOption = builder.Configuration.GetSection("SnowId").Get<SnowflakeOptions>();
            builder.Services.AddSnowflake(options =>
            {
                options = SnowflakeOption;
            });

            // MediatR
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
                // cfg.AddOpenBehavior(typeof(NewCommandUnitOfWorkBehavior<,>));
                //cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>))
                //   .AddUnitOfWorkBehaviors();
                cfg.AddDataValidationBehavior()
                .AddUnitOfWorkBehaviors();
            });
            builder.Services.AddHttpContextAccessor();
            // 通过命名空间注入验证
            builder.Services.AddValidatorsFromAssembly(typeof(CreateTaskSchedulingInfoCommandValidators).Assembly);

            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

            // 后台任务
            builder.Services.AddHostedService<DeviceWarningTask>();
            builder.Services.AddHostedService<FileCenterDeleteTask>();

            builder.Services.AddFreeRedis();
            //builder.Services.AddRedis();
            builder.Services.AddMemoryCache();
            // 文件服务配置
            builder.Services.AddFileExcel();
            builder.Services.AddHostedService<FileUploadBackgroundService>();

            // 配置连接字符串 分表配置
            var constr = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<CoffeeMachineTimescaleDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("TimescaleDb"))); // 使用 QuestDB 兼容模式

            builder.Services.AddDbContext<CoffeeMachinePlatformDbContext>(options =>
            {
                //使用SqlServer
                options.UseSqlServer(constr);

                options.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

            builder.Services.AddRepositories(typeof(CoffeeMachinePlatformDbContext).Assembly);
            // 事务
            builder.Services.AddUnitOfWork<CoffeeMachinePlatformDbContext>();

            builder.Services.AddDbContext<CoffeeMachineDbContext>(options =>
            {
                //使用SqlServer
                options.UseSqlServer(constr);

                options.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

            builder.Services.AddRepositories(typeof(CoffeeMachineDbContext).Assembly);
            // 事务
            builder.Services.AddUnitOfWork<CoffeeMachineDbContext>();

            builder.Services.AddScoped<DeviceDownSeedActionFilter>();

            //builder.Services.AddInnerDependencyInjection(new[] { typeof(TaskSchedulingInfoQueries).Assembly });

            // 查询相关依赖注入
            builder.Services.AddScoped<ITaskSchedulingInfoQueries, TaskSchedulingInfoQueries>();// 任务调度查询
            builder.Services.AddScoped<IEnterpriseInfoQueries, EnterpriseInfoQueries>();// 企业信息查询
            builder.Services.AddScoped<IApplicationUserQueries, ApplicationUserQueries>();// 用户信息查询
            builder.Services.AddScoped<IApplicationRoleQueries, ApplicationRoleQueries>();// 角色信息查询
            builder.Services.AddScoped<IApplicationMenuQueries, ApplicationMenuQueries>();// 菜单信息查询
            builder.Services.AddScoped<IEnumHelperQueries, EnumHelperQueries>();// 枚举信息查询
            builder.Services.AddScoped<IEnumHelper, EnumHelper>();// 枚举信息查询
            builder.Services.AddScoped<IdeviceModelQueries, DeviceModelQueries>();// 设备查询
            builder.Services.AddScoped<IDeviceInfoQueries, DeviceInfoQueries>();// 设备查询
            builder.Services.AddScoped<IEnterpriseDevicesQueries, EnterpriseDevicesQueries>();// 设备分配查询
            builder.Services.AddScoped<IGroupsQueries, GroupsQueries>();// 分组信息查询
            builder.Services.AddScoped<IServiceProviderInfoQueries, ServiceProviderInfoQueries>();// 服务商信息查询
            builder.Services.AddScoped<ISettingInfoQueries, SettingInfoQueries>();// 时区信息查询
            builder.Services.AddScoped<ICountryInfoQueries, CountryInfoQueries>();// 国家信息查询
            builder.Services.AddScoped<ITimeZoneInfoQueries, TimeZoneInfoQueries>();// 时区信息查询
            builder.Services.AddScoped<IInterfaceStyleQueries, InterfaceStyleQueries>();// 时区信息查询
            builder.Services.AddScoped<ILanguageInfoQueries, LanguageInfoQueries>();// 多语言查询
            builder.Services.AddScoped<IBeverageInfoQueries, BeverageInfoQueries>();// 饮品信息查询
            builder.Services.AddScoped<IFormulaInfosQueries, FormulaInfosQueries>();// 饮品配方参数信息查询
            builder.Services.AddScoped<IBeverageInfoTemplateQueries, BeverageInfoTemplateQueries>();// 饮品库信息查询
            builder.Services.AddScoped<IBeverageCollectionInfoQueries, BeverageCollectionInfoQueries>();// 饮品集合信息查询
            builder.Services.AddScoped<IAdvertisementInfoQueries, AdvertisementInfoQueries>();// 广告信息查询
            builder.Services.AddScoped<IEarlyWarningConfigQueries, EarlyWarningConfigQueries>();// 预警信息查询
            builder.Services.AddScoped<IOperationLogQueries, OperationLogQueries>();// 操作日志
            builder.Services.AddScoped<IPaymentQueries, PaymentQueries>();// 支付相关查询
            builder.Services.AddScoped<IDeviceBaseQueries, DeviceBaseQueries>();
            builder.Services.AddScoped<IDevicePaymentQueries, DevicePaymentSettingQueries>(); // 设备支付相关查询
            builder.Services.AddScoped<IOrderInfoQueries, OrderInfoQueries>(); // 订单查询
            builder.Services.AddScoped<IFileManageQuerie, FileManageQuerie>(); // 文件管理查询
            builder.Services.AddScoped<IDictionaryQueries, DictionaryQueries>(); // 字典管理查询
            builder.Services.AddScoped<INoticeCfgQueries, NoticeCfgQueries>(); // 通知设置查询
            builder.Services.AddScoped<IReportsQuerie, ReportsQuerie>(); // 报表查询
            builder.Services.AddScoped<IFileCenterQuerie, FileCenterQuerie>();
            builder.Services.AddScoped<IEnterpriseQualificationInfoQueries, EnterpriseQualificationInfoQueries>();// 企业资质信息查询
            builder.Services.AddScoped<IFaultCodeInfoQueries, FaultCodeInfoQueries>(); // 故障码信息查询
            builder.Services.AddScoped<IProductCategoryQueries, ProductCategoryQueries>(); // 商品分类查询
            #region 微信支付宝支付相关
            builder.Services.AddScoped<ISystemPaymentInfoQueries, SystemPaymentInfoQueries>();// 国内支付信息查询
            builder.Services.AddScoped<IWechatPaymentQueries, WechatPaymentQueries>(); // 微信支付相关查询
            builder.Services.AddWechatService();// 注入微信商户服务
            builder.Services.AddAlipayService();// 注入支付宝商户服务
            //builder.Services.AddScoped<IWechatMerchantService, WechatMerchantService>();// 注入微信商户服务
            //builder.Services.AddScoped<IAlipayService, AlipayService>();// 注入支付宝商户服务
            builder.Services.AddTransient<PaymentPlatformUtil>(); // 支付对接帮助类
            builder.Services.AddTransient<ProfitSharingUnit>(); // 分账帮助类
            builder.Services.AddTransient<OrderPaymentMethodUnit>(); // 订单支付方式帮助类
            builder.Services.AddScoped<PaymentConfigService>(); // 支付配置服务
            builder.Services.AddScoped<IWechatMerchantOptionsProvider, PaymentConfigService>(); // 支付配置服务
            builder.Services.AddScoped<IAlipaySettingProvider, PaymentConfigService>(); // 支付配置服务
            builder.Services.AddScoped<IDivideAccountsConfigQueries, DivideAccountsConfigQueries>(); // 分账相关查询
            #endregion
            builder.Services.AddScoped<CommonHelper>();
            builder.Services.AddScoped<IClearDeviceRelationshipService, ClearDeviceRelationshipService>(); // 清除设备关系服务
            builder.Services.AddScoped<ICardQueries, CardQueries>();
            builder.Services.AddScoped<IGoodsQueries, GoodsQueries>();
            builder.Services.AddScoped<IPromotionQueries, PromotionQueries>();
            builder.Services.AddScoped<IIotCardInfoQuerie, IotCardInfoQuerie>();

            builder.Services.AddScoped<ILotCardApi, LotCardApiClient>();
            // builder.Services.AddScoped<IHttpApiExecutor, HttpApiExecutor>();

            // 时区偏移量
            builder.Services.AddScoped<ITimezoneContext, TimezoneContext>();

            // cap
            builder.Services.AddMCodeCap();

            // 登录用户jwt信息
            builder.Services.AddScoped<UserHttpContext>();

            // 通讯服务基类
            builder.Services.AddScoped<IotBaseService>();

            // 阿里云发送邮件服务
            builder.Services.AddScoped<IEmailServiceProvider, AliyunEmailService>();

            // 阿里云短信服务
            builder.Services.AddSingleton<IAliyunSmsService, AliyunSmsService>();

            // 支付通用创建订单服务
            builder.Services.AddScoped<ICreateOrderService, CreateOrderService>();

            builder.Services.AddScoped<ExportService>();
            // 文件导入监控服务
            builder.Services.AddKeyedScoped<IDocmentService, LanguageDocmentService>(nameof(DocmentTypeEnum.Language));

            // builder.Services.AddKeyedScoped<IDocmentService, LanguageDocment1Service>(nameof(DocmentTypeEnum.Language1));

            // redis服务
            builder.Services.AddSingleton<IRedisService, RedisService>();

            // 注册 Hangfire 服务器
            builder.Services.AddHangfireServer();

            builder.Services.ConfigureServices(builder.Configuration);

            builder.Services.AddAuthorization();

            #region 定时任务
            builder.Services.AddScoped<SyncAlipayAllApplymentsJob>();
            builder.Services.AddScoped<SyncWechatAllApplymentsTJob>();
            builder.Services.AddScoped<testTask>();
            builder.Services.AddScoped<testTask2>();

            #endregion

        }

        /// <summary>
        /// AddMCodeCap
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

            var logger = ServiceProviderServiceExtensions.GetRequiredService<ILogger<CoffeeMachineDbContext>>(services.BuildServiceProvider());

            if (rabbitMQOptions == null)
            {
                throw new ArgumentNullException("rabbitmq not config.");
            }

            var capJson = ServiceProviderServiceExtensions.GetRequiredService<IConfiguration>(services.BuildServiceProvider()).GetValue<string>(capSection);

            if (string.IsNullOrEmpty(capJson))
            {
                throw new ArgumentException("cap未设置");
            }

            services.AddCap(x =>
            {
                x.UseDashboard();

                //使用RabbitMQ传输
                x.UseRabbitMQ(opt => { opt = rabbitMQOptions; });

                //使用MySQL持久化
                //x.UseMySql(capJson);

                //使用SqlServer持久化
                x.UseSqlServer(capJson);

                //启用数据库分布式锁
                x.UseStorageLock = true;

                // 失败重试次数
                x.FailedRetryCount = 5;

                // 初始重试间隔不宜过长，以便快速处理临时性故障（如网络抖动）
                x.FailedRetryInterval = 5;

                // 消费者线程并行处理消息的线程数,当这个值大于1时，将不能保证消息执行的顺序
                x.ConsumerThreadCount = 1;

                // 发送消息的任务将由.NET线程池并行处理
                x.EnablePublishParallelSend = true;

                x.TopicNamePrefix = "CoffeeMachine";

                x.FailedMessageExpiredAfter = 3600 * 24 * 3;

                // 设置自动清理过期数据的间隔（例如每小时执行一次清理任务）
                x.SucceedMessageExpiredAfter = 3600 * 24 * 1;

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
                 .FromAssemblyOf<CreateOperationLogSubscriber>()
                .AddClasses(c => c.AssignableTo<ICapSubscribe>())
                .AsSelf()
                .WithScopedLifetime());

            services.AddScoped<IPublishService, PublishService>();
            return services;
        }

        /// <summary>
        /// 第三方插件服务配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //配置OSS
            // 1. 配置
            services.Configure<OSSOptions>(configuration.GetSection("OSSOptions"));
            var ossopt = configuration.GetSection("OSSOptions").Get<OSSOptions>();
            // 2. 注入
            if (ossopt != null)
            {
                services.AddOSSService(Enum.GetName(ossopt.Provider), option =>
                {
                    option.Provider = ossopt.Provider;
                    option.Endpoint = ossopt.Endpoint;
                    option.AccessKey = ossopt.AccessKey;
                    option.SecretKey = ossopt.SecretKey;
                    option.CDNAddress = ossopt.CDNAddress;
                    option.IsEnableHttps = ossopt.IsEnableHttps;
                    option.IsEnableCache = ossopt.IsEnableCache;
                });
            }

            // 注册 AutoMapper 服务
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(AutoMapperProfile).Assembly);
            });

            #region SignalR 服务配置
            // 注册 SignalR 服务
            services.AddSignalR();
            services.AddSingleton<ISignalRService, SignalRService>();
            services.AddHostedService<SignalRQueueProcessor>();
            // 配置 Redis 连接管理
            //services.AddSingleton<IConnectionTracker, RedisConnectionTracker>();
            // 从配置文件中读取 Redis 配置并绑定到 RedisConfig 对象
            //services.Configure<RedisConfig>(configuration.GetSection("SignalRConfig:Redis"));
            //services.AddSingleton<IRedisHelper, RedisHelper>();
            // 注册后台服务，用于消息队列处理等
            //services.AddSingleton<IHostedService, SignalRQueueProcessor>();
            #endregion

            // HangFire任务调度
            var hfconstr = configuration.GetSection("HangfireCredentials:HangFireConnection").Value;
            services.AddHangfire(config =>
            {
                // 使用 SQL Server 做 Hangfire 存储
                var options = new SqlServerStorageOptions
                {
                    // 队列轮询间隔（默认15秒，这里设置为1秒更实时，但增加数据库压力）
                    QueuePollInterval = TimeSpan.FromSeconds(1),

                    // 指定批量命令的超时时间（默认30秒）
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),

                    // 指定 SQL Server 的事务隔离级别
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),

                    // 并行度控制：最大队列获取作业的批量大小
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true // 建议开启以提升吞吐量（适合 SQL Server 2012+）
                };

                config.UseSqlServerStorage(hfconstr, options);
            });
            //services.AddHangfire(config =>
            //{
            //    //使用内存做HnagFire存储
            //    //config.UseMemoryStorage(new MemoryStorageOptions
            //    //{
            //    //    JobExpirationCheckInterval = TimeSpan.FromMinutes(30),
            //    //    CountersAggregateInterval = TimeSpan.FromMinutes(5),
            //    //    FetchNextJobTimeout = TimeSpan.FromMinutes(1)
            //    //});

            //    //使用MySql做HnagFire存储MySqlStorageOptions
            //    var options = new MySqlStorageOptions
            //    {
            //        //指定数据库事务的隔离级别。常见的隔离级别包括 ReadUncommitted、ReadCommitted、RepeatableRead 和 Serializable
            //        TransactionIsolationLevel = IsolationLevel.ReadCommitted,
            //        //指定轮询队列作业的时间间隔。这个选项决定 Hangfire 将多长时间检查一次是否有新的作业可以处理。
            //        QueuePollInterval = TimeSpan.FromSeconds(1),
            //        //指定检查并删除过期作业的时间间隔。定期清理过期的作业有助于保持数据库的良好性能。
            //        JobExpirationCheckInterval = TimeSpan.FromHours(1),
            //        //指定将统计数据汇总到数据库的时间间隔。
            //        CountersAggregateInterval = TimeSpan.FromMinutes(5),
            //    };

            //    config.UseStorage(
            //              new MySqlStorage(
            //                  hfconstr, options)
            //              {
            //                  //配置作业过期时间。在这个时间之后，Hangfire 将会清理掉执行过的作业。
            //                  JobExpirationTimeout = TimeSpan.FromDays(1)
            //              });
            //});
        }

        /// <summary>
        /// 启用 Body 重复读功能
        /// </summary>
        /// <remarks>须在 app.UseRouting() 之前注册</remarks>
        /// <param name="app"></param>
        /// <returns><see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder EnableBuffering(this IApplicationBuilder app)
        {
            return app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });
        }
    }
}
