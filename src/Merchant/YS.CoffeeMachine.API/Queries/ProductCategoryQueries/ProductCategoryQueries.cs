using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.ProductCategoryDtos;
using YS.CoffeeMachine.Application.Queries.IProductCategoryQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YS.CoffeeMachine.Infrastructure;
using YS.Pay.SDK;

namespace YS.CoffeeMachine.API.Queries.ProductCategoryQueries
{
    /// <summary>
    /// 商品分类查询
    /// </summary>
    public class ProductCategoryQueries(CoffeeMachineDbContext context) : IProductCategoryQueries
    {
        /// <summary>
        /// 根据Id获取商品分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductCategoryDto> GetByIdAsync(long id)
        {
            return await context.ProductCategory.AsQueryable()
                .Where(w => w.Id == id)
                .Select(s => new ProductCategoryDto
                {
                    Id = s.Id,
                    ParentId = s.ParentId,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    IconUrl = s.IconUrl,
                    ProductCategoryType = s.ProductCategoryType,
                    IsEnabled = s.IsEnabled,
                    Level = s.Level,
                    Sort = s.Sort,
                    Description = s.Description
                }).FirstOrDefaultAsync() ?? new ProductCategoryDto();
        }

        /// <summary>
        /// 获取当前企业所有一级商品分类
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductCategoryDto>> GetFirstCategoryListAsync(ProductCategoryInput input)
        {
            return await context.ProductCategory.AsQueryable()
                .Where(w => w.ParentId == null && w.IsEnabled == IsEnabledEnum.Enabled && w.ProductCategoryType == input.ProductCategoryType)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryName), w => w.Name.Contains(input.ProductCategoryName!))
                .Select(s => new ProductCategoryDto
                {
                    Id = s.Id,
                    ParentId = s.ParentId,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    IconUrl = s.IconUrl,
                    ProductCategoryType = s.ProductCategoryType,
                    IsEnabled = s.IsEnabled,
                    Level = s.Level,
                    Sort = s.Sort,
                    Description = s.Description
                })
                .OrderBy(o => o.Sort)
                .ToListAsync();
        }

        /// <summary>
        /// 获取当前企业所有一级商品分类分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductCategoryDto>> GetFirstCategoryPageListAsync(ProductCategoryPageInput input)
        {
            return await context.ProductCategory.AsQueryable()
                .Where(w => w.ParentId == null && w.ProductCategoryType == input.ProductCategoryType)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryName), w => w.Name.Contains(input.ProductCategoryName!))
                .Select(s => new ProductCategoryDto
                {
                    Id = s.Id,
                    ParentId = s.ParentId,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    IconUrl = s.IconUrl,
                    ProductCategoryType = s.ProductCategoryType,
                    IsEnabled = s.IsEnabled,
                    Level = s.Level,
                    Sort = s.Sort,
                    Description = s.Description
                })
                .OrderBy(o => o.Sort)
                .ToPagedListAsync(input);
        }

        /// <summary>
        /// 根据父Id获取商品分类列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<ProductCategoryDto>> GetProductCategoriesByParentId(long parentId)
        {
            return await context.ProductCategory.AsQueryable()
                .Where(w => w.ParentId == parentId && w.IsEnabled == IsEnabledEnum.Enabled)
                .Select(s => new ProductCategoryDto
                {
                    Id = s.Id,
                    ParentId = s.ParentId,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    IconUrl = s.IconUrl,
                    ProductCategoryType = s.ProductCategoryType,
                    IsEnabled = s.IsEnabled,
                    Level = s.Level,
                    Sort = s.Sort,
                    Description = s.Description
                })
                .OrderBy(o => o.Sort)
                .ToListAsync();
        }

        /// <summary>
        /// 根据Id获取商品分类树形结构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ProductCategoryTreeDto>> GetProductCategoriesTreeById(long? id)
        {
            var allCategories = await context.ProductCategory
                .Select(c => new ProductCategoryTreeDto
                {
                    Id = c.Id,
                    ParentId = c.ParentId,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    IconUrl = c.IconUrl,
                    ProductCategoryType = c.ProductCategoryType,
                    IsEnabled = c.IsEnabled,
                    Level = c.Level,
                    Sort = c.Sort,
                    Description = c.Description
                })
                .OrderBy(c => c.Sort)
                .ToListAsync();

            var roots = id == null
                ? allCategories.Where(c => c.ParentId == null).ToList()
                : allCategories.Where(c => c.ParentId == id).ToList();

            BuildTree(roots, allCategories);

            return roots;
        }

        /// <summary>
        /// 递归组装树形结构
        /// </summary>
        /// <param name="parents"></param>
        /// <param name="all"></param>
        private void BuildTree(List<ProductCategoryTreeDto> parents, List<ProductCategoryTreeDto> all)
        {
            foreach (var parent in parents)
            {
                var children = all.Where(c => c.ParentId == parent.Id).ToList();
                if (children.Any())
                {
                    parent.Children = children;
                    BuildTree(children, all);
                }
            }
        }
    }
}
