using NLog;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.Domain.IoT.Extension;
using YS.Domain.IoT.Option;
namespace YS.Domain.IoT.Util;

/// <summary>
/// 项目工具类
/// </summary>
public class ProjectUtil
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 获取环境配置文件路径
    /// </summary>
    /// <returns></returns>
    public static string GetEnvironmentConfigurationPath()
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        Console.WriteLine($"当前运行环境:{environmentName}");
        // var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Configuration");
        var basePath = Directory.GetCurrentDirectory();
        string path = null;
        if (environmentName == "Development")
        {
            path = Path.Combine(basePath, "appsettings.Development.json");
        }
        else if (environmentName == "Staging")
        {
            path = Path.Combine(basePath, "appsettings.Staging.json");
        }
        else if (environmentName == "Production")
        {
            path = Path.Combine(basePath, "appsettings.Production.json");
        }
        else
        {
            path = Path.Combine(basePath, "appsettings.Development.json");
        }
        return path;
    }

    /// <summary>
    /// 存储设备信息
    /// </summary>
    /// <param name="mid"></param>
    /// <returns></returns>
    public static async Task<bool> StoreDeviceInfoAsync(string mid)
    {
        //通过环境变量获取地址
        //string grpcPort = Environment.GetEnvironmentVariable("GRPC_PORT") ?? AppSettingsHelper.GetContent("GrpcHttp2Listen", "Port");

        ////var grpcPort = AppSettingsHelper.GetContent("GrpcHttp2Listen", "Port").ToInt32();
        //var grpcIp = AppSettingsHelper.GetContent("GrpcHttp2Listen", "IP");
        //var tcpServerAddr = AppSettingsHelper.GetContent("SocketListen");
        // 预分配固定大小的数组以提高性能
        object[] keyValuePairs = new object[10]; // 假设有5个字段，每个字段对应一个键和一个值
        var cacheKey = CacheConst.VendChannelKey.ToFormat(mid);
        // 手动填充数组，确保键和值成对出现
        keyValuePairs[0] = "VendCode";
        keyValuePairs[1] = mid;
        keyValuePairs[2] = "GrpcServerAddr";
        keyValuePairs[3] = $"{AppConfigCache.GrpcIp}:{AppConfigCache.GrpcPort}"; // 使用缓存
        keyValuePairs[4] = "TcpServerAddr";
        keyValuePairs[5] = AppConfigCache.TcpServerAddr; // 使用缓存
        keyValuePairs[6] = "lastSeen";
        keyValuePairs[7] = DateTime.Now.ToString("o"); // ISO 8601 格式化时间戳
        keyValuePairs[8] = "OtherInfo";
        keyValuePairs[9] = string.Empty;

        // 调用 HMSetAsync 方法
        var res = await RedisHelper.HMSetAsync(cacheKey, keyValuePairs);
        if (res == false) _logger.Error($"缓存地址写入失败：{cacheKey}");
        return res;
    }

    /// <summary>
    ///  配置缓存类（线程安全且延迟加载）
    /// </summary>
    public static class AppConfigCache
    {
        /// <summary>
        /// GRPC 端口（优先环境变量，其次配置文件）
        /// </summary>
        public static string GrpcPort => _grpcPort.Value;
        private static readonly Lazy<string> _grpcPort = new Lazy<string>(() =>
            Environment.GetEnvironmentVariable("GRPC_PORT") ?? AppSettingsHelper.GetContent("GrpcHttp2Listen", "Port"));

        /// <summary>
        /// GRPC IP 地址
        /// </summary>
        public static string GrpcIp => _grpcIp.Value;
        private static readonly Lazy<string> _grpcIp = new Lazy<string>(() =>
            AppSettingsHelper.GetContent("GrpcHttp2Listen", "IP"));

        /// <summary>
        /// TCP 服务地址
        /// </summary>
        public static string TcpServerAddr => _tcpServerAddr.Value;
        private static readonly Lazy<string> _tcpServerAddr = new Lazy<string>(() =>
            AppSettingsHelper.GetContent("SocketListen"));
    }
}