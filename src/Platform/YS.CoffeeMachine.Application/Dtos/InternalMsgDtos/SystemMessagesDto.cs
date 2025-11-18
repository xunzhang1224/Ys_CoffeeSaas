using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.InternalMsgDtos
{
    /// <summary>
    /// 系统信息dto
    /// </summary>
    public class SystemMessagesDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// 站内信内容
        /// </summary>
        public string Content { get; set; } = default!;

        /// <summary>
        /// 消息类型：0=全局公告，1=群组消息，2=私信
        /// </summary>
        public InternalMsgEnum MessageType { get; set; }

        /// <summary>
        /// 指定用户 ID（仅用于私信）
        /// </summary>
        public long? TargetUserId { get; set; }

        /// <summary>
        /// 指定群组/企业（仅用于群组消息）
        /// </summary>
        public List<long>? TargetGroup { get; set; }

        /// <summary>
        /// 目标组文本
        /// </summary>
        public string? TargetGroupText { get; set; } = default!;

        /// <summary>
        /// 是否是弹窗消息
        /// </summary>
        public bool IsPopup { get; set; }

        /// <summary>
        /// 优先级排序，0=最高，255=最低
        /// </summary>
        public byte Priority { get; set; } = 0;

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
