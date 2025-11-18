// YS.K12
using Masuit.Tools;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YS.Domain.IoT.Option;
/// <summary>
/// 获取Appsettings配置信息
/// </summary>
public class AppSettingsHelper
{

    private static IConfiguration Configuration { get; set; }

    // 静态构造函数现在只负责定义如何初始化
    static AppSettingsHelper()
    {
        // 初始化逻辑将在外部调用Initialize方法时执行
    }

    /// <summary>
    /// 初始化AppSettingsHelper
    /// </summary>
    /// <param name="configuration"></param>
    public static void Initialize(IConfiguration configuration)
    {
        Configuration = configuration;

    }

    /// <summary>
    /// 获取配置项内容
    /// </summary>
    /// <param name="sections">节点配置</param>
    /// <returns>配置项值</returns>
    public static string GetContent(params string[] sections)
    {
        if (sections == null || sections.Length == 0) return "";

        var key = string.Join(":", sections);
        return Configuration[key];
    }
    /// <summary>
    /// 获取并反序列化复杂类型配置项
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="sections">节点配置</param>
    /// <returns>反序列化后的对象</returns>
    public static T GetComplexContent<T>(string sections)
    {
        return Configuration.GetSection(sections).Get<T>();
    }
    /// <summary>
    /// 设置配置项内容到指定文件中（注意：这会覆盖原有文件）
    /// </summary>
    /// <param name="value">要设置的值</param>
    /// <param name="sections">节点配置</param>
    /// <returns>是否成功设置</returns>
    public static bool SetContent(string value, params string[] sections)
    {
        try
        {
            // 注意: 修改配置文件的行为在生产环境中是不推荐的。
            // 更好的做法是通过部署过程或者环境变量来更改配置。
            string contentPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(contentPath, "appsettings.json");

            JObject jsonObject;
            using (StreamReader file = new StreamReader(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                jsonObject = (JObject)JToken.ReadFrom(reader);
                JToken token = jsonObject;
                foreach (var section in sections.Take(sections.Length - 1))
                {
                    token = token[section];
                }
                token[sections.Last()] = value;
            }

            using (var writer = new StreamWriter(path))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                jsonObject.WriteTo(jsonWriter);
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

}
