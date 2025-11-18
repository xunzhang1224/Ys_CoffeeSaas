using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.DevicesQueries
{
    /// <summary>
    /// 预警配置查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class EarlyWarningConfigQueries(CoffeeMachineDbContext context, IMapper mapper) : IEarlyWarningConfigQueries
    {
        /// <summary>
        /// 根据Id获取预警配置信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<EarlyWarningConfigDto> GetEarlyWarningConfigByIdAsync(long deviceId)
        {
            var info = await context.EarlyWarningConfig.FirstOrDefaultAsync(x => x.DeviceInfoId == deviceId);
            if (info is null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0075)]);
            return mapper.Map<EarlyWarningConfigDto>(info);
        }
    }
}
