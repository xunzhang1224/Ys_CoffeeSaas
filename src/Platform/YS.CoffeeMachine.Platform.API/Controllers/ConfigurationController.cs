using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.ServiceProviderDtos;
using YS.CoffeeMachine.Application.PlatformCommands.ServiceProviderInfoCommands;
using YS.CoffeeMachine.Application.Queries.IServiceProviderInfoQueries;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// ConfigurationController
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="serviceProviderInfoQueries"></param>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class ConfigurationController(IMediator mediator, IServiceProviderInfoQueries serviceProviderInfoQueries) : Controller
    {
        #region 服务商相关
        /// <summary>
        /// 添加服务商
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateServiceProviderInfo")]
        public Task<bool> CreateServiceProviderInfo([FromBody] CreateServiceProviderInfoCommand command) => mediator.Send(command);
        /// <summary>
        /// 获取服务商分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetServiceProviderInfoList")]
        public Task<PagedResultDto<ServiceProviderInfoDto>> GetServiceProviderInfoList([FromBody] QueryRequest request) => serviceProviderInfoQueries.GetServiceProviderInfoListAsync(request);
        /// <summary>
        /// 编辑服务商
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateServiceProviderInfo")]
        public Task<bool> UpdateServiceProviderInfo([FromBody] UpdateServiceProviderInfoCommand command) => mediator.Send(command);
        /// <summary>
        /// 删除服务商
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteServiceProviderInfo")]
        public Task<bool> DeleteServiceProviderInfo([FromBody] DeleteServiceProviderInfoCommand command) => mediator.Send(command);
        #endregion
    }
}
