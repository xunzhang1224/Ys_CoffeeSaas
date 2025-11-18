using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;

namespace YS.CoffeeMachine.Application.Queries.ISettingQueries
{
    /// <summary>
    /// 设置信息查询
    /// </summary>
    public interface ISettingInfoQueries
    {
        /// <summary>
        /// 根据设备Id获取设置信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<SettingInfoDto> GetSettingInfoDtoByDeviceIdAsync(long deviceId);
    }
}
