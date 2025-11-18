using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.DevicesQueries
{
    /// <summary>
    /// 设备型号查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class DeviceModelQueries(CoffeeMachineDbContext context, IMapper mapper) : IdeviceModelQueries
    {
        /// <summary>
        /// 根据Id获取设备型号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
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
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PagedResultDto<DeviceModelDto>> GetDeviceModelListAsync(QueryRequest request)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
                throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0011)]);
            //获取分页数据
            var list = await context.DeviceModel.ToPagedListAsync(request);

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

        /// <summary>
        /// 获取所有设备型号
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeviceModel>> GetAllDeviceModels()
        {
            return await context.DeviceModel.AsNoTracking().Where(w => !w.IsDelete).ToListAsync();
        }
    }
}
