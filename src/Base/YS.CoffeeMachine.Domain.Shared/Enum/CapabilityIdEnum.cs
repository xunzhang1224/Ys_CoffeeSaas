using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 能力Id枚举
    /// </summary>
    [Description("能力Id枚举")]
    public enum CapabilityIdEnum
    {
        [Description("异常锁机")]
        AbnormalLocking = 1,

        [Description("运营时间")]
        N2_OperationTime = 2,

        [Description("购物车")]
        Cart = 3,

        [Description("补货模式")]
        ReplenishWay = 4,

        [Description("温控相关")]
        N5_TemperatureMode = 5,

        [Description("掉货光检")]
        OpticalInspection = 6,

        [Description("灯带控制")]
        N7_LightBar = 7,

        [Description("支付相关")]
        Payment = 8,

        [Description("远程进后台")]
        RemoteBackstage = 9,

        [Description("日志上报")]
        LogUpload = 10,

        [Description("机器重启")]
        Restart = 11,

        [Description("清除货道故障")]
        ClearFault = 12,

        [Description("是否可修改服务器")]
        EditServer = 13,

        [Description("是否支持广告")]
        Advertising = 14,

        [Description("是否支持修改机器后台密码")]
        EditPwd = 15,

        [Description("机器购买二维码配置")]
        PurchaseQRCode = 16,

        [Description("出货超时时间配置")]
        ShipmentTimeout = 17,

        [Description("锁机模式")]
        N18_LockMode = 18,

        [Description("皮肤插件")]
        Skin = 19,

        [Description("按商品编码显示")]
        CommodityCodeShow = 20,

        [Description("玻璃加热配置")]
        N21_GlassHeating = 21,

        [Description("蜂鸣器配置")]
        Buzzer = 22,

        [Description("监控摄像头配置")]
        Camera = 23,

        [Description("锁定能力配置")]
        LockCaoabikity,

        [Description("设备迁移")]
        DeviceMigration,

        [Description("音量")]
        N26_Volume = 26,

        [Description("密码")]
        Password = 27,

        [Description("周期")]
        N28_Cycle = 28,

        [Description("温控仪模式")]
        TemperatureControllerMode,

        [Description("安卓截屏")]
        Screenshot,

        [Description("清除日志")]
        ClearLog,

        [Description("清除缓存")]
        ClearCache,

        #region 新加
        [Description("体检")]
        Checkup,
        [Description("料盒名称")]
        BoxName = 44,
        [Description("广告")]
        AdvertisNow = 45,
        [Description("清洗整机")]
        WASH_WHOLE_MACHINE = 61,
        [Description("远程锁机或解锁")]
        LOCK_UNLOCK_MACHINE = 63,
        [Description("远程关机")]
        POWER_OFF = 64,
        [Description("恢复出厂设置")]
        RESET_PROGRAM = 65,
        [Description("上报指标")]
        UPLOAD_METRIC = 66,
        [Description("远程出货")]
        REMOTE_SHIP = 67,
        [Description("远程清除下位机故障")]
        REMOTE_CLEAR_FAULTS = 68,
        [Description("清洗配置")]
        FlushCfg = 69,
        [Description("币种")]
        Currency = 70,
        [Description("支付方式")]
        PayTypes = 71,
        [Description("初始化")]
        Init = 72,
        #endregion
    }

    /// <summary>
    /// 设备能力类型
    /// </summary>
    [Description("设备能力类型")]
    public enum CapacityTypeEnum
    {
        [Description("硬件能力")]
        Hardware = 1,
        [Description("软件能力")]
        Software = 2,
    }

    /// <summary>
    /// 设备能力权限属性 0:可读可写 1:可读不可写 2:不可读可写 3:不可读不可写
    /// </summary>
    [Description("权限属性")]
    public enum PremissionTypeEnum
    {
        [Description("可读可写")]
        Rw = 0,
        [Description("可读不可写")]
        R = 1,
        [Description("不可读可写")]
        W = 2,
        [Description("不可读不可写")]
        Not = 3
    }

    /// <summary>
    /// 数据结构类型; 0整型，1布尔型，2字符串，3jsonObject，4jsonArray，5浮点型，6String数组型
    /// </summary>
    [Description("数据结构类型")]
    public enum StructureTypeEnum
    {
        [Description("整型")]
        Int = 0,

        [Description("布尔型")]
        Bl = 1,

        [Description("字符串")]
        Str = 2,

        [Description("jsonObject")]
        JsonObject = 3,

        [Description("jsonArray")]
        JsonArray = 4,

        [Description("浮点型")]
        Float = 5,

        [Description("String数组型")]
        StrArray = 6,
    }
}
