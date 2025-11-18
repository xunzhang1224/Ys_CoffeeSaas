namespace YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 用户消息
    /// </summary>
    public class UserMessages : BaseEntity
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
        public long UserId { get; private set; } = default!;

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; private set; } = false;

        /// <summary>
        /// 已读时间
        /// </summary>
        public DateTime? ReadTime { get; private set; }

        /// <summary>
        /// 是否已弹窗
        /// </summary>
        public bool IsPopupShown { get; private set; } = false;

        /// <summary>
        /// 保护构造
        /// </summary>
        protected UserMessages() { }

        /// <summary>
        /// 添加用户消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="message"></param>
        /// <param name="userId"></param>
        /// <param name="isRead"></param>
        /// <param name="readTime"></param>
        /// <param name="isPopupShown"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserMessages(long messageId, long userId, bool isRead = false, DateTime? readTime = null, bool isPopupShown = false)
        {
            MessageId = messageId;
            UserId = userId;
            IsRead = isRead;
            ReadTime = readTime;
            IsPopupShown = isPopupShown;
        }

        /// <summary>
        /// 标记为已读
        /// </summary>
        public void MarkAsRead()
        {
            IsRead = true;
            ReadTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 标记为已弹窗
        /// </summary>
        public void MarkAsPopupShown()
        {
            IsPopupShown = true;
        }
    }
}