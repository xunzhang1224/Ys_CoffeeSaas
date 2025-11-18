using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.SettringDtos
{
    /// <summary>
    /// 多语言详情列表查询入参
    /// </summary>
    public class LanguageTextQuery : QueryRequest
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string? Code { get; set; } = null;

        /// <summary>
        /// 语种
        /// </summary>
        public string? LangCode { get; set; } = null;

        /// <summary>
        /// 详情
        /// </summary>
        public string? Text { get; set; } = null;
    }

    /// <summary>
    /// 语言dto
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
