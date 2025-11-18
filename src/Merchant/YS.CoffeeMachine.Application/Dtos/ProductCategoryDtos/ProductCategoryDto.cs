using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.ProductCategoryDtos
{
    /// <summary>
    /// 商品分类树形结构Dto
    /// </summary>
    public class ProductCategoryTreeDto : ProductCategoryDto
    {
        /// <summary>
        /// 商品分类子节点
        /// </summary>
        public List<ProductCategoryTreeDto> Children { get; set; } = new List<ProductCategoryTreeDto>();
    }

    /// <summary>
    /// 商品分类Dto
    /// </summary>
    public class ProductCategoryDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父级分类Id
        /// </summary>
        public long? ParentId { get; set; } = null;

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类图片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 分类图标
        /// </summary>
        public string? IconUrl { get; set; } = null;

        /// <summary>
        /// 类型
        /// </summary>
        public ProductCategoryTypeEnum ProductCategoryType { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public IsEnabledEnum IsEnabled { get; set; }

        /// <summary>
        /// 分类层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}