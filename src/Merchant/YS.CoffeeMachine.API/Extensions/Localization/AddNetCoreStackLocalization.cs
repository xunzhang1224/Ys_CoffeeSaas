using Microsoft.Extensions.Localization;
using YS.CoffeeMachine.Localization;

namespace YS.CoffeeMachine.API.Extensions.Localization
{
    /// <summary>
    /// 依赖注入多语言工厂拓展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 依赖注入多语言工厂拓展
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNetCoreStackLocalization(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, RedisStringLocalizerFactory>((ctx) =>
            {
                return new RedisStringLocalizerFactory();
            });

            services.AddScoped<IStringLocalizer, RedisStringLocalizer>((ctx) =>
            {
                return new RedisStringLocalizer();
            });
            return services;
        }
        //public static IMvcBuilder AddAppLocalization(this IMvcBuilder mvcBuilder, Action<LocalizationSettingsOptions> customizeConfigure = null)
        //{
        //    mvcBuilder.Services.AddAppLocalization(customizeConfigure);
        //    LocalizationSettingsOptions localizationSettings = App.GetConfig<LocalizationSettingsOptions>("LocalizationSettings", loadPostConfigure: true);
        //    mvcBuilder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization(delegate (MvcDataAnnotationsLocalizationOptions options)
        //    {
        //        options.DataAnnotationLocalizerProvider = (SubType type, IStringLocalizerFactory factory) => factory.Create(localizationSettings.LanguageFilePrefix, localizationSettings.AssemblyName);
        //    });
        //    return mvcBuilder;
        //}
    }
}
