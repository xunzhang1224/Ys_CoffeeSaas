using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.DeviceDots;
using YS.CoffeeMachine.Application.PlatformQueries.IDevicesQueries;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.DevicesQueries
{
    /// <summary>
    /// 设备分配查询
    /// </summary>
    /// <param name="context"></param>
    public class P_DeviceAllocationQueries(CoffeeMachinePlatformDbContext context) : IP_DeviceAllocationQueries
    {
        /// <summary>
        /// 设备分配分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<DeviceAllocationDto>> GetDeviceAllocationPageListAsync(DeviceAllocationInput request)
        {
            var query = from a in context.DeviceBaseInfo
                        join b in context.DeviceInfo on a.Id equals b.DeviceBaseId into bGroup
                        from b in bGroup.DefaultIfEmpty()
                        join c in context.EnterpriseInfo on b.EnterpriseinfoId equals c.Id into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        join d in context.DeviceModel on b.DeviceModelId equals d.Id into dGroup
                        from d in dGroup.DefaultIfEmpty()
                        select new DeviceAllocationDto
                        {
                            DeviceBaseId = a.Id,
                            DeviceId = b != null ? b.Id : null,
                            EquipmentNumber = a.MachineStickerCode,
                            Name = b != null ? b.Name : null,
                            EnterpriseId = b != null ? b.EnterpriseinfoId : null,
                            EnterpriseName = c != null ? c.Name : null,
                            DeviceModel = d != null ? d.Name : null,
                            CreateTime = b != null ? b.CreateTime : null,
                            RegisterTime = b != null ? b.CreateTime.ToString() : null
                        };

            return await query.WhereIf(!string.IsNullOrWhiteSpace(request.DeviceName), a => a.EquipmentNumber.Contains(request.DeviceName) || a.Name.Contains(request.DeviceName))
                .WhereIf(!string.IsNullOrEmpty(request.EnterpriseName), a => request.EnterpriseName.Contains(a.EnterpriseName))
                .WhereIf(request.TimeRange.Count > 0 && request.TimeRange.Count == 2, a => a.CreateTime >= request.TimeRange[0] && a.CreateTime < request.TimeRange[1].AddDays(1))
                .ToPagedListAsync(request);

            //request.IsIncludeQueries = true;
            //var query = context.DeviceInfo.AsQueryable();
            ////获取企业Id
            //List<long> enterpriseIds = new List<long>();
            //if (!string.IsNullOrWhiteSpace(request.EnterpriseName))
            //{
            //    var enterpriseInfos = await context.EnterpriseInfo.AsNoTracking().Where(w => w.Name.Contains(request.EnterpriseName)).ToListAsync();
            //    if (enterpriseInfos != null && enterpriseInfos.Count > 0)
            //        enterpriseIds = enterpriseInfos.Select(s => s.Id).ToList();
            //}
            ////数据过滤
            //query = query.WhereIf(!string.IsNullOrWhiteSpace(request.DeviceName), w => w.Name.Contains(request.DeviceName))
            //    .WhereIf(request.TimeRange.Count > 0 && request.TimeRange.Count == 2, w => w.CreateTime >= request.TimeRange[0] && w.CreateTime < request.TimeRange[1].AddDays(1))
            //    .WhereIf(enterpriseIds.Count > 0, w => enterpriseIds.Contains(w.EnterpriseinfoId));
            //var pageList = await query.ToPagedListAsync(request, "DeviceUserAssociations.User", "DeviceModel");
            ////获取当前设备企业信息
            //var curAllEnterpriseList = await context.EnterpriseInfo.Where(w => pageList.Items.Select(s => s.EnterpriseinfoId).Contains(w.Id)).AsNoTracking().ToListAsync();
            //var result = pageList.Items.Select(i => new DeviceAllocationDto
            //{
            //    DeviceBaseId = i.Id,
            //    Name = i.Name,
            //    EquipmentNumber = i.EquipmentNumber,
            //    DeviceModel = i.DeviceModel.Name,
            //    EnterpriseId = i.EnterpriseinfoId,
            //    EnterpriseName = curAllEnterpriseList.FirstOrDefault(w => w.Id == i.EnterpriseinfoId)?.Name,
            //    UserNames = string.Join(",", i.DeviceUserAssociations.Select(i => i.User.Account)),
            //    RegisterTime = i.CreateTime.ToString("G")
            //}).ToList();
            PagedResultDto<DeviceAllocationDto> pagedResultDto = new PagedResultDto<DeviceAllocationDto>()
            {
                //Items = result,
                //PageNumber = pageList.PageNumber,
                //PageSize = pageList.PageSize,
                //TotalCount = pageList.TotalCount
            };
            return pagedResultDto;
        }
    }
}
