using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 指标类型
    /// </summary>
    [Description("指标类型")]
    public enum MetricTypeEnum
    {
        /// <summary>
        /// 信号强度
        /// </summary>
        [Description("信号强度")]
        SignalStrength = 1,
        /// <summary>
        /// 箱体内温度（摄氏度）
        /// </summary>
        [Description("箱体内温度（摄氏度）")]
        BoxTmp = 2,

        /// <summary>
        ///门状态
        /// </summary>
        [Description("门状态")]
        DoorStatus = 4,
        /// <summary>
        /// 电池电压
        /// </summary>
        [Description("电池电压")]
        BatteryVoltage = 5,
        /// <summary>
        /// 地理位置
        /// </summary>
        [Description("地理位置")]
        Location = 6,
        /// <summary>
        /// 当前上电累计的电量（度）
        /// </summary>
        [Description("当前上电累计的电量（度）")]
        CEC = 7,
        /// <summary>
        /// 机器出厂后的累计电量（度）
        /// </summary>
        [Description("机器出厂后的累计电量（度）")]
        FCEC = 8,
        /// <summary>
        /// 电池电流
        /// </summary>
        [Description("电池电流")]
        BatteryCurrent = 9,
        /// <summary>
        /// 供电状态（On/Off）
        /// </summary>
        [Description("供电状态（On/Off）")]
        EState = 10,
        /// <summary>
        ///基站信息
        /// </summary>
        [Description("基站信息")]
        BaseInfo = 11,

        /// <summary>
        /// 咖啡豆盒光检
        /// </summary>
        [Description("咖啡豆盒光检")]
        CoffeeBoxLightInspection = 20,

        /// <summary>
        /// 废水桶
        /// </summary>
        [Description("废水桶")]
        WasteWaterBucket = 21,

        /// <summary>
        /// 中转水箱
        /// </summary>
        [Description("中转水箱")]
        TransferWaterTank = 22,

        /// <summary>
        /// 落杯器光检
        /// </summary>
        [Description("落杯器光检")]
        OpticalInspectionOfCupDropper = 23,

        /// <summary>
        /// 锅炉温度
        /// </summary>
        [Description("锅炉温度")]
        BoilerTemperature = 24,

        /// <summary>
        /// 落盖器光检
        /// </summary>
        [Description("落盖器光检")]
        OpticalInspectionOfLidDropDevice = 25,

        /// <summary>
        /// 取杯口光检
        /// </summary>
        [Description("取杯口光检")]
        GlassMouthLightInspection = 26,

        /// <summary>
        /// 防夹手光检
        /// </summary>
        [Description("防夹手光检")]
        AntiPinchLightInspection = 27,

        /// <summary>
        /// 机器状态
        /// </summary>
        [Description("机器状态")]
        MachineStatus = 28,

        /// <summary>
        /// 在线状态
        /// </summary>
        [Description("在线状态")]
        LineStatus = 51,
    }

    /// <summary>
    /// 表示机器能力下温控模式的枚举
    /// </summary>
    [Description("温控模式")]
    public enum TemperatureModeEnum
    {
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 0,
        [Description("冷")]
        Cold = 1,
        [Description("热")]
        Hot = 2,
    }
    /// <summary>
    /// 指标状态
    /// </summary>
    [Description("指标状态")]
    public enum MetricsStatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 告警
        /// </summary>
        [Description("告警")]
        Alarm = 1,
        /// <summary>
        /// 故障
        /// </summary>
        [Description("故障")]
        Fault = 2,

    }
}
