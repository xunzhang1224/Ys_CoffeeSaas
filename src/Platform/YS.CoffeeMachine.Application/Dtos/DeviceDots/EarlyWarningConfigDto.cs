using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 预警dto
    /// </summary>
    public class EarlyWarningConfigDto
    {/// <summary>
     /// 设备Id
     /// </summary>
        public long DeviceInfoId { get; set; }
        /// <summary>
        /// 整机清洗开关
        /// </summary>
        public bool WholeMachineCleaningSwitch { get; set; }
        /// <summary>
        /// 整机下次清洗时间
        /// </summary>
        public DateTime NextWholeMachineCleaningTime { get; set; }
        /// <summary>
        /// 冲泡器清洗开关
        /// </summary>
        public bool BrewingMachineCleaningSwitch { get; set; }
        /// <summary>
        /// 冲泡器下次清洗时间
        /// </summary>
        public DateTime NextBrewingMachineCleaningTime { get; set; }
        /// <summary>
        /// 奶沫器清洗剂清洗开关
        /// </summary>
        public bool MilkFrotherCleaningSwitch { get; set; }
        /// <summary>
        /// 奶沫器清洗剂下次清洗时间
        /// </summary>
        public DateTime NextMilkFrotherCleaningTime { get; set; }
        /// <summary>
        /// 咖啡水路清洗开关
        /// </summary>
        public bool CoffeeWaterwayCleaningSwitch { get; set; }
        /// <summary>
        /// 咖啡水路下次清洗时间
        /// </summary>
        public DateTime NextCoffeeWaterwayCleaningTime { get; set; }
        /// <summary>
        /// 蒸汽水路清洗开关
        /// </summary>
        public bool SteamWaterwayCleaningSwitch { get; set; }
        /// <summary>
        /// 蒸汽水路下次清洗时间
        /// </summary>
        public DateTime NextSteamWaterwayCleaningTime { get; set; }
        /// <summary>
        /// 离线预警开关
        /// </summary>
        public bool OfflineWarningSwitch { get; set; }
        /// <summary>
        /// 离线超多少天触发预警
        /// </summary>
        public int OfflineDays { get; set; }
        /// <summary>
        /// 缺料预警
        /// </summary>
        public bool ShortageWarningSwitch { get; set; }
        /// <summary>
        /// 咖啡豆余量预警
        /// </summary>
        public double CoffeeBeanRemaining { get; set; }
        /// <summary>
        /// 水余量预警
        /// </summary>
        public double WaterRemaining { get; set; }
    }
}
