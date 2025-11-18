using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Compact;
using System.Net;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Platform.API.Extensions.Middleware;
using YSCore.Base.App.Extensions;
using YSCore.Base.Localization.Extensions;

// 必须放在Program.cs最开头！禁止改动位置！
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", false);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("Application", "CoffeeMachine")
    .WriteTo.File(
                path: "logs/errorslog-.txt",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 31)
    //.WriteTo.File(
    //            path: "logs/log-.json",
    //            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
    //            rollingInterval: RollingInterval.Day,
    //            formatter: new CompactJsonFormatter(), // 使用 JSON 格式化程序
    //            retainedFileCountLimit: 31)
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();
try
{
    var builder = WebApplication.CreateBuilder(args).InitYsCore();

    //跨越问题
    builder.Services.AddCors(options =>
    {
        options.AddPolicy
        (name: "myCors",
            b =>
            {
                //b.WithOrigins("http://218.77.110.2:8078", "http://218.77.110.2:8079", "http://csys.ourvend.com:8078", "http://csys.ourvend.com:8079", "http://172.16.66.207:8079")
                b.SetIsOriginAllowed(_ => true)  // 允许任意 Origin
                    .AllowAnyHeader()
                    .AllowAnyMethod()    // 允许所有HTTP方法
                    .AllowAnyMethod();
            }
        );
    });
    builder.Host.UseSerilog();
    // 配置 Kestrel 服务器
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Listen(IPAddress.Any, 5100, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1;
        });
        //Setup a HTTP/2 endpoint without TLS.
        options.Listen(IPAddress.Any, 5101, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });
    });

    builder.Services.AddHttpClient();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        var basePath = AppContext.BaseDirectory;
        var xmlPath = Path.Combine(basePath, "YS.CoffeeMachine.Platform.API.xml");
        c.IncludeXmlComments(xmlPath, true);

        #region 支持Swagger版本控制
        foreach (var field in typeof(ApiManages).GetFields())
        {
            c.SwaggerDoc(field.Name, new OpenApiInfo()
            {
                Title = $"{field.Name}",
                Version = field.Name,
                Description = $"{field.Name} 版本",
            });
        }
        #endregion

        // 添加 Bearer Token 认证支持
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            In = ParameterLocation.Header,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "请输入 Token（可以不填Bearer）"
        });

        // 在 API 请求中启用 Bearer Token 认证
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "Bearer",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                },
                new string[] { }
            }
        });
    });

    builder.Services.AddYsCore();

    // 业务服务
    builder.AddApplicationServices();

    builder.Services.AddSingleton<IStringLocalizerFactory, RedisStringLocalizerFactory>();
    builder.Services.AddScoped<IStringLocalizer, RedisStringLocalizer>();
    // 多语言
    builder.Services.AddAppLocalization();

    builder.Services.AddGrpc();

    //// swagger 显示枚举值字符
    //builder.Services.AddMvc(option => option.EnableEndpointRouting = false).AddJsonOptions(options => {
    //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    //});

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        foreach (var field in typeof(ApiManages).GetFields())
        {
            c.SwaggerEndpoint($"/swagger/{field.Name}/swagger.json", $"{field.Name}");
        }
        //c.SwaggerEndpoint("/swagger/v1/swagger.json", "YS.OpenPlatform.API");
    });
    //}

    //Hangfire Dashboard
    //app.UseHangfireDashboard("/TaskScheduling", new DashboardOptions
    //{
    //    Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
    //        {
    //            SslRedirect=false,
    //            RequireSsl=false,
    //            // 登录的用户名和密码
    //            Users = new[]
    //            {
    //                new BasicAuthAuthorizationUser
    //                {
    //                    Login = app.Configuration.GetSection("HangfireCredentials:UserName").Value, // 用户名
    //                    PasswordClear = app.Configuration.GetSection("HangfireCredentials:Password").Value // 密码
    //                }
    //            }
    //        })}
    //});

    // 多语言中间件
    app.UseAppLocalization();

    app.UseCors("myCors");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<PermissionMiddleware>();
    app.MapControllers();
    //app.UseCap();
    app.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Use a gRPC client to communicate with gRPC endpoints.");
    });

    ////从依赖注入容器中获取 RecurringTaskLoader 实例
    //using (var scope = app.Services.CreateScope()) // 创建服务范围
    //{
    //    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    //    var taskSchedulingInfoQueries = scope.ServiceProvider.GetRequiredService<ITaskSchedulingInfoQueries>();

    //    var taskLoader = new RecurringTaskLoader(recurringJobManager, taskSchedulingInfoQueries);
    //    // 调用方法来加载和调度定时任务
    //    await taskLoader.LoadAndScheduleTasks();
    //}
    foreach (var service in builder.Services)
    {
        Console.WriteLine($"Service: {service.ServiceType.FullName}, Lifetime: {service.Lifetime}");
    }
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
