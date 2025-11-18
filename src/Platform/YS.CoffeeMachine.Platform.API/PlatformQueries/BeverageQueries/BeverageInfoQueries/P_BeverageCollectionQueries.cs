using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos;
using YS.CoffeeMachine.Application.PlatformQueries.IBeverageQueries;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.BeverageQueries.BeverageInfoQueries
{
    /// <summary>
    /// 饮品合集查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class P_BeverageCollectionQueries(CoffeeMachinePlatformDbContext context, IMapper mapper) : IBeverageCollectionQueries
    {
        /// <summary>
        /// 获取饮品合集列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<P_BeverageCollectionDto>> GetBeverageCollectionList(P_BeverageCollectionInput request)
        {
            var info = await context.P_BeverageCollection.AsQueryable()
                .WhereIf(!string.IsNullOrWhiteSpace(request.Name), a => request.Name.Contains(a.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(request.LanguageKey), a => a.LanguageKey == request.LanguageKey)
                .WhereIf(request.DeviceModelId != null, a => a.DeviceModelId == request.DeviceModelId)
                .WhereIf(!string.IsNullOrWhiteSpace(request.BeverageName), a => a.BeverageNames.Contains(request.BeverageName))
                .Select(a => new P_BeverageCollectionDto
                {
                    Id = a.Id,
                    LanguageKey = a.LanguageKey,
                    DeviceModelId = a.DeviceModelId,
                    Name = a.Name,
                    BeverageIds = a.BeverageIds,
                    BeverageNames = a.BeverageNames,
                    CreateTime = a.CreateTime,
                    CreateUserId = a.CreateUserId,
                    CreateUserName = a.CreateUserName
                }).ToPagedListAsync(request);

            var deviceModels = new Dictionary<long, string>();
            if (info.TotalCount > 0)
            {
                var deviceModelIds = new List<long>();
                deviceModelIds = info.Items.Select(s => s.DeviceModelId).Distinct().ToList();
                var curModels = await context.DeviceModel.Where(w => deviceModelIds.Contains(w.Id)).ToListAsync();
                deviceModels = curModels.ToDictionary(s => s.Id, s => s.Name);
            }

            // 组装设备类型名称
            if (deviceModels.Count > 0)
                info.Items.ForEach(s => s.DeviceModelName = deviceModels[s.DeviceModelId]);

            return info;
        }
    }
}
