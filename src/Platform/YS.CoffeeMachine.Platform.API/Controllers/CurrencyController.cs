using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Commands.SettingsCommands.SettingInfoCommands;
using YS.CoffeeMachine.Application.PlatformCommands.CurrencyCommands;
using YS.CoffeeMachine.Application.PlatformDto.CurrencyDtos;
using YS.CoffeeMachine.Application.PlatformQueries.ICurrencyQueries;
using YS.CoffeeMachine.Platform.API.Extensions;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 币种管理
    /// </summary>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class CurrencyController(IMediator mediator, ICurrencyInfoQueries currencyInfoQueries) : Controller
    {
        /// <summary>
        /// 创建币种
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateCurrency")]
        public async Task<bool> CreateCurrency([FromBody] CreateCurrencyCommand command) => await mediator.Send(command);

        /// <summary>
        /// 币种查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrencyList")]
        public async Task<List<CurrencyDto>> GetCurrencyList()
        {
            return await currencyInfoQueries.GetCurrencyList();
        }

        /// <summary>
        /// 更新币种
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateCurrency")]
        public async Task<bool> UpdateCurrency([FromBody] UpdateCurrencyCommand command) => await mediator.Send(command);
    }
}