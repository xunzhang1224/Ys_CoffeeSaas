using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Dtos.BasicDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Platform.API.Queries.IOperationLog;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 操作日志
    /// </summary>
    /// <param name="queries"></param>
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class OperationLogController(IOperationLogQueries logQueries): Controller
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResultDto<PlatformOperationLog>> GetOperationLogAsync([FromBody] POperationLogInput input)
        {
            return await logQueries.GetOperationLogQueriesAsync(input);
        }
    }
}
