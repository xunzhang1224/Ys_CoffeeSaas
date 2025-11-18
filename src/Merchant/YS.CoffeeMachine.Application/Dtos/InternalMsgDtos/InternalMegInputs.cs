using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.InternalMsgDtos
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class SystemMessagesInput : QueryRequest
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; private set; } = null;

        /// <summary>
        /// 时间范围
        /// </summary>
        public List<DateTime>? DateTimes { get; set; } = null;
    }

    /// <summary>
    /// 用户消息
    /// </summary>
    public class UserMessagesInput : QueryRequest
    {
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool? IsRead { get; set; } = null;
    }

    /// <summary>
    /// 用户已读全局消息
    /// </summary>
    public class UserReadGlobalMessagesInput : QueryRequest
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public long? MessageId { get; set; } = null;
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; } = null;
    }
}
