using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Beverages
{
    /// <summary>
    /// 商品分类
    /// </summary>
    public class ProductCategory : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 父级分类Id
        /// </summary>
        public long? ParentId { get; private set; } = null;

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 分类图片
        /// </summary>
        public string ImageUrl { get; private set; }

        /// <summary>
        /// 分类图标
        /// </summary>
        public string? IconUrl { get; private set; } = null;

        /// <summary>
        /// 类型
        /// </summary>
        public ProductCategoryTypeEnum ProductCategoryType { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public IsEnabledEnum IsEnabled { get; private set; }

        /// <summary>
        /// 分类层级
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 保护构造函数
        /// </summary>
        protected ProductCategory() { }

        /// <summary>
        /// 添加商品分类
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="name"></param>
        /// <param name="imageUrl"></param>
        /// <param name="iconUrl"></param>
        /// <param name="productCategoryType"></param>
        /// <param name="isEnabled"></param>
        /// <param name="sort"></param>
        /// <param name="description"></param>
        public ProductCategory(long? parentId, string name, string imageUrl, string? iconUrl, ProductCategoryTypeEnum productCategoryType, IsEnabledEnum isEnabled, int sort, string description)
        {
            ParentId = parentId;
            Name = name;
            ImageUrl = imageUrl;
            IconUrl = iconUrl;
            ProductCategoryType = productCategoryType;
            IsEnabled = isEnabled;
            Sort = sort;
            Description = description;
        }

        /// <summary>
        /// 设置父级分类Id
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="level"></param>
        public void SetParentId(long? parentId, int level)
        {
            ParentId = parentId;
            Level = level;
        }

        /// <summary>
        /// 修改商品分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="imageUrl"></param>
        /// <param name="iconUrl"></param>
        /// <param name="productCategoryType"></param>
        /// <param name="isEnabled"></param>
        /// <param name="sort"></param>
        /// <param name="description"></param>
        public void Update(string name, string imageUrl, string? iconUrl, ProductCategoryTypeEnum productCategoryType, IsEnabledEnum isEnabled, int sort, string description)
        {
            Name = name;
            ImageUrl = imageUrl;
            IconUrl = iconUrl;
            ProductCategoryType = productCategoryType;
            IsEnabled = isEnabled;
            Sort = sort;
            Description = description;
        }

        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="isEnabled"></param>
        public void SetIsEnabled(IsEnabledEnum isEnabled)
        {
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// 设置分类类型
        /// </summary>
        /// <param name="productCategoryType"></param>
        public void SetCategoryType(ProductCategoryTypeEnum productCategoryType)
        {
            ProductCategoryType = productCategoryType;
        }
    }
}