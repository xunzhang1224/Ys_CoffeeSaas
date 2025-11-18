using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 商品分类类型
    /// </summary>
    public enum ProductCategoryTypeEnum
    {
        /// <summary>
        /// 配方饮品
        /// </summary>
        [Description("配方饮品")]
        Beverage = 1,

        /// <summary>
        /// 副柜商品
        /// </summary>
        [Description("副柜商品")]
        Commodity = 2
    }
}