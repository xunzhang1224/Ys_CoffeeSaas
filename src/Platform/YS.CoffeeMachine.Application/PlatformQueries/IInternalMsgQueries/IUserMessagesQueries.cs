using YS.CoffeeMachine.Application.Dtos.InternalMsgDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.PlatformQueries.IInternalMsgQueries
{
    /// <summary>
    /// 用户消息查询
    /// </summary>
    public interface IUserMessagesQueries
    {
        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserMessagesDto>> GetPagedResultAsync(UserMessagesInput request);
    }
}
