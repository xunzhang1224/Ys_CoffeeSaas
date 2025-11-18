using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Dtos.OrderDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IOrderQueries;
using YS.CoffeeMachine.Platform.API.Extensions;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class OrderInfoController(IOrderInfoQueries orderInfoQueries) : Controller
    {
        /// <summary>
        /// 订单分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetOrderInfosPage")]
        public async Task<PagedResultDto<OrderInfoDto>> GetOrderInfosPageAsync([FromBody] OrderInfoInput input) => await orderInfoQueries.GetOrderInfosPageAsync(input);

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetOrderDetailsByOrderId")]
        public async Task<List<OrderDetailsDto>> GetOrderDetailsByOrderIdAsync([FromQuery] long id) => await orderInfoQueries.GetOrderDetailsByOrderIdAsync(id);
    }
}