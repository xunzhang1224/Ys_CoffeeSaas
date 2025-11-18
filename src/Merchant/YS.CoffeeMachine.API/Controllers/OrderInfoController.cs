using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Dtos.OrderDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IOrderQueries;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class OrderInfoController(IOrderInfoQueries orderInfoQueries) : Controller
    {
        /// <summary>
        /// 查询订单汇总信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetOrderTotalInfo")]
        public async Task<OrderTotalInfo> GetOrderTotalInfoAsync([FromBody] OrderInfoInput input) => await orderInfoQueries.GetOrderTotalInfoAsync(input);

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