using Microsoft.Extensions.Localization;

namespace YS.CoffeeMachine.Localization
{
    /// <summary>
    /// Redis多语言工厂
    /// </summary>
    public class RedisStringLocalizerFactory : IStringLocalizerFactory
    {
        /// <summary>
        /// 扩展IStringLocalizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            return new RedisStringLocalizer();
        }

        /// <summary>
        /// 扩展IStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            return new RedisStringLocalizer();
        }
    }
}
