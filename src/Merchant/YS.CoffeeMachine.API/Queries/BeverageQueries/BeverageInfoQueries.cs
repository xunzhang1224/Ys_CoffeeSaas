using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula;
using System.Diagnostics;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.Util.Core;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.BeverageQueries
{
    /// <summary>
    /// 饮品查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class BeverageInfoQueries(CoffeeMachineDbContext context, IMapper mapper) : IBeverageInfoQueries
    {
        /// <summary>
        /// 根据Id获取饮品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<BeverageInfoDto> GetBeverageInfoAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = await context.BeverageInfo.AsNoTracking().Includes("FormulaInfos"/*, "BeverageVersions"*/).FirstOrDefaultAsync(x => x.Id == id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var dto = mapper.Map<BeverageInfoDto>(info);
            var deviceInfo = await context.DeviceInfo.AsNoTracking().FirstOrDefaultAsync(w => w.Id == info.DeviceId);
            if (deviceInfo != null)
            {
                var deviceMaterialInfos = await context.DeviceMaterialInfo.AsNoTracking()
                    .Where(w => w.DeviceBaseId == deviceInfo.DeviceBaseId && w.Type == MaterialTypeEnum.Cassette)
                    .ToListAsync();
                foreach (var item in dto.FormulaInfos)
                {
                    // 当前饮品的物料盒信息
                    item.MaterialBoxName = deviceMaterialInfos.FirstOrDefault(w => w.Index == item.MaterialBoxId)?.Name ?? string.Empty;
                }
            }
            return dto;
        }

        /// <summary>
        /// 根据设备Id获取饮品列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<BeveragePageListDto>> GetBeverageInfoListAsync(QueryRequest request, long deviceId)
        {
            if (deviceId <= 0)
                throw new ArgumentOutOfRangeException(nameof(deviceId));
            //var deviceInfo = await context.DeviceInfo.FirstOrDefaultAsync(w => w.TransId == deviceId);
            //if (deviceInfo == null)
            //    throw new ArgumentException("设备不存在");
            request.IsIncludeQueries = true;
            var list = await context.BeverageInfo.AsNoTracking()
                .Where(w => w.DeviceId == deviceId)
                .OrderBy(o => o.Sort).ThenByDescending(o => o.CreateTime)
                .Select(s => new { s.Id, s.CategoryIds, s.Name, s.BeverageIcon, s.Code, s.CodeIsShow, s.Price, s.DiscountedPrice, s.Remarks, s.ProductionForecast, s.ForecastQuantity, s.Sort, s.SellStradgy, s.DisplayStatus, s.DeviceId })
                .ToPagedListAsync(request);

            long deviceBaseInfoId = (await context.DeviceInfo.FirstOrDefaultAsync(w => w.Id == deviceId))?.DeviceBaseId ?? 0;

            //当前页所有饮品Id集合
            var beverageIds = list.Items.Select(s => s.Id).ToList();

            //获取各饮品版本数量
            var versionCount = await context.BeverageVersion.AsNoTracking()
                .Where(w => w.VersionType != BeverageVersionTypeEnum.Collection && beverageIds.Contains(w.BeverageInfoId))
                .GroupBy(g => g.BeverageInfoId)
                .Select(s => new { beverageId = s.Key, Count = s.Count() })
                .ToDictionaryAsync(d => d.beverageId, d => d.Count);

            //获取各饮品配方集合
            //var formulaInfos = await context.FormulaInfo.AsNoTracking()
            //    .Where(w => beverageIds.Contains(w.BeverageInfoId))
            //    .GroupBy(g => g.BeverageInfoId)
            //    .Select(s => new { beverageId = s.Key, formulaType = string.Join(",", s.Select(s => s.FormulaType.GetDescription())) })
            //    .ToDictionaryAsync(d => d.beverageId, d => d.formulaType);

            var query = await (
                from f in context.FormulaInfo.AsNoTracking()
                join d in context.DeviceMaterialInfo.AsNoTracking()
                on f.MaterialBoxId equals d.Index into fd
                from d in fd.Where(x => x.Type == MaterialTypeEnum.Cassette).DefaultIfEmpty()
                where beverageIds.Contains(f.BeverageInfoId) && d.DeviceBaseId == deviceBaseInfoId
                select new
                {
                    f.Sort,
                    f.BeverageInfoId,
                    f.FormulaType,
                    BoxName = f.FormulaType == FormulaTypeEnum.Lh && d != null ? d.Name : null
                }
            )
            .OrderBy(o => o.Sort)
            .Distinct()
            .ToListAsync();

            var formulaInfos = query
               .GroupBy(x => x.BeverageInfoId)
               .ToDictionary(
                   g => g.Key,
                   g => string.Join(",",
                       g.Select(x =>
                       {
                           var desc = x.FormulaType.GetDescription();
                           var boxName = !string.IsNullOrEmpty(x.BoxName) ? $"({x.BoxName})" : "";
                           return $"{desc}{boxName}";
                       }))
               );

            var resDto = new List<BeveragePageListDto>();

            list.Items.ForEach(x =>
            {
                resDto.Add(new BeveragePageListDto()
                {
                    Id = x.Id,
                    CategoryIds = x.CategoryIds,
                    Name = x.Name,
                    BeverageIcon = x.BeverageIcon,
                    Code = x.Code,
                    CodeIsShow = x.CodeIsShow,
                    Price = x.Price,
                    DiscountedPrice = x.DiscountedPrice,
                    Remarks = x.Remarks,
                    ProductionForecast = x.ProductionForecast,
                    ForecastQuantity = x.ForecastQuantity,
                    FormulasText = formulaInfos.GetValueOrDefault(x.Id, string.Empty),
                    VersionText = string.Format($"第{versionCount.GetValueOrDefault(x.Id, 1)}版"),
                    Sort = x.Sort,
                    SellStradgy = x.SellStradgy,
                    DisplayStatus = x.DisplayStatus,
                });
            });

            if (resDto.Count > 0)
            {
                var productCategories = await context.ProductCategory.AsQueryable().Where(w => w.ProductCategoryType == ProductCategoryTypeEnum.Beverage).ToListAsync();
                foreach (var item in resDto)
                {
                    if (item.CategoryIds != null)
                    {
                        productCategories.Where(w => item.CategoryIds.Contains(w.Id)).ToList().ForEach(e => item.CategoryNames += e.Name + "，");
                    }
                }
            }

            return new PagedResultDto<BeveragePageListDto>()
            {
                Items = resDto,
                PageNumber = list.PageNumber,
                PageSize = list.PageSize,
                TotalCount = list.TotalCount
            };
        }

        /// <summary>
        /// 根据设备Id获取价格信息列表
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<PriceInfoDot>> GetPriceInfoListAsync(long deviceId)
        {
            var list = await context.BeverageInfo.AsNoTracking().Where(w => w.DeviceId == deviceId)
                .OrderBy(a => a.Sort).ThenByDescending(a => a.CreateTime)
                .Select(s => new PriceInfoDot()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    DiscountedPrice = s.DiscountedPrice,
                    Code = s.Code
                }).ToListAsync();
            return list;
        }
    }
}
