using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.AdvertisementDtos
{
    /// <summary>
    /// 设备广告输入
    /// </summary>
    public class DeviceAdInput
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long? DeviceId { get; set; }

        /// <summary>
        /// 半屏数据
        /// </summary>
        public HalfScreen HalfScreen { get; set; }

        /// <summary>
        /// 全屏数据
        /// </summary>
        public FullScreen FullScreen { get; set; }
    }

    /// <summary>
    /// 半屏数据
    /// </summary>
    public class HalfScreen
    {

        /// <summary>
        /// id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 轮播间隔时间
        /// </summary>
        public int powerOnAdsPlayTime { get; set; }

        /// <summary>
        /// 轮播图片
        /// </summary>
        public List<AdFile> adList { get; set; }
    }

    /// <summary>
    /// 全屏数据
    /// </summary>
    public class FullScreen
    {

        /// <summary>
        /// id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 待机广告开关
        /// </summary>
        public bool? StandbyAdStatus { get; set; }

        /// <summary>
        /// 待机广告轮询时间
        /// </summary>
        public int StandbyAdsPlayTime { get; set; }

        /// <summary>
        /// 待机广告等待时间
        /// </summary>
        public int? StandbyAdsAwaitTime { get; set; }

        /// <summary>
        /// 轮播图片
        /// </summary>
        public List<AdFile> adList { get; set; }
    }

    /// <summary>
    /// 轮播图片
    /// </summary>
    public class AdFile
    {

        /// <summary>
        /// id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 播放时长
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 是否全屏广告
        /// </summary>
        public bool IsFullScreenAd { get; set; }

        /// <summary>
        /// 文件长度
        /// </summary>
        public int FileLength { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 文件信息
        /// </summary>
        public FileManageInput? file { get; set; }

        /// <summary>
        /// 文件Id
        /// </summary>
        public long? FileId { get; set; }
    }
}
