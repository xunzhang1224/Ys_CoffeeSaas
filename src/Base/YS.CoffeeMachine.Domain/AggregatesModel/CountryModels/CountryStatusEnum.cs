namespace YS.CoffeeMachine.Domain.AggregatesModel.CountryModels
{
    using System.ComponentModel;

    /// <summary>
    /// IsEnabledEnum
    /// </summary>
    public enum IsEnabledEnum
    {
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disable = 0,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enabled = 1
    }
}
