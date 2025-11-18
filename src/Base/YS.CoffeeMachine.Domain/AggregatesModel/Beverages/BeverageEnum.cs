namespace YS.CoffeeMachine.Domain.AggregatesModel.Beverages
{
    using System.ComponentModel;

    /// <summary>
    /// 饮料类型
    /// </summary>
    public enum FormulaTypeEnum
    {
        [Description("咖啡")]
        Coffee,
        [Description("水")]
        Water,
        [Description("冰")]
        Ice,
        [Description("料盒")]
        Lh
    }

    /// <summary>
    /// 温度类型
    /// </summary>
    public enum TemperatureEnum
    {
        ///// <summary>
        ///// 高温
        ///// </summary>
        //[Description("高温")]
        //High = 0,

        ///// <summary>
        ///// 中温
        ///// </summary>
        //[Description("中温")]
        //Medium = 1,

        /// <summary>
        /// 高温
        /// </summary>
        [Description("冷饮")]
        Low = 0,

        /// <summary>
        /// 低温
        /// </summary>
        [Description("热饮")]
        High = 1
    }

    /// <summary>
    /// 饮品应用类型
    /// </summary>
    public enum BeverageAppliedType
    {
        /// <summary>
        /// 替换
        /// </summary>
        Replace = 0,

        /// <summary>
        /// 新增
        /// </summary>
        Add = 1
    }

    /// <summary>
    /// 替换内容类型
    /// </summary>
    public enum ReplaceContentType
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        BaseInfo = 0,

        /// <summary>
        /// 配方信息
        /// </summary>
        FormulaInfo = 1,

        /// <summary>
        /// 基本信息+配方信息
        /// </summary>
        ALL = 2
    }

    /// <summary>
    /// 饮品温度
    /// </summary>
    public enum BeverageHeatTypeEnum
    {
        /// <summary>
        /// 冷饮
        /// </summary>
        [Description("冷饮")]
        Cold,

        /// <summary>
        /// 热饮
        /// </summary>
        [Description("热饮")]
        Heat
    }
}
