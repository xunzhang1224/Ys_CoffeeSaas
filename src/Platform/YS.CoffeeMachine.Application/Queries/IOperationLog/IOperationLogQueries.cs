using YS.CoffeeMachine.Application.Dtos.BasicDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Platform.API.Queries.IOperationLog
{
    /// <summary>
    /// 平台日志查询
    /// </summary>
    public interface IOperationLogQueries
    {
        /// <summary>
        /// 查询平台操作日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PlatformOperationLog>> GetOperationLogQueriesAsync(POperationLogInput input);
    }
}
