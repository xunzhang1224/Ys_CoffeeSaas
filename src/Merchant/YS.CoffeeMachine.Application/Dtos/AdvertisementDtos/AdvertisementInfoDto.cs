using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Dtos.AdvertisementDtos
{
    /// <summary>
    /// 广告信息
    /// </summary>
    public class AdvertisementInfoDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceInfoId { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public DeviceInfo DeviceInfo { get; set; }
        #region 开机广告
        /// <summary>
        /// 开机广告音量
        /// </summary>
        public int PowerOnAdsVolume { get; set; }
        /// <summary>
        /// 开机广告播放时间
        /// </summary>
        public int PowerOnAdsPlayTime { get; set; }
        /// <summary>
        /// 开机广告内容（图片集合）
        /// </summary>
        public string PowerOnAdsImagesJson { get; set; }
        #endregion

        #region 待机广告
        /// <summary>
        /// 待机广告音量
        /// </summary>
        public int StandbyAdsVolume { get; set; }
        /// <summary>
        /// 待机广告播放时间
        /// </summary>
        public int StandbyAdsPlayTime { get; set; }
        /// <summary>
        /// 待机广告等待时间
        /// </summary>
        public int StandbyAdsAwaitTime { get; set; }
        /// <summary>
        /// 待机广告间隔时间
        /// </summary>
        public int StandbyAdsLoopTime { get; private set; }
        /// <summary>
        /// 待机广告循环类型（true:无限循环,false:播放自动退出）
        /// </summary>
        public bool StandbyAdsLoopType { get; set; }
        /// <summary>
        /// 待机广告内容
        /// </summary>
        public string StandbyAdsImagesJson { get; set; }
        #endregion

        #region 饮品制作广告
        /// <summary>
        /// 饮品制作广告音量
        /// </summary>
        public int ProductionAdsVolume { get; set; }
        /// <summary>
        /// 饮品制作广告播放时间
        /// </summary>
        public int ProductionAdsPlayTime { get; set; }
        /// <summary>
        /// 饮品制作广告内容
        /// </summary>
        public string ProductionAdsImagesJson { get; set; }
        #endregion
    }
}
