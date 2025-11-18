using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;

namespace YS.CoffeeMachine.Application.Queries.IAdvertisementQueries
{
    /// <summary>
    /// 广告查询
    /// </summary>
    public interface IAdvertisementInfoQueries
    {
        /// <summary>
        /// 根据设备获取广告信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<AdvertisementInfoDto> GetAdvertisementInfoDtoByDeviceIdAsync(long deviceId);
    }
}
