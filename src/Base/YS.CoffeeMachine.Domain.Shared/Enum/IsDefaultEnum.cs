using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 是否默认
    /// </summary>
    public enum IsDefaultEnum
    {
        /// <summary>
        /// 不默认
        /// </summary>
        [Description("否")]
        No,
        /// <summary>
        /// 默认
        /// </summary>
        [Description("是")]
        Yes,
    }
}