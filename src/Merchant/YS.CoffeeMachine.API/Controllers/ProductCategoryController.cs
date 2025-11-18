using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.ProductCategoryCommands;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.ProductCategoryDtos;
using YS.CoffeeMachine.Application.Queries.IProductCategoryQueries;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 分类管理
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="productCategoryQueries"></param>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class ProductCategoryController(IMediator mediator, IProductCategoryQueries productCategoryQueries) : Controller
    {
        /// <summary>
        /// 创建商品分类
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateProductCategory")]
        public async Task<bool> CreateProductCategoryAsync([FromBody] CreateProductCategoryCommand command) => await mediator.Send(command);

        /// <summary>
        /// 设置父级分类
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("ChangeParentId")]
        public async Task<bool> ChangeParentIdAsync([FromBody] ChangeParentIdCommand command) => await mediator.Send(command);

        /// <summary>
        /// 编辑商品分类
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateProductCategory")]
        public async Task<bool> UpdateProductCategoryAsync([FromBody] UpdateProductCategoryCommand command) => await mediator.Send(command);

        /// <summary>
        /// 变更商品分类启用禁用状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("ChangeProductCategoryStatus")]
        public async Task<bool> ChangeProductCategoryStatusAsync([FromBody] ChangeProductCategoryStatusCommand command) => await mediator.Send(command);

        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProductCategory")]
        public async Task<bool> DeleteProductCategoryAsync([FromBody] DeleteProductCategoryCommand command) => await mediator.Send(command);

        /// <summary>
        /// 根据Id获取商品分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProductCategoryById")]
        public async Task<ProductCategoryDto> GetByIdAsync([FromQuery] long id) => await productCategoryQueries.GetByIdAsync(id);

        /// <summary>
        /// 获取当前企业所有一级商品分类
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetProductCategoryList")]
        public async Task<List<ProductCategoryDto>> GetFirstCategoryListAsync([FromBody] ProductCategoryInput input) => await productCategoryQueries.GetFirstCategoryListAsync(input);

        /// <summary>
        /// 获取当前企业所有一级商品分类分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetFirstCategoryPageList")]
        public async Task<PagedResultDto<ProductCategoryDto>> GetFirstCategoryPageListAsync(ProductCategoryPageInput input) => await productCategoryQueries.GetFirstCategoryPageListAsync(input);

        /// <summary>
        /// 根据父Id获取商品分类列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet("GetProductCategoriesByParentId")]
        public async Task<List<ProductCategoryDto>> GetProductCategoriesByParentIdAsync([FromQuery] long parentId) => await productCategoryQueries.GetProductCategoriesByParentId(parentId);

        /// <summary>
        /// 根据Id获取商品分类树形结构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProductCategoriesTreeById")]
        public async Task<List<ProductCategoryTreeDto>> GetProductCategoriesTreeByIdAsync([FromQuery] long? id) => await productCategoryQueries.GetProductCategoriesTreeById(id);
    }
}