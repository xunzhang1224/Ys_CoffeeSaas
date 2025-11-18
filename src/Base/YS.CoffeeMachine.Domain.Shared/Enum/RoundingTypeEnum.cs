namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    using System.ComponentModel;

    /// <summary>
    /// 小数处理方式
    /// </summary>
    public enum RoundingTypeEnum
    {
        /// <summary>
        /// 四舍五入
        /// </summary>
        [Description("四舍五入")]
        RoundingType0,

        /// <summary>
        /// 四舍五入
        /// </summary>
        [Description("向上取整")]
        RoundingType1,

        /// <summary>
        /// 四舍五入
        /// </summary>
        [Description("向下取整")]
        RoundingType2,
    }
}
