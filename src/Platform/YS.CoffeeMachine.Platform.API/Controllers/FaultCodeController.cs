using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformCommands.FaultCodeCommands;
using YS.CoffeeMachine.Application.PlatformDto.BasicDtos;
using YS.CoffeeMachine.Application.PlatformQueries.IFaultCodeInfoQueries;
using YS.CoffeeMachine.Platform.API.Extensions;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 故障码管理
    /// </summary>
    /// <param name="mediator"></param>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
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
        public async Task<bool> UpdateFaultCodeAsync([FromBody] UpdateFaultCodeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 删除故障码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteFaultCode")]
        public async Task<bool> DeleteFaultCodeAsync([FromBody] DeleteFaultCodeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取故障码列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetFalutCodeList")]
        public async Task<PagedResultDto<FaultCodeDto>> GetFalutCodeListAsync([FromBody] FaultCodeInput input)
        {
            return await _faultCodeInfoQueries.GetFalutCodeList(input);
        }
    }
}
