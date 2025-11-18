using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Settings
{
    /// <summary>
    /// 清洗枚举
    /// </summary>
    public enum WashEnum
    {
        /// <summary>
        /// 自动清洗
        /// </summary>
        [Description("自动清洗")]
        Automatic = 0,
        /// <summary>
        /// 开机自动清洗
        /// </summary>
        [Description("开机自动清洗")]
        PowerOn = 1,
        /// <summary>
        /// 定时自动清洗
        /// </summary>
        [Description("定时自动清洗")]
        Regular = 2
    }
    /// <summary>
    /// 运营权限枚举
    /// </summary>
    public enum OperationPermissionEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 编辑
        /// </summary>
        [Description("饮品编辑")]
        Edit = 1 << 1,
        /// <summary>
        /// 导入、导出
        /// </summary>
        [Description("配方导入、导出")]
        Import_Export = 1 << 2,
        /// <summary>
        /// 补货
        /// </summary>
        [Description("补货")]
        Replenish = 1 << 3
    }
    /// <summary>
    /// 调试权限枚举
    /// </summary>
    public enum DebuggingPermissionEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 查询状态
        /// </summary>
        [Description("查询状态")]
        QueryState = 1 << 1,
        /// <summary>
        /// 执行动作
        /// </summary>
        [Description("执行动作")]
        ExecuteAction = 1 << 2,
        /// <summary>
        /// 设置参数
        /// </summary>
        [Description("设置参数")]
        SetParameters = 1 << 3,
        /// <summary>
        /// 出料校准
        /// </summary>
        [Description("出料校准")]
        DischargeCalibration = 1 << 4,
        /// <summary>
        /// 制作产品
        /// </summary>
        [Description("制作产品")]
        MakeProduct = 1 << 5,
        /// <summary>
        /// 程序更新
        /// </summary>
        [Description("程序更新")]
        ProgramUpdates = 1 << 6
    }
    /// <summary>
    /// 设置权限枚举
    /// </summary>
    public enum SetUpPermissionEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 信息配置
        /// </summary>
        [Description("信息配置")]
        InfoConfig = 1 << 1,
        /// <summary>
        /// 版本
        /// </summary>
        [Description("版本")]
        Version = 1 << 2,
        /// <summary>
        /// 串口
        /// </summary>
        [Description("串口")]
        SerialPort = 1 << 3,
        /// <summary>
        /// 日志
        /// </summary>
        [Description("日志")]
        Logs = 1 << 4,
        /// <summary>
        /// 销售报表
        /// </summary>
        [Description("销售报表")]
        SalesReport = 1 << 5,
    }
    /// <summary>
    /// 周枚举，使用位运算可做到枚举多选
    /// </summary>
    public enum WeekEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 周一
        /// </summary>
        [Description("周一")]
        Monday = 1 << 0,    // 1 << 0 = 1 (0000001)
        /// <summary>
        /// 周二
        /// </summary>
        [Description("周二")]
        Tuesday = 1 << 1,   // 1 << 1 = 2 (0000010)
        /// <summary>
        /// 周三
        /// </summary>
        [Description("周三")]
        Wednesday = 1 << 2, // 1 << 2 = 4 (0000100)
        /// <summary>
        /// 周四
        /// </summary>
        [Description("周四")]
        Thursday = 1 << 3,  // 1 << 3 = 8 (0001000)
        /// <summary>
        /// 周五
        /// </summary>
        [Description("周五")]
        Friday = 1 << 4,    // 1 << 4 = 16 (0010000)
        /// <summary>
        /// 周六
        /// </summary>
        [Description("周六")]
        Saturday = 1 << 5,  // 1 << 5 = 32 (0100000)
        /// <summary>
        /// 周日
        /// </summary>
        [Description("周日")]
        Sunday = 1 << 6     // 1 << 6 = 64 (1000000)
    }
    /// <summary>
    /// 供水方式
    /// </summary>
    public enum WaterSupplyMethodEnum
    {
        /// <summary>
        /// 水箱供水
        /// </summary>
        [Description("水箱供水")]
        WaterTank = 0,
        /// <summary>
        /// 水桶供水
        /// </summary>
        [Description("水桶供水")]
        WaterBucket = 1,
        /// <summary>
        /// 直饮供水
        /// </summary>
        [Description("直饮供水")]
        DirectDrinking
    }
}
