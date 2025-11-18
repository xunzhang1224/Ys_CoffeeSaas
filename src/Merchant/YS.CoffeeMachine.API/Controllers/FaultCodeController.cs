using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.BasicCommands.FaultCodeCommands;
using YS.CoffeeMachine.Application.Dtos.BasicDtos.FaultCode;
using YS.CoffeeMachine.Application.Queries.BasicQueries.FaultCode;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 故障码管理
    /// </summary>
    /// <param name="mediator"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class FaultCodeController(IMediator mediator, IFaultCodeInfoQueries _faultCodeInfoQueries) : Controller
    {
        /// <summary>
        /// 创建故障码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateFaultCode")]
        public async Task<bool> CreateFaultCodeAsync([FromBody] FaultCodeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 更新故障码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateFaultCode")]
        public async Task<bool> UpdateFaultCodeAsync([FromBody] FaultCodeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 删除故障码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteFaultCode")]
        public async Task<bool> DeleteFaultCodeAsync([FromBody] DeleteFaultCodeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 根据故障码类型获取故障码
        /// </summary>
        [HttpGet("GetFaultCodeByType")]
        public async Task<List<FaultCodeDto>> GetFaultCodeByTypeAsync(FaultCodeTypeEnum type)
        {
            return await _faultCodeInfoQueries.GetFaultCodeByType(type);
        }
    }
}