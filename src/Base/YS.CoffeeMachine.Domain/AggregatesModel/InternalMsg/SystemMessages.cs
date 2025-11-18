namespace YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg
{
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示系统内部消息的实体类。
    /// 用于支持平台内的全局公告、群组通知及用户私信功能。
    /// </summary>
    public class SystemMessages : BaseEntity
    {
        /// <summary>
        /// 获取或设置消息标题。
        /// </summary>
        public string Title { get; private set; } = default!;

        /// <summary>
        /// 获取或设置消息正文内容。
        /// </summary>
        public string Content { get; private set; } = default!;

        /// <summary>
        /// 获取或设置当前消息类型：
        /// 参考 InternalMsgEnum 定义，如：0=全局公告，1=群组消息，2=私信。
        /// </summary>
        public InternalMsgEnum MessageType { get; private set; }

        /// <summary>
        /// 获取或设置目标用户 ID，仅在消息类型为私信时使用。
        /// 可为空。
        /// </summary>
        public long? TargetUserId { get; private set; }

        /// <summary>
        /// 获取或设置目标群组/企业标识，仅在消息类型为群组消息时使用。
        /// 可为空。
        /// </summary>
        public string? TargetGroup { get; private set; }

        /// <summary>
        /// 获取或设置是否为弹窗提示消息。
        /// </summary>
        public bool IsPopup { get; private set; }

        /// <summary>
        /// 获取或设置消息优先级排序值，0 表示最高优先级，255 表示最低。
        /// 默认为 0。
        /// </summary>
        public byte Priority { get; private set; } = 0;

        /// <summary>
        /// 获取或设置消息过期时间。消息将在该时间后不再显示。
        /// 可为空。
        /// </summary>
        public DateTime? ExpireTime { get; private set; }

        /// <summary>
        /// 获取或设置当前消息是否已被撤销。
        /// 默认为 false。
        /// </summary>
        public bool IsCanceled { get; private set; } = false;

        /// <summary>
        /// 受保护的无参构造函数，防止外部直接实例化。
        /// 供ORM工具使用。
        /// </summary>
        protected SystemMessages() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 SystemMessages 实例。
        /// </summary>
        /// <param name="title">消息标题。</param>
        /// <param name="content">消息正文内容。</param>
        /// <param name="messageType">消息类型。</param>
        /// <param name="targetUserId">目标用户ID（仅用于私信）。</param>
        /// <param name="targetGroup">目标群组/企业（仅用于群组消息）。</param>
        /// <param name="isPopup">是否为弹窗提示消息。</param>
        /// <param name="priority">消息优先级，默认为0（最高）。</param>
        /// <param name="expireTime">消息过期时间。</param>
        public SystemMessages(
            string title,
            string content,
            InternalMsgEnum messageType,
            long? targetUserId = null,
            string? targetGroup = null,
            bool isPopup = false,
            byte priority = 0,
            DateTime? expireTime = null)
        {
            Title = title;
            Content = content;
            MessageType = messageType;
            TargetUserId = targetUserId;
            TargetGroup = targetGroup;
            IsPopup = isPopup;
            Priority = priority;
            ExpireTime = expireTime;
        }

        /// <summary>
        /// 更新当前消息的标题与正文内容。
        /// </summary>
        /// <param name="title">新的标题。</param>
        /// <param name="content">新的正文内容。</param>
        public void Update(string title, string content)
        {
            Title = title;
            Content = content;
        }

        /// <summary>
        /// 撤销当前消息，标记为已取消。
        /// 撤销后消息将不再推送或展示。
        /// </summary>
        public void Cancel()
        {
            IsCanceled = true;
        }
    }
}