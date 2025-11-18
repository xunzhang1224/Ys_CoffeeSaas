using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.PlatformCommands.PaymentCommands;
using YS.CoffeeMachine.Application.PlatformDto.PaymentDtos;
using YS.CoffeeMachine.Application.PlatformQueries.IPaymentQueries;
using YS.CoffeeMachine.Platform.API.Extensions;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 支付配置
    /// </summary>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class PaymentController(IMediator mediator, IPaymentConfigQueries paymentConfigQueries) : Controller
    {
        /// <summary>
        /// 获取平台端支付配置
        /// </summary>
        /// <returns>平台端支付配置列表</returns>
        [HttpGet("GetPaymentConfigList")]
        public async Task<List<P_PaymetConfigDto>> GetPaymentConfigListAsync()
        {
            return await paymentConfigQueries.GetPaymentConfigList();
        }

        /// <summary>
        /// 更新平台端支付配置
        /// </summary>
        /// <param name="command">更新方法</param>
        /// <returns></returns>
        [HttpPut("UpdatePaymenConfig")]
        public async Task<bool> UpdatePaymenConfigAsync([FromBody] UpdatePaymentConfigCommand command) => await mediator.Send(command);

    }
}
