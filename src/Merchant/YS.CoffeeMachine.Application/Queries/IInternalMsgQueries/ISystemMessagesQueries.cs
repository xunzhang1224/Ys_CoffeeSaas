using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.InternalMsgDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IInternalMsgQueries
{
    /// <summary>
    /// 系统消息查询接口
    /// </summary>
    public interface ISystemMessagesQueries
    {
        /// <summary>
        /// 获取系统消息分页结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<SystemMessagesDto>> GetPagedResultAsync(SystemMessagesInput request);

        /// <summary>
        /// 根据ID获取系统消息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SystemMessagesDto> GetSystemMessagesByIdAsync(long id);
    }
}
