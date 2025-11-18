using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.InternalMsgDtos
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class SystemMessagesInput : QueryRequest
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public InternalMsgEnum? MessageType { get; set; } = null;

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; private set; } = null;

        /// <summary>
        /// 目标用户Id
        /// </summary>
        public long? TargetUserId { get; set; } = null;

        /// <summary>
        /// 时间范围
        /// </summary>
        public List<DateTime>? DateTimes { get; set; } = null;
    }

    /// <summary>
    /// 用户信息
    /// </summary>

    public class UserMessagesInput : QueryRequest
    {
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool? IsRead { get; set; } = null;
    }

    /// <summary>
    /// UserReadGlobalMessagesInput
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