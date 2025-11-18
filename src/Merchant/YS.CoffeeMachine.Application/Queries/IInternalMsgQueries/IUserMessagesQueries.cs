using YS.CoffeeMachine.Application.Dtos.InternalMsgDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IInternalMsgQueries
{
    /// <summary>
    /// 用户消息查询接口
    /// </summary>
    public interface IUserMessagesQueries
    {
        /// <summary>
        /// 获取用户消息分页结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserMessagesDto>> GetPagedResultAsync(UserMessagesInput request);
    }
}