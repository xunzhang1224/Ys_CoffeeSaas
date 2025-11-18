namespace YS.CoffeeMachine.Iot.Api
{
    /// <summary>
    /// /
    /// </summary>
    public class ProjectUtil
    {
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

            //path = Path.Combine(basePath, "appsettings.json");
            Console.WriteLine($"当前配置路径:{path} aaaa");
            return path;
        }
    }
}
