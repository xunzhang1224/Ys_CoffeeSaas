using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.PaymentCommands;
using YS.CoffeeMachine.Application.Commands.PaymentCommands.PaypalStripePayment;
using YS.CoffeeMachine.Application.Dtos.PaymentDtos;
using YS.CoffeeMachine.Application.Queries.IPaymentInfoQueries;
using YS.Pay.SDK.Response;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 商户端支付相关接口
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class PaymentController(IMediator mediator, IPaymentQueries paymentQueries) : Controller
    {
        /// <summary>
        /// 创建支付配置基本信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreatePaymentConfig")]
        public async Task<MerchantIncomingResponse> CreatePaymentConfigAsync([FromBody] CreatePaymentConfigCommand command) => await mediator.Send(command);

        /// <summary>
        /// 支付平台回调
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(PublicResponse), 200)] // 当前api禁用框架统一数据返回格式，返回类型为 PublicResponse
        [HttpPost("PaymentPlatformCallback")]
        public async Task<PublicResponse> PaymentPlatformCallbackAsync([FromForm] PaymentCallbackCommand command) => await mediator.Send(command);

        /// <summary>
        /// 更新商户端支付配置的备注
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdatePaymentConfigRemark")]
        public async Task UpdatePaymentConfigRemarkAsync([FromBody] UpdatePaymentConfigRemarkCommand command) => await mediator.Send(command);

        /// <summary>
        /// 更新商户端支付配置的开启
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdatePaymentConfigEnabled")]
        public async Task UpdatePaymentConfigEnabledAsync([FromBody] UpdatePaymentConfigEnabledCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取符合条件的平台端支付配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPpaymentConfig")]
        public async Task<List<P_PaymentConfigDto>> GetPpaymentConfigAsync()
        {
            return await paymentQueries.GetPpaymentConfig();
        }

        /// <summary>
        /// 支付配置列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPaymentConfig")]
        public async Task<List<PaymentConfigDto>> GetPaymentConfigAsync()
        {
            return await paymentQueries.GetPaymentConfig();
        }

        /// <summary>
        /// 添加支付方式绑定设备
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("PaymentConfigBindDevice")]
        public async Task PaymentConfigBindDeviceAsync([FromBody] PaymentConfigBindDeviceCommand command) => await mediator.Send(command);

        /// <summary>
        /// 支付方式解绑设备
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("PaymentConfigNoBindDevice")]
        public async Task PaymentConfigNoBindDeviceAsync([FromBody] PaymentConfigNoBindDeviceCommand command) => await mediator.Send(command);
    }
}
