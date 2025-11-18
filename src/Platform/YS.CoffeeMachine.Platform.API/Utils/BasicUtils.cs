using Newtonsoft.Json;
using System.Reflection;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Localization;
using YSCore.Base.DatabaseAccessor;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Utils
{
    /// <summary>
    /// BasicUtils
    /// </summary>
    public static class BasicUtils
    {
        /// <summary>
        /// 检查LanguageInfo实体数据
        /// </summary>
        /// <param name="isDefault"></param>
        /// <param name="enabled"></param>
        public static void CheckLanguageInfo(IsDefaultEnum isDefault, EnabledEnum enabled)
        {
            if (enabled == EnabledEnum.Disable && isDefault == IsDefaultEnum.Yes)
            {
                //var _localizer = AppHelper.RootServices.GetService<IStringLocalizer>();
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0001)]);
            }
        }

        /// <summary>
        /// 比较两个实体，得到差异属性与值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1"></param>
        /// <param name="object2">新</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Dictionary<string, string> GetDifferentProperties<T>(T object1, T object2) where T : class
        {
            if (object1 == null || object2 == null)
                throw new ArgumentException("The objects to compare cannot be null.");

            var differentProperties = new Dictionary<string, string>();
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] removes = typeof(BaseEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                var value1 = property.GetValue(object1);
                var value2 = property.GetValue(object2);
                bool isBase = false;
                foreach (var r in removes)
                {
                    if (r.Name == property.Name)
                        isBase = true;
                }
                if (!Equals(value1, value2) && !isBase)
                {
                    differentProperties.Add(property.Name, JsonConvert.SerializeObject(value2));
                }
            }

            return differentProperties;
        }
    }
}

