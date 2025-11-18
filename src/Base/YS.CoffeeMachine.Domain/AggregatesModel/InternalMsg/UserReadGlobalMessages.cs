namespace YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 用户消息
    /// </summary>
    public class UserReadGlobalMessages : BaseEntity
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public long MessageId { get; private set; }

        /// <summary>
        /// 消息实体
        /// </summary>
        public SystemMessages Message { get; private set; } = default!;

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; private set; }

        /// <summary>
        /// 已读时间
        /// </summary>
        public DateTime ReadTime { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// 保护构造
        /// </summary>
        protected UserReadGlobalMessages() { }

        /// <summary>
        /// 消息已读记录
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        public UserReadGlobalMessages(long messageId, long userId)
        {
            MessageId = messageId;
            UserId = userId;
        }
    }
}