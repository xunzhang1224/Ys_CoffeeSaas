using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.Queries.ISettingQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.SettingQueries
{
    /// <summary>
    /// 获取设置信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class SettingInfoQueries(CoffeeMachineDbContext context, IMapper mapper) : ISettingInfoQueries
    {
        /// <summary>
        /// 根据设备Id获取设置信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SettingInfoDto> GetSettingInfoDtoByDeviceIdAsync(long deviceId)
        {
            var settingInfo = await context.SettingInfo.Includes("MaterialBoxs").FirstOrDefaultAsync(w => w.DeviceId == deviceId);
            return mapper.Map<SettingInfoDto>(settingInfo);
        }
    }
}
