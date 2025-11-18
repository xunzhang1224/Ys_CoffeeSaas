namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 站内信枚举
    /// </summary>
    public enum InternalMsgEnum
    {
        /// <summary>
        /// 全局公告
        /// </summary>
        Global = 0,

        /// <summary>
        /// 群组消息（按企业分组）
        /// </summary>
        Group = 1,

        /// <summary>
        /// 私信
        /// </summary>
        Private = 2
    }
}