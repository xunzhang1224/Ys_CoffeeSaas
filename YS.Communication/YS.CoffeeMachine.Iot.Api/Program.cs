using Autofac;
using Autofac.Extensions.DependencyInjection;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Iot.Api;
using YS.CoffeeMachine.Iot.Api.Extensions;
using YS.CoffeeMachine.Iot.Api.Extensions.Autofac;
using YS.CoffeeMachine.Iot.Api.Extensions.BackTask;
using YS.CoffeeMachine.Iot.Api.Iot.CommandHandler;
using YS.CoffeeMachine.Iot.Api.Services;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands;
using YS.CoffeeMachine.Platform.API.Extensions.TaskSchedulingBase.Wrapper;
using YS.CoffeeMachine.Provider;
using YS.Provider.Snowflake;
using YSCore.Base.DataValidation.Extensions;
using YSCore.Provider.EntityFrameworkCore.Extensions;

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
    .WriteTo.File(
                path: "logs/log-.json",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 31)
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(ProjectUtil.GetEnvironmentConfigurationPath(), optional: true, reloadOnChange: true);
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 6200, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
    //Setup a HTTP/2 endpoint without TLS.
    options.Listen(IPAddress.Any, 6100, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
// 添加控制器服务
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<DeviceOfflineTask>();
//雪花id
SnowflakeOptions? SnowflakeOption = builder.Configuration.GetSection("SnowId").Get<SnowflakeOptions>();
builder.Services.AddSnowflake(options =>
{
    options = SnowflakeOption;
});

// 业务服务（cap,rabbitmq）
builder.AddApplicationServices();

builder.Services.AddScoped<IGrpcCommandService, CommandService>();
//builder.Services.Configure<RabbitMQOptions>(
//    builder.Configuration.GetSection("rabbitmq"));

//增加Grpc
builder.Services.AddGrpc();
builder.Services.AddMagicOnion();
builder.Services.AddFreeRedis();
// 登录用户jwt信息
//builder.Services.AddScoped<ICommandSenderService, CommandSender>();
builder.Services.AddScoped<GrpcClientPool>();
builder.Services.AddScoped<GrpcClusterIotWrapp>();
builder.Services.AddScoped<UserHttpContext>();
builder.Services.AddScoped<CommonHelper>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
    cfg.AddDataValidationBehavior()
    .AddUnitOfWorkBehaviors();
});
// 配置连接字符串 分表配置
var constr = builder.Configuration.GetConnectionString("DefaultConnection");
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
// 配置连接字符串 分表配置

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

builder.Services.AddDbContext<CoffeeMachineTimescaleDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("TimescaleDb")));

builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<AutofacRegister>();
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // 推荐启用
});

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

var rabbitOptions = app.Services.GetRequiredService<IOptions<RabbitMQOptions>>().Value;
Console.WriteLine("RabbitMQ Host: " + rabbitOptions.HostName);

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
app.MapMagicOnionService();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    /// <summary>
    /// TemperatureF
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

/// <summary>
/// 自定义日期时间转换器
/// </summary>
public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string[] _formats = new[]
    {
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-dd"
    };

    /// <summary>
    /// a
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var dateString = reader.GetString();
            foreach (var format in _formats)
            {
                if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }
            // 最后尝试通用解析
            if (DateTime.TryParse(dateString, out DateTime defaultResult))
            {
                return defaultResult;
            }
        }
        throw new JsonException($"无法解析日期时间: {reader.GetString()}");
    }

    /// <summary>
    /// a
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
    }
}