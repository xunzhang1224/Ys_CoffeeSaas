using YS.CoffeeMachine.Application.Dtos.BasicDtos;
using YS.CoffeeMachine.Application.Dtos.LogDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.API.Queries.IOperationLog
{
    /// <summary>
    /// 操作日志查询
    /// </summary>
    public interface IOperationLogQueries
    {
        /// <summary>
        /// 新增的查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<OperationLogDto> GetOperationLogAsync(string code);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<PagedResultDto<OperationLogQueriesDto>> GetOperationLogQueriesAsync(OperationLogInput dto);
    }
}
