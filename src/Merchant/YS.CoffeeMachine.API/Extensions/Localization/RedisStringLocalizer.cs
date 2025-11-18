using Microsoft.Extensions.Localization;
using System.Globalization;
using YS.CoffeeMachine.Application.Queries.BasicQueries.Language;
using YSCore.Base.App;

namespace YS.CoffeeMachine.Localization
{
    /// <summary>
    /// 获取多语言文本
    /// </summary>
    public class RedisStringLocalizer : IStringLocalizer
    {
        private readonly IServiceScopeFactory _scopeFactory = AppHelper.RootServices.GetRequiredService<IServiceScopeFactory>();
        //private readonly ILanguageInfoQueries _queries = AppHelper.RootServices.GetRequiredService<ILanguageInfoQueries>();
        //private readonly ILanguageInfoQueries _queries = AppHelper.RootServices.GetRequiredService<ILanguageInfoQueries>();

        /// <summary>
        /// 实现多语言工厂方法 L.Text["key"]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                if (string.IsNullOrWhiteSpace(value))
                    value = name;
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        /// <summary>
        /// 实现多语言工厂方法 L.Text["key", "params"]
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                if (string.IsNullOrWhiteSpace(format))
                    format = name;
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        /// <summary>
        /// 设置默认语言
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            return new RedisStringLocalizer();
        }

        /// <summary>
        /// 获取所有文本
        /// </summary>
        /// <param name="includeAncestorCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
        {
            using var serviceScope = _scopeFactory.CreateScope();
            var _queries = serviceScope.ServiceProvider.GetRequiredService<ILanguageInfoQueries>();
            var datas = _queries.GetLanguageTextsAsync().GetAwaiter().GetResult();
            return GetLocalizedStrings(datas);
        }

        private IEnumerable<LocalizedString> GetLocalizedStrings(Dictionary<string, string> dic)
        {
            foreach (var item in dic)
            {
                yield return new LocalizedString(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 根据code获取当前语言文本
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetString(string code)
        {
            using var serviceScope = _scopeFactory.CreateScope();
            var _queries = serviceScope.ServiceProvider.GetRequiredService<ILanguageInfoQueries>();
            if (string.IsNullOrWhiteSpace(code))
                return string.Empty;
            return _queries.GetLangTextByCodeAsync(code).GetAwaiter().GetResult();
        }
    }
}
