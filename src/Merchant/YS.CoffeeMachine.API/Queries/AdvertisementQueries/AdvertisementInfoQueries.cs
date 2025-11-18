using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Application.Queries.IAdvertisementQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.AdvertisementQueries
{
    /// <summary>
    /// 获取广告信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class AdvertisementInfoQueries(CoffeeMachineDbContext context, IMapper mapper) : IAdvertisementInfoQueries
    {
        /// <summary>
        /// 根据设备Id获取广告信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<AdvertisementInfoDto> GetAdvertisementInfoDtoByDeviceIdAsync(long deviceId)
        {
            var advertisementInfo = await context.AdvertisementInfo.FirstOrDefaultAsync(w => w.DeviceInfoId == deviceId);
            return mapper.Map<AdvertisementInfoDto>(advertisementInfo);
        }
    }
}
