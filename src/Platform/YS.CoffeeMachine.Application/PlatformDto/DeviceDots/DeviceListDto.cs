using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.PlatformDto.DeviceDots
{
    /// <summary>
    /// 设备列表dto
    /// </summary>
    public class DeviceListDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Mid
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 设备sn编号
        /// </summary>
        public string EquipmentNumber { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceStatusEnum DeviceStatus { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 设备型号id
        /// </summary>
        public long? DeviceModelId { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string DeviceModelName { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetaileAddress { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegisterTime { get; set; }
        /// <summary>
        /// 最近上线时间
        /// </summary>
        public DateTime? LatestOnlineTime { get; set; }
        /// <summary>
        /// 最近下线时间
        /// </summary>
        public DateTime? LatestOfflineTime { get; set; }

        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime? ActiveTime { get; set; }

        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 离线时间
        /// </summary>
        public DateTime? UpdateOfflineTime { get; set; }

        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime? UpdateOnlineTime { get; set; }
    }
}
