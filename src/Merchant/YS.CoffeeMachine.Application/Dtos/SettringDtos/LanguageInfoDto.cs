using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.SettringDtos
{
    /// <summary>
    /// 语言信息
    /// </summary>
    public class LanguageInfoDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum IsEnabled { get; set; }

        /// <summary>
        /// 是否默认（枚举类型：IsDefaultEnum)
        /// </summary>
        public IsDefaultEnum IsDefault { get; set; }
    }
}
