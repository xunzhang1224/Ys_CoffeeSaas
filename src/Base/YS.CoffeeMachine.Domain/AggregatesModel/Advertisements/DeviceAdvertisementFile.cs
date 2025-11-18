using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Advertisements
{
    /// <summary>
    /// 设备广告文件
    /// </summary>
    public class DeviceAdvertisementFile : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 设备广告Id
        /// </summary>
        public long DeviceAdvertisementId { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string Url { get; protected set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 播放时长
        /// </summary>
        public int? Duration { get; protected set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; protected set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string Suffix { get; protected set; }

        /// <summary>
        /// 是否全屏广告
        /// </summary>
        public bool IsFullScreenAd { get; protected set; }

        /// <summary>
        /// 文件长度
        /// </summary>
        public int FileLength { get; protected set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool Enable { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected DeviceAdvertisementFile()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="duration"></param>
        /// <param name="sort"></param>
        /// <param name="suffix"></param>
        /// <param name="isFullScreenAd"></param>
        /// <param name="fileLength"></param>
        /// <param name="enable"></param>
        public DeviceAdvertisementFile(long deviceAdvertisementId, string url, string name, int? duration, int sort, string suffix, bool isFullScreenAd, int fileLength, bool enable, long? id = null)
        {
            DeviceAdvertisementId = deviceAdvertisementId;
            Url = url;
            Name = name;
            Duration = duration;
            Sort = sort;
            Suffix = suffix;
            IsFullScreenAd = isFullScreenAd;
            FileLength = fileLength;
            Enable = enable;
            if (id != null)
            {
                Id = id ?? 0;
            }
        }
    }
}
