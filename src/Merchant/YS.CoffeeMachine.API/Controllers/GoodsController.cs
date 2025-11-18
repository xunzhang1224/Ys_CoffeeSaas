using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.API.Queries.GoodsQueries;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceBaseCommands;
using YS.CoffeeMachine.Application.Commands.Goods;
using YS.CoffeeMachine.Application.Dtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IGoods;
using YS.CoffeeMachine.Domain.AggregatesModel.Goods;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 商品服务
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class GoodsController(IMediator mediator, IGoodsQueries _queries) : Controller
    {
        /// <summary>
        /// 设备补货
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("AddGoods")]
        public async Task<bool> AddGoods([FromBody] AddPrivateGoodsCommand command) => await mediator.Send(command);

        /// <summary>
        /// 设备补货
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SetGoodsStatus")]
        public async Task<bool> SetGoodsStatus([FromBody] SetPrivateGoodsStatusCommand command) => await mediator.Send(command);

        /// <summary>
        /// 设备补货
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateGoods")]
        public async Task<bool> UpdateGoods([FromBody] UpdatePrivateGoodsCommand command) => await mediator.Send(command);

        /// <summary>
        /// 私有库列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetPrivateGoodsPage")]
        public async Task<PagedResultDto<PrivateGoodsRepository>> GetPrivateGoodsPage([FromBody] GoodsDto dto)
        {
            return await _queries.GetPrivateGoodsPage(dto);
        }
    }
}
