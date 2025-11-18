using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries.IBeverageCollectionQueries;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.BeverageQueries.BeverageCollectionQueries
{
    /// <summary>
    /// 饮品集合查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class BeverageCollectionInfoQueries(CoffeeMachineDbContext context, IMapper mapper) : IBeverageCollectionInfoQueries
    {
        /// <summary>
        /// 饮品集合查询
        /// </summary>
        public async Task<PagedResultDto<BeverageCollectionDto>> GetBeverageColltionListAsync(QueryRequest request, long enterpriseInfoId)
        {
            if (enterpriseInfoId <= 0)
                throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0011)]);
            var deviceInfo = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == enterpriseInfoId);
            if (deviceInfo == null)
                throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.D0008)]);
            var list = await context.BeverageCollection.Where(w => w.EnterpriseInfoId == enterpriseInfoId).OrderByDescending(o => o.CreateTime).ToPagedListAsync(request);
            if (list.TotalCount == 0)
                return
                    new PagedResultDto<BeverageCollectionDto>()
                    {
                        Items = [],
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
                        TotalCount = list.TotalCount
                    };
            var listDto = mapper.Map<List<BeverageCollectionDto>>(list.Items);
            PagedResultDto<BeverageCollectionDto> pagedResultDto = new PagedResultDto<BeverageCollectionDto>()
            {
                Items = listDto,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = list.TotalCount
            };
            return pagedResultDto;
        }

        /// <summary>
        /// 获取平台端饮品集合下拉数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<P_BeverageCollectionSelectedDto>> GetP_BeverageCollectionsAsync()
        {
            return await context.P_BeverageCollection.Where(w => !w.IsDelete).OrderBy(o => o.Id).AsNoTracking()
                .Select(s => new P_BeverageCollectionSelectedDto
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToListAsync();
        }
    }
}
