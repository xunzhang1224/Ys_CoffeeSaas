using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.BeverageWarehouseDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries.IBeverageInfoTemplateQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.BeverageQueries.BeverageInfoTemplateQueries
{
    /// <summary>
    /// 饮品库信息查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class BeverageInfoTemplateQueries(CoffeeMachineDbContext context, IMapper mapper) : IBeverageInfoTemplateQueries
    {
        /// <summary>
        /// 根据Id获取饮品库信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<BeverageInfoTemplateDto> GetBeverageInfoTemplateAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = await context.BeverageInfoTemplate.AsNoTracking().Includes("FormulaInfoTemplates", "BeverageTemplateVersions").FirstOrDefaultAsync(x => x.Id == id);
            var dto = mapper.Map<BeverageInfoTemplateDto>(info);
            //获取设备类型
            var deviceModel = await context.DeviceModel.FirstOrDefaultAsync(w => w.Id == info.DeviceModelId);
            dto.MaterialBoxCount = deviceModel.MaxCassetteCount;
            dto.FormulaInfos = dto.FormulaInfoTemplates;
            dto.FormulaInfos.ForEach(e => { e.MaterialBoxId = e.MaterialBoxId == null ? null : e.MaterialBoxId; });
            dto.FormulaInfoTemplates = [];
            return dto;
        }

        /// <summary>
        /// 根据企业Id获取饮品库列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="enterpriseInfoId"></param>
        /// <param name="formulaType"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<BeverageInfoTemplateDto>> GetBeverageInfoTemplateListAsync(QueryRequest request, long enterpriseInfoId, FormulaTypeEnum? formulaType)
        {
            if (enterpriseInfoId <= 0)
                throw new ArgumentOutOfRangeException(nameof(enterpriseInfoId));
            var query = context.BeverageInfoTemplate.AsQueryable()
                .WhereIf(formulaType != null, w => w.FormulaInfoTemplates.Select(s => s.FormulaType).Contains(formulaType.Value))
                .WhereIf(enterpriseInfoId > 0, w => w.EnterpriseInfoId == enterpriseInfoId);

            var list = await query.OrderByDescending(o => o.CreateTime).ToPagedListAsync(request, "FormulaInfoTemplates", "BeverageTemplateVersions");
            var deviceModelIds = new List<long?>();
            var deviceModels = new Dictionary<long, string>();

            var productCategories = new List<ProductCategory>();
            if (list.TotalCount > 0)
            {
                deviceModelIds = list.Items.Select(s => s.DeviceModelId).Distinct().ToList();
                var curModels = await context.DeviceModel.Where(w => deviceModelIds.Contains(w.Id)).ToListAsync();
                deviceModels = curModels.ToDictionary(s => s.Id, s => s.Name);
                productCategories = await context.ProductCategory.AsQueryable().Where(w => w.ProductCategoryType == ProductCategoryTypeEnum.Beverage).ToListAsync();

            }
            var listDto = mapper.Map<List<BeverageInfoTemplateDto>>(list.Items);
            listDto.ForEach(e => { e.FormulaInfos = e.FormulaInfoTemplates; e.FormulaInfoTemplates = []; e.FormulaInfos.ForEach(e => { e.MaterialBoxId = e.MaterialBoxId == null ? null : e.MaterialBoxId; }); });
            //组装设备类型名称
            if (deviceModels.Count > 0)
                listDto.ForEach(s => s.DeviceModelName = deviceModels[s.DeviceModelId]);

            if (productCategories.Count > 0)
            {
                foreach (var item in listDto)
                {
                    if (item.CategoryIds != null)
                    {
                        productCategories.Where(w => item.CategoryIds.Contains(w.Id)).ToList().ForEach(e => item.CategoryNames += e.Name + "，");
                    }

                }
            }

            PagedResultDto<BeverageInfoTemplateDto> pagedResultDto = new PagedResultDto<BeverageInfoTemplateDto>()
            {
                Items = listDto,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = list.TotalCount
            };
            return pagedResultDto;
        }

        /// <summary>
        /// 根据饮品Id验证SKU是否存在饮品库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool?> VerifySkuByBeverageIdAsync(long id)
        {
            var info = await context.BeverageInfo.FirstOrDefaultAsync(w => w.Id == id);
            if (info == null)
                throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var count = await context.BeverageInfoTemplate.Where(w => !string.IsNullOrWhiteSpace(w.Code) && w.Code == info.Code).CountAsync();
            return count > 0 ? true : null;
        }
    }
}
