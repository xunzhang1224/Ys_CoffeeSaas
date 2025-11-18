using YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg;

namespace YS.CoffeeMachine.Application.Dtos.InternalMsgDtos
{
    /// <summary>
    /// UserReadGlobalMessagesDto
    /// </summary>
    public class UserReadGlobalMessagesDto
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public long MessageId { get; set; }

        /// <summary>
        /// 消息实体
        /// </summary>
        public SystemMessages Message { get; set; } = default!;

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 已读时间
        /// </summary>
        public DateTime ReadTime { get; set; } = DateTime.UtcNow;
    }
}