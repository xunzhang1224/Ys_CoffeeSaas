using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.ProductCategoryDtos;

namespace YS.CoffeeMachine.Application.Queries.IProductCategoryQueries
{
    /// <summary>
    /// 商品分类查询接口
    /// </summary>
    public interface IProductCategoryQueries
    {
        /// <summary>
        /// 根据Id获取商品分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductCategoryDto> GetByIdAsync(long id);

        /// <summary>
        /// 获取当前企业所有一级商品分类
        /// </summary>
        /// <returns></returns>
        Task<List<ProductCategoryDto>> GetFirstCategoryListAsync(ProductCategoryInput input);

        /// <summary>
        /// 获取当前企业所有一级商品分类分页列表
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<ProductCategoryDto>> GetFirstCategoryPageListAsync(ProductCategoryPageInput input);

        /// <summary>
        /// 根据父Id获取商品分类列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<List<ProductCategoryDto>> GetProductCategoriesByParentId(long parentId);

        /// <summary>
        /// 根据Id获取商品分类树形结构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ProductCategoryTreeDto>> GetProductCategoriesTreeById(long? id);
    }
}