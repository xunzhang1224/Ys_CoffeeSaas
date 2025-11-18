using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Tools
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举值对应的描述，如果没有 Description 特性则返回枚举值的名称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionOrValue(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.ToString();
        }

        /// <summary>
        /// 根据位运算，获取包含的枚举状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetDescriptionsByInt<TEnum>(int status) where TEnum : Enum
        {
            var descriptions = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Where(statusEnum => Convert.ToInt32(statusEnum) != 0 && (status & Convert.ToInt32(statusEnum)) == Convert.ToInt32(statusEnum))
                .Select(statusEnum => statusEnum.GetType()
                    .GetField(statusEnum.ToString())
                    .GetCustomAttribute<DescriptionAttribute>()
                    ?.Description)
                .ToList();

            return string.Join("、", descriptions);
        }
    }
}
