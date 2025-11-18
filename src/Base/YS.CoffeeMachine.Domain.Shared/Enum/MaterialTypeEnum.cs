using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 物料类型
    /// </summary>
    public enum MaterialTypeEnum
    {
        /// <summary>
        /// Not
        /// </summary>
        [Description("Not")]
        Not,

        /// <summary>
        /// 咖啡豆
        /// </summary>
        [Description("咖啡豆")]
        CoffeeBean,

        /// <summary>
        /// 水
        /// </summary>
        [Description("水")]
        Water,

        /// <summary>
        /// 料盒
        /// </summary>
        [Description("料盒")]
        Cassette,

        /// <summary>
        /// 杯盖
        /// </summary>
        [Description("杯盖")]
        CupCover,

        /// <summary>
        /// 杯子
        /// </summary>
        [Description("杯子")]
        Cup
    }

    /// <summary>
    /// 补货类型
    /// </summary>
    public enum RestockTypeEnum
    {
        /// <summary>
        /// 本地补货
        /// </summary>
        [Description("本地补货")]
        BD,

        /// <summary>
        /// 远程补货
        /// </summary>
        [Description("远程补货")]
        YC
    }

    /// <summary>
    /// 补货商品货柜
    /// </summary>
    public enum HGTypeEnum
    {
        /// <summary>
        /// 咖啡机
        /// </summary>
        [Description("咖啡机")]
        CoffeeMachine,

        /// <summary>
        /// 副柜
        /// </summary>
        [Description("副柜")]
        FG
    }
}
