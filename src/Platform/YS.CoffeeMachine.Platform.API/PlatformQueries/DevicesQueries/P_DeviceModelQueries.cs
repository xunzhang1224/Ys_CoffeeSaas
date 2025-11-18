using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformQueries.IDevicesQueries;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.DevicesQueries
{
    /// <summary>
    /// 设备型号查询
    /// </summary>
    /// <param name="context"></param>
    public class P_DeviceModelQueries(CoffeeMachinePlatformDbContext context, IMapper mapper) : IP_DeviceModelQueries
    {
        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DeviceModelDto> GetDeviceModelAsync(long id)
        {
            var info = await context.DeviceModel.FirstOrDefaultAsync(x => x.Id == id);
            if (info is null)
                throw new KeyNotFoundException();
            return mapper.Map<DeviceModelDto>(info);
        }

        /// <summary>
        /// 获取设备型号列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<object>> GetDeviceModelListAsync()
        {
            var list = await context.DeviceModel.AsNoTracking().ToListAsync();
            return list.Select(s => new { s.Id, s.Name, s.MaxCassetteCount }).Cast<object>().ToList();
        }

        /// <summary>
        /// 获取设备型号列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<DeviceModelDto>> GetDeviceModelListAsync(QueryRequest request)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
                throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0011)]);
            //获取分页数据
            var list = await context.DeviceModel.AsQueryable().OrderByDescending(o => o.CreateTime).ToPagedListAsync(request);

            var dtoList = mapper.Map<List<DeviceModelDto>>(list.Items);

            PagedResultDto<DeviceModelDto> pagedResultDto = new PagedResultDto<DeviceModelDto>()
            {
                Items = dtoList,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = list.TotalCount
            };
            return pagedResultDto;
        }
    }
}
