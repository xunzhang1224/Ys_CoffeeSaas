using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 饮品类型枚举
    /// </summary>
    public enum BeverageTypeEnum
    {
        /// <summary>
        /// 咖啡
        /// </summary>
        [Description("咖啡")]
        Coffee,

        /// <summary>
        /// 非咖啡
        /// </summary>
        [Description("非咖啡")]
        NotCoffee,
    }
}