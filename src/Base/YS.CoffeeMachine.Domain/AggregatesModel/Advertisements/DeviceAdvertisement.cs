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
    /// 设备广告
    /// </summary>
    public class DeviceAdvertisement : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public long DeviceId { get; protected set; }

        /// <summary>
        /// 广告类型
        /// </summary>
        public AdvertisementEnum Type { get; protected set; }

        /// <summary>
        /// 轮播间隔时间
        /// </summary>
        public int CarouselIntervalSecond { get; protected set; }

        /// <summary>
        /// 待机时间
        /// </summary>
        public int? StandbyWaiteSecond { get; protected set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Enabled { get; protected set; }

        /// <summary>
        /// 设备广告
        /// </summary>
        protected DeviceAdvertisement()
        {

        }

        /// <summary>
        /// 设备广告
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="type"></param>
        /// <param name="carouselIntervalSecond"></param>
        /// <param name="standbyWaiteSecond"></param>
        /// <param name="enabled"></param>
        public DeviceAdvertisement(long deviceId, AdvertisementEnum type, int carouselIntervalSecond, int? standbyWaiteSecond, bool? enabled = true, long? id = null)
        {
            DeviceId = deviceId;
            Type = type;
            CarouselIntervalSecond = carouselIntervalSecond;
            StandbyWaiteSecond = standbyWaiteSecond;
            Enabled = enabled;
            if (id != null)
            {
                Id = id ?? 0;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="carouselIntervalSecond"></param>
        /// <param name="standbyWaiteSecond"></param>
        /// <param name="enabled"></param>
        public void Update(int carouselIntervalSecond, int? standbyWaiteSecond = null, bool? enabled = true)
        {
            CarouselIntervalSecond = carouselIntervalSecond;
            StandbyWaiteSecond = standbyWaiteSecond;
            Enabled = enabled;
        }
    }
}
