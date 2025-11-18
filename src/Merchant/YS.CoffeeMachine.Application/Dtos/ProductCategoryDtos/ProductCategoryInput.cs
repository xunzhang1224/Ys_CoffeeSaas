using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.ProductCategoryDtos
{
    /// <summary>
    /// 分类查询入参
    /// </summary>
    public class ProductCategoryInput
    {
        /// <summary>
        /// 类型
        /// </summary>
        public ProductCategoryTypeEnum ProductCategoryType { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string? ProductCategoryName { get; set; } = null;
    }

    /// <summary>
    /// 分类查询入参
    /// </summary>
    public class ProductCategoryPageInput : QueryRequest
    {
        /// <summary>
        /// 类型
        /// </summary>
        public ProductCategoryTypeEnum ProductCategoryType { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string? ProductCategoryName { get; set; } = null;
    }
}