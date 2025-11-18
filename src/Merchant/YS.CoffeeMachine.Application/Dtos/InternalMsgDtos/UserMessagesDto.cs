using YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg;

namespace YS.CoffeeMachine.Application.Dtos.InternalMsgDtos
{
    /// <summary>
    /// 用户消息
    /// </summary>
    public class UserMessagesDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public long MessageId { get; set; }

        /// <summary>
        /// 消息实体
        /// </summary>
        public SystemMessages Message { get; set; } = default!;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; } = default!;

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; } = default!;

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// 已读时间
        /// </summary>
        public DateTime? ReadTime { get; set; }

        /// <summary>
        /// 是否已弹窗
        /// </summary>
        public bool IsPopupShown { get; set; } = false;
    }
}