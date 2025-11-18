using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.ProductCategoryCommands
{
    /// <summary>
    /// 创建商品分类
    /// </summary>
    public record CreateProductCategoryCommand(long? parentId, string name, string imageUrl, string? iconUrl, ProductCategoryTypeEnum productCategoryType, IsEnabledEnum isEnabled, int sort, string description) : ICommand<bool>;

    /// <summary>
    /// 设置父级分类
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parentId"></param>
    public record ChangeParentIdCommand(long id, long? parentId) : ICommand<bool>;

    /// <summary>
    /// 编辑商品分类
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="imageUrl"></param>
    /// <param name="iconUrl"></param>
    /// <param name="productCategoryType"></param>
    /// <param name="isEnabled"></param>
    /// <param name="sort"></param>
    /// <param name="description"></param>
    public record UpdateProductCategoryCommand(long id, string name, string imageUrl, string? iconUrl, ProductCategoryTypeEnum productCategoryType, IsEnabledEnum isEnabled, int sort, string description) : ICommand<bool>;

    /// <summary>
    /// 变更商品分类启用禁用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isEnabled"></param>
    public record ChangeProductCategoryStatusCommand(long id, IsEnabledEnum isEnabled) : ICommand<bool>;

    /// <summary>
    /// 删除商品分类
    /// </summary>
    /// <param name="id"></param>
    public record DeleteProductCategoryCommand(long id) : ICommand<bool>;
}