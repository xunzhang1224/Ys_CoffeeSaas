namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备预警配置的实体类。
    /// 用于定义设备在运行过程中需要触发的各种预警规则与时间设定。
    /// </summary>
    public class EarlyWarningConfig : BaseEntity
    {
        /// <summary>
        /// 获取或设置关联的设备唯一标识符。
        /// </summary>
        public long DeviceInfoId { get; private set; }

        /// <summary>
        /// 获取与此配置绑定的设备信息实体对象。
        /// 用于导航至设备详情。
        /// </summary>
        public DeviceInfo DeviceInfo { get; private set; }

        /// <summary>
        /// 获取或设置整机清洗功能是否启用。
        /// </summary>
        public bool WholeMachineCleaningSwitch { get; private set; }

        /// <summary>
        /// 获取或设置整机下次清洗计划时间。
        /// </summary>
        public DateTime NextWholeMachineCleaningTime { get; private set; }

        /// <summary>
        /// 获取或设置冲泡器清洗功能是否启用。
        /// </summary>
        public bool BrewingMachineCleaningSwitch { get; private set; }

        /// <summary>
        /// 获取或设置冲泡器下次清洗计划时间。
        /// </summary>
        public DateTime NextBrewingMachineCleaningTime { get; private set; }

        /// <summary>
        /// 获取或设置奶沫器清洗功能是否启用。
        /// </summary>
        public bool MilkFrotherCleaningSwitch { get; private set; }

        /// <summary>
        /// 获取或设置奶沫器下次清洗计划时间。
        /// </summary>
        public DateTime NextMilkFrotherCleaningTime { get; private set; }

        /// <summary>
        /// 获取或设置咖啡水路清洗功能是否启用。
        /// </summary>
        public bool CoffeeWaterwayCleaningSwitch { get; private set; }

        /// <summary>
        /// 获取或设置咖啡水路下次清洗计划时间。
        /// </summary>
        public DateTime NextCoffeeWaterwayCleaningTime { get; private set; }

        /// <summary>
        /// 获取或设置蒸汽水路清洗功能是否启用。
        /// </summary>
        public bool SteamWaterwayCleaningSwitch { get; private set; }

        /// <summary>
        /// 获取或设置蒸汽水路下次清洗计划时间。
        /// </summary>
        public DateTime NextSteamWaterwayCleaningTime { get; private set; }

        /// <summary>
        /// 获取或设置离线预警功能是否启用。
        /// </summary>
        public bool OfflineWarningSwitch { get; private set; }

        /// <summary>
        /// 获取或设置触发离线预警的天数阈值。
        /// </summary>
        public int OfflineDays { get; private set; }

        /// <summary>
        /// 获取或设置缺料预警功能是否启用。
        /// </summary>
        public bool ShortageWarningSwitch { get; private set; }

        /// <summary>
        /// 获取或设置咖啡豆余量预警阈值（百分比或数值）。
        /// </summary>
        public double CoffeeBeanRemaining { get; private set; }

        /// <summary>
        /// 获取或设置水箱余量预警阈值（百分比或数值）。
        /// </summary>
        public double WaterRemaining { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected EarlyWarningConfig() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 EarlyWarningConfig 实例。
        /// </summary>
        /// <param name="deviceInfoId">关联的设备唯一标识。</param>
        /// <param name="wholeMachineCleaningSwitch">整机清洗开关状态。</param>
        /// <param name="nextWholeMachineCleaningTime">整机下次清洗时间。</param>
        /// <param name="brewingMachineCleaningSwitch">冲泡器清洗开关状态。</param>
        /// <param name="nextBrewingMachineCleaningTime">冲泡器下次清洗时间。</param>
        /// <param name="milkFrotherCleaningSwitch">奶沫器清洗开关状态。</param>
        /// <param name="nextMilkFrotherCleaningTime">奶沫器下次清洗时间。</param>
        /// <param name="coffeeWaterwayCleaningSwitch">咖啡水路清洗开关状态。</param>
        /// <param name="nextCoffeeWaterwayCleaningTime">咖啡水路下次清洗时间。</param>
        /// <param name="steamWaterwayCleaningSwitch">蒸汽水路清洗开关状态。</param>
        /// <param name="nextSteamWaterwayCleaningTime">蒸汽水路下次清洗时间。</param>
        /// <param name="offlineWarningSwitch">离线预警开关状态。</param>
        /// <param name="offlineDays">触发离线预警的天数阈值。</param>
        /// <param name="shortageWarningSwitch">缺料预警开关状态。</param>
        /// <param name="coffeeBeanRemaining">咖啡豆余量预警阈值。</param>
        /// <param name="waterRemaining">水箱余量预警阈值。</param>
        public EarlyWarningConfig(
            long deviceInfoId,
            bool wholeMachineCleaningSwitch,
            DateTime nextWholeMachineCleaningTime,
            bool brewingMachineCleaningSwitch,
            DateTime nextBrewingMachineCleaningTime,
            bool milkFrotherCleaningSwitch,
            DateTime nextMilkFrotherCleaningTime,
            bool coffeeWaterwayCleaningSwitch,
            DateTime nextCoffeeWaterwayCleaningTime,
            bool steamWaterwayCleaningSwitch,
            DateTime nextSteamWaterwayCleaningTime,
            bool offlineWarningSwitch,
            int offlineDays,
            bool shortageWarningSwitch,
            double coffeeBeanRemaining,
            double waterRemaining)
        {
            DeviceInfoId = deviceInfoId;
            WholeMachineCleaningSwitch = wholeMachineCleaningSwitch;
            NextWholeMachineCleaningTime = nextWholeMachineCleaningTime;
            BrewingMachineCleaningSwitch = brewingMachineCleaningSwitch;
            NextBrewingMachineCleaningTime = nextBrewingMachineCleaningTime;
            MilkFrotherCleaningSwitch = milkFrotherCleaningSwitch;
            NextMilkFrotherCleaningTime = nextMilkFrotherCleaningTime;
            CoffeeWaterwayCleaningSwitch = coffeeWaterwayCleaningSwitch;
            NextCoffeeWaterwayCleaningTime = nextCoffeeWaterwayCleaningTime;
            SteamWaterwayCleaningSwitch = steamWaterwayCleaningSwitch;
            NextSteamWaterwayCleaningTime = nextSteamWaterwayCleaningTime;
            OfflineWarningSwitch = offlineWarningSwitch;
            OfflineDays = offlineDays;
            ShortageWarningSwitch = shortageWarningSwitch;
            CoffeeBeanRemaining = coffeeBeanRemaining;
            WaterRemaining = waterRemaining;
        }

        /// <summary>
        /// 更新当前预警配置的所有字段值。
        /// </summary>
        /// <param name="deviceInfoId">关联的设备唯一标识。</param>
        /// <param name="wholeMachineCleaningSwitch">整机清洗开关状态。</param>
        /// <param name="nextWholeMachineCleaningTime">整机下次清洗时间。</param>
        /// <param name="brewingMachineCleaningSwitch">冲泡器清洗开关状态。</param>
        /// <param name="nextBrewingMachineCleaningTime">冲泡器下次清洗时间。</param>
        /// <param name="milkFrotherCleaningSwitch">奶沫器清洗开关状态。</param>
        /// <param name="nextMilkFrotherCleaningTime">奶沫器下次清洗时间。</param>
        /// <param name="coffeeWaterwayCleaningSwitch">咖啡水路清洗开关状态。</param>
        /// <param name="nextCoffeeWaterwayCleaningTime">咖啡水路下次清洗时间。</param>
        /// <param name="steamWaterwayCleaningSwitch">蒸汽水路清洗开关状态。</param>
        /// <param name="nextSteamWaterwayCleaningTime">蒸汽水路下次清洗时间。</param>
        /// <param name="offlineWarningSwitch">离线预警开关状态。</param>
        /// <param name="offlineDays">触发离线预警的天数阈值。</param>
        /// <param name="shortageWarningSwitch">缺料预警开关状态。</param>
        /// <param name="coffeeBeanRemaining">咖啡豆余量预警阈值。</param>
        /// <param name="waterRemaining">水箱余量预警阈值。</param>
        public void Update(
            long deviceInfoId,
            bool wholeMachineCleaningSwitch,
            DateTime nextWholeMachineCleaningTime,
            bool brewingMachineCleaningSwitch,
            DateTime nextBrewingMachineCleaningTime,
            bool milkFrotherCleaningSwitch,
            DateTime nextMilkFrotherCleaningTime,
            bool coffeeWaterwayCleaningSwitch,
            DateTime nextCoffeeWaterwayCleaningTime,
            bool steamWaterwayCleaningSwitch,
            DateTime nextSteamWaterwayCleaningTime,
            bool offlineWarningSwitch,
            int offlineDays,
            bool shortageWarningSwitch,
            double coffeeBeanRemaining,
            double waterRemaining)
        {
            DeviceInfoId = deviceInfoId;
            WholeMachineCleaningSwitch = wholeMachineCleaningSwitch;
            NextWholeMachineCleaningTime = nextWholeMachineCleaningTime;
            BrewingMachineCleaningSwitch = brewingMachineCleaningSwitch;
            NextBrewingMachineCleaningTime = nextBrewingMachineCleaningTime;
            MilkFrotherCleaningSwitch = milkFrotherCleaningSwitch;
            NextMilkFrotherCleaningTime = nextMilkFrotherCleaningTime;
            CoffeeWaterwayCleaningSwitch = coffeeWaterwayCleaningSwitch;
            NextCoffeeWaterwayCleaningTime = nextCoffeeWaterwayCleaningTime;
            SteamWaterwayCleaningSwitch = steamWaterwayCleaningSwitch;
            NextSteamWaterwayCleaningTime = nextSteamWaterwayCleaningTime;
            OfflineWarningSwitch = offlineWarningSwitch;
            OfflineDays = offlineDays;
            ShortageWarningSwitch = shortageWarningSwitch;
            CoffeeBeanRemaining = coffeeBeanRemaining;
            WaterRemaining = waterRemaining;
        }
    }
}