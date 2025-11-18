using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformQueries.IBeverageQueries.IBeverageInfoQueries;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.Util.Core;
using YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos;
using NPOI.SS.Formula;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.BeverageQueries.BeverageInfoQueries
{
    /// <summary>
    /// 平台饮品查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class P_BeverageInfoQueries(CoffeeMachinePlatformDbContext context, IMapper mapper) : IP_BeverageInfoQueries
    {
        /// <summary>
        /// 饮品分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<P_BeverageInfoDto>> GetBeverageInfoListAsync(BeverageInfoInput request)
        {
            var query = context.P_BeverageInfo.AsQueryable()
            .WhereIf(request.FormulaType != null, w => w.FormulaInfos.Select(s => s.FormulaType).Contains(request.FormulaType.Value))
            .WhereIf(!string.IsNullOrWhiteSpace(request.BevergeInfoName), w => w.Name.Contains(request.BevergeInfoName));

            var list = await query.OrderByDescending(a => a.CreateTime).ToPagedListAsync(request, "FormulaInfos", "BeverageVersions");

            var deviceModelIds = new List<long>();
            var deviceModels = new Dictionary<long, string>();
            if (list.TotalCount > 0)
            {
                deviceModelIds = list.Items.Select(s => s.DeviceModelId).Distinct().ToList();
                var curModels = await context.DeviceModel.Where(w => deviceModelIds.Contains(w.Id)).ToListAsync();
                deviceModels = curModels.ToDictionary(s => s.Id, s => s.Name);
            }

            var listDto = mapper.Map<List<P_BeverageInfoDto>>(list.Items);
            //listDto.ForEach(e => { e.FormulaInfos = e.FormulaInfoTemplates; e.FormulaInfoTemplates = []; e.FormulaInfos.ForEach(e => { e.MaterialBoxId = e.MaterialBoxId == null ? null : e.MaterialBoxId; }); });

            //组装设备类型名称
            if (deviceModels.Count > 0)
                listDto.ForEach(s => s.DeviceModelName = deviceModels[s.DeviceModelId]);

            PagedResultDto<P_BeverageInfoDto> pagedResultDto = new PagedResultDto<P_BeverageInfoDto>()
            {
                Items = listDto,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = list.TotalCount
            };

            return pagedResultDto;
        }

        /// <summary>
        /// 饮品版本数据（根据饮品id）
        /// </summary>
        /// <param name="beverageInfoId"></param>
        /// <returns></returns>
        public async Task<List<P_BeverageVersionDto>> GetBeverageVersionListAsync(long beverageInfoId)
        {
            var temp = await context.P_BeverageVersion.AsQueryable().AsNoTracking()
               .Where(a => a.BeverageInfoId == beverageInfoId && a.VersionType != BeverageVersionTypeEnum.Collection)
               .Select(a => new P_BeverageVersionDto
               {
                   Id = a.Id,
                   BeverageInfoId = a.BeverageInfoId,
                   VersionType = a.VersionType,
                   BeverageInfoDataString = a.BeverageInfoDataString,
                   CreateTime = a.CreateTime,
                   CreateUserId = a.CreateUserId,
                   CreateUserName = a.CreateUserName
               })
               .OrderByDescending(a => a.CreateTime)
               .ToListAsync();
            var count = temp.Count();
            foreach (var item in temp)
            {
                item.VersionNum = count--;
            }
            return temp;
        }
    }
}
