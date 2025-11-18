using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;

namespace YS.CoffeeMachine.Application.Queries.IAdvertisementQueries
{
    /// <summary>
    /// 广告信息查询接口，定义了与广告数据相关的查询操作
    /// 当前接口主要用于根据设备信息获取对应的广告内容
    /// </summary>
    public interface IAdvertisementInfoQueries
    {
        /// <summary>
        /// 根据设备唯一标识异步获取广告信息
        /// </summary>
        /// <param name="deviceId">设备的唯一标识ID</param>
        /// <returns>广告信息 DTO 对象</returns>
        Task<AdvertisementInfoDto> GetAdvertisementInfoDtoByDeviceIdAsync(long deviceId);
    }
}
