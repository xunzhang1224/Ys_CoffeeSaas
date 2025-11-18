using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using YS.CoffeeMachine.API.Extensions.Cap.Aop;
using YS.CoffeeMachine.API.Queries.FileResourceQueries;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries;
using YS.CoffeeMachine.Application.PlatformQueries.IBeverageQueries;
using YS.CoffeeMachine.Application.PlatformQueries.IBeverageQueries.IBeverageInfoQueries;
using YS.CoffeeMachine.Application.PlatformQueries.ICurrencyQueries;
using YS.CoffeeMachine.Application.PlatformQueries.IDevicesQueries;
using YS.CoffeeMachine.Application.PlatformQueries.IFaultCodeInfoQueries;
using YS.CoffeeMachine.Application.PlatformQueries.IFileResourceQueries;
using YS.CoffeeMachine.Application.PlatformQueries.IPaymentQueries;
using YS.CoffeeMachine.Application.PlatformQueries.StrategyQueries;
using YS.CoffeeMachine.Application.Queries.BasicQueries;
using YS.CoffeeMachine.Application.Queries.BasicQueries.Language;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries;
using YS.CoffeeMachine.Application.Queries.ICountryAndRegionQueries;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Application.Queries.IOrderQueries;
using YS.CoffeeMachine.Application.Queries.IServiceProviderInfoQueries;
using YS.CoffeeMachine.Application.Queries.IServiceQueries;
using YS.CoffeeMachine.Application.Queries.ITaskSchedulingInfoQueries;
using YS.CoffeeMachine.Cap;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Platform.API.Application.Services;
using YS.CoffeeMachine.Platform.API.Application.Services.Basic.Docment;
using YS.CoffeeMachine.Platform.API.Application.Validations.TaskSchedulingInfoCommandValidators;
using YS.CoffeeMachine.Platform.API.Controllers.Aop;
using YS.CoffeeMachine.Platform.API.Extensions.Cap;
using YS.CoffeeMachine.Platform.API.Extensions.IExecl;
using YS.CoffeeMachine.Platform.API.PlatformQueries.ApplicationInfoQueries;
using YS.CoffeeMachine.Platform.API.PlatformQueries.BasicQueries.Language;
using YS.CoffeeMachine.Platform.API.PlatformQueries.BeverageQueries;
using YS.CoffeeMachine.Platform.API.PlatformQueries.BeverageQueries.BeverageInfoQueries;
using YS.CoffeeMachine.Platform.API.PlatformQueries.CurrencyQueries;
using YS.CoffeeMachine.Platform.API.PlatformQueries.DevicesQueries;
using YS.CoffeeMachine.Platform.API.PlatformQueries.FileResourceQueries;
using YS.CoffeeMachine.Platform.API.PlatformQueries.OrderQueries;
using YS.CoffeeMachine.Platform.API.PlatformQueries.PaymenQueries;
using YS.CoffeeMachine.Platform.API.PlatformQueries.StrategyQueries;
using YS.CoffeeMachine.Platform.API.Queries.CountryAndRegionQueries;
using YS.CoffeeMachine.Platform.API.Queries.DeviceBase;
using YS.CoffeeMachine.Platform.API.Queries.Dictionary;
using YS.CoffeeMachine.Platform.API.Queries.FaultCodeInfoQueries;
using YS.CoffeeMachine.Platform.API.Queries.IOperationLog;
using YS.CoffeeMachine.Platform.API.Queries.OperationLog;
using YS.CoffeeMachine.Platform.API.Queries.ServiceProviderQueries;
using YS.CoffeeMachine.Platform.API.Queries.ServiceQueries;
using YS.CoffeeMachine.Platform.API.Queries.TaskSchedulingInfoQueries;
using YS.CoffeeMachine.Platform.API.Services;
using YS.CoffeeMachine.Platform.API.Services.EmailServices;
using YS.CoffeeMachine.Platform.API.Services.SmsServices;
using YS.CoffeeMachine.Provider;
using YS.CoffeeMachine.Provider.IServices;
using YS.Provider.OSS;
using YS.Provider.Snowflake;
using YSCore.Base.DataValidation.Extensions;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Platform.API.Extensions
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

            //通过命名空间注入验证
            builder.Services.AddValidatorsFromAssembly(typeof(CreateTaskSchedulingInfoCommandValidators).Assembly);

            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

            builder.Services.AddFreeRedis();
            //builder.Services.AddRedis();
            builder.Services.AddMemoryCache();
            // 文件服务配置
            builder.Services.AddFileExcel();
            builder.Services.AddHostedService<FileUploadBackgroundService>();

            builder.Services.AddScoped<DeviceDownSeedActionFilter>();

            builder.Services.AddDbContext<CoffeeMachineTimescaleDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("TimescaleDb")));
            // 配置连接字符串 分表配置
            var constr = builder.Configuration.GetConnectionString("DefaultConnection");
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

            builder.Services.AddDbContext<CoffeeMachinePlatformDbContext>(options =>
            {
                //使用MySql
                //options.UseMySql(constr, ServerVersion.AutoDetect(constr), builder =>
                //{
                //    builder.UseRelationalNulls();
                //});

                //使用SqlServer
                options.UseSqlServer(constr);

                options.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

            builder.Services.AddRepositories(typeof(CoffeeMachinePlatformDbContext).Assembly);
            // 事务
            builder.Services.AddUnitOfWork<CoffeeMachinePlatformDbContext>();

            //builder.Services.AddInnerDependencyInjection(new[] { typeof(TaskSchedulingInfoQueries).Assembly });

            //查询相关依赖注入
            builder.Services.AddScoped<ITaskSchedulingInfoQueries, TaskSchedulingInfoQueries>();//任务调度查询
            builder.Services.AddScoped<IEnumHelper, EnumHelper>();//枚举信息查询
            builder.Services.AddScoped<IOperationLogQueries, OperationLogQueries>();// 操作日志
            builder.Services.AddScoped<IEnumHelperQueries, EnumHelperQueries>();//枚举信息查询
            builder.Services.AddScoped<IFormulaInfosQueries, FormulaInfosQueries>();//饮品配方参数信息查询

            builder.Services.AddScoped<IotBaseService>();
            // 发送邮件服务
            builder.Services.AddScoped<IEmailServiceProvider, AliyunEmailService>();
            // 阿里云短信服务
            builder.Services.AddSingleton<IAliyunSmsService, AliyunSmsService>();
            // 登录用户jwt信息
            builder.Services.AddScoped<UserHttpContext>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IFileCenterQuerie, FileCenterQuerie>();
            builder.Services.AddScoped<ExportService>();
            //文件导入键控服务
            builder.Services.AddKeyedScoped<IDocmentService, LanguageDocmentService>(nameof(DocmentTypeEnum.Language));
            //builder.Services.AddKeyedScoped<IDocmentService, LanguageDocment1Service>(nameof(DocmentTypeEnum.Language1));

            // redis服务
            builder.Services.AddSingleton<IRedisService, RedisService>();
            builder.Services.AddScoped<IsOnlineActionFilter>();

            #region 平台端相关
            builder.Services.AddScoped<IServiceProviderInfoQueries, ServiceProviderInfoQueries>();//服务商信息查询
            builder.Services.AddScoped<IP_EnterpriseTypesQueries, P_EnterpriseTypesQueries>();//企业类型查询
            builder.Services.AddScoped<IP_EnterpriseInfoQueries, P_EnterpriseInfoQueries>();//企业信息查询
            builder.Services.AddScoped<IP_ApplicationRoleQueries, P_ApplicationRoleQueries>();//角色信息查询
            builder.Services.AddScoped<IP_ApplicationMenuQueries, P_ApplicationMenuQueries>();//菜单信息查询
            builder.Services.AddScoped<IP_ApplicationUserQueries, P_ApplicationUserQueries>();//用户信息查询
            builder.Services.AddScoped<IP_DeviceAllocationQueries, P_DeviceAllocationQueries>();//设备分配信息查询
            builder.Services.AddScoped<IDeviceInfoQueries, DeviceInfoQueries>();//设备信息查询
            builder.Services.AddScoped<IP_DeviceModelQueries, P_DeviceModelQueries>();//设备型号信息查询
            builder.Services.AddScoped<IP_BeverageInfoQueries, P_BeverageInfoQueries>();//饮品信息查询
            builder.Services.AddScoped<ILanguageInfoQueries, LanguageInfoQueries>();//多语言查询
            builder.Services.AddScoped<IDictionaryQueries, DictionaryQueries>();//字典查询
            builder.Services.AddScoped<IBeverageCollectionQueries, P_BeverageCollectionQueries>();//饮品集合查询
            builder.Services.AddScoped<ICurrencyInfoQueries, CurrencyInfoQueries>(); //币种查询
            builder.Services.AddScoped<IStrategyQueries, StrategyQueries>();  //地区关联 查询
            builder.Services.AddScoped<IDeviceBaseQueries, DeviceBaseQueries>();
            builder.Services.AddScoped<ICountryInfoQueries, CountryInfoQueries>();
            builder.Services.AddScoped<IOrderInfoQueries, OrderInfoQueries>(); // 订单信息查询
            builder.Services.AddScoped<IPFileManageQueries, PFileManageQueries>(); // 资源文件查询
            builder.Services.AddScoped<IFaultCodeInfoQueries, FaultCodeInfoQueries>(); // 故障代码查询
            #endregion

            #region 支付相关
            builder.Services.AddScoped<IPaymentConfigQueries, PaymentConfigQueries>();  //地区关联 查询
            #endregion

            //builder.Services.AddCap(x =>
            //{
            //    x.UseEntityFramework<CoffeeMachinePlatformDbContext>();
            //    x.UseRabbitMQ(p => builder.Configuration.GetSection("RabbitMQ").Bind(p));
            //    x.UseDashboard(); //CAP Dashboard  path：  /cap

            //    //x.Version = "openapi-v1";
            //});

            builder.Services.AddMCodeCap();
            // 注册 Hangfire 服务器
            //builder.Services.AddHangfireServer();

            builder.Services.ConfigureServices(builder.Configuration);

            builder.Services.AddAuthorization();
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

                x.FailedMessageExpiredAfter = 3600 * 24 * 3;

                // 设置自动清理过期数据的间隔（例如每小时执行一次清理任务）
                x.SucceedMessageExpiredAfter = 3600 * 24 * 1;

                // 失败重试次数
                x.FailedRetryCount = 5;

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
                 .FromAssemblyOf<OperationLogSubscriber>()
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

            //注入AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //HangFire任务调度
            //var hfconstr = configuration.GetSection("HangfireCredentials:HangFireConnection").Value;
            //services.AddHangfire(config =>
            //{
            //    //使用内存做HnagFire存储
            //    //config.UseMemoryStorage(new MemoryStorageOptions
            //    //{
            //    //    JobExpirationCheckInterval = TimeSpan.FromMinutes(30),
            //    //    CountersAggregateInterval = TimeSpan.FromMinutes(5),
            //    //    FetchNextJobTimeout = TimeSpan.FromMinutes(1)
            //    //});

            //    //使用MySql做HnagFire存储
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
    }
}
