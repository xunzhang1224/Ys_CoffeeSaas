namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    using System.ComponentModel;

    /// <summary>
    /// 步骤类型
    /// </summary>
    public enum TrailTypeEnum
    {
        /// <summary>
        /// 无轨迹类型
        /// </summary>
        [Description("无轨迹类型")]
        Not,

        /// <summary>
        /// 表示创建操作
        /// </summary>
        [Description("表示创建操作")]
        Add,

        /// <summary>
        /// 表示更新操作
        /// </summary>
        [Description("表示更新操作")]
        Update,

        /// <summary>
        /// 表示删除操作
        /// </summary>
        [Description("表示删除操作")]
        Delete
    }
}
