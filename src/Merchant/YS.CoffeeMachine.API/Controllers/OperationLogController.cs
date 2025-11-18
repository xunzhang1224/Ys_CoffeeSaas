using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.API.Queries.IOperationLog;
using YS.CoffeeMachine.Application.Dtos.BasicDtos;
using YS.CoffeeMachine.Application.Dtos.LogDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 日志
    /// </summary>
    /// <param name="logQueries"></param>
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    [Authorize]
    public class OperationLogController(IOperationLogQueries logQueries) : Controller
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("OperationLogAsync")]
        public async Task<PagedResultDto<OperationLogQueriesDto>> GetOperationLogAsync([FromBody] OperationLogInput dto)
        {
            return await logQueries.GetOperationLogQueriesAsync(dto);
        }
    }
}
