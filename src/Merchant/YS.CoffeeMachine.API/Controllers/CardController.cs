using Aop.Api.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.CardCommands;
using YS.CoffeeMachine.Application.Dtos.Card;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.CardQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Card;
using CardInfo = YS.CoffeeMachine.Domain.AggregatesModel.Card.CardInfo;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 卡管理
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class CardController(IMediator mediator,ICardQueries _cardQueries) : Controller
    {
        /// <summary>
        /// 创建卡
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateCardAsync")]
        public Task<bool> CreateCardAsync([FromBody] CreateCardCommand command) => mediator.Send(command);

        /// <summary>
        /// 禁用/启用
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateAsync")]
        public Task<bool> UpdateAsync([FromBody] UpdateCommand command) => mediator.Send(command);

        /// <summary>
        /// 获取卡信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetCradInfosAsync")]
        public async Task<PagedResultDto<CardInfo>> GetCradInfosAsync([FromBody]CardDto query) => await _cardQueries.GetCardInfosAsync(query);

        /// <summary>
        /// 卡绑定
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("CardBindAsync")]
        public Task<bool> CardBindAsync([FromBody] CardBindDeviceCommand command) => mediator.Send(command);

        /// <summary>
        /// 获取卡绑定的设备
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpPost("GetCardInfosByIdAsync")]
        public async Task<List<long>> GetCardInfosByIdAsync(long query) => await _cardQueries.GetCardInfosByIdAsync(query);

        /// <summary>
        /// 获取当前设备绑定的卡
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpPost("GetCardInfosByDeviceIdAsync")]
        public async Task<List<CardInfo>> GetCardInfosByDeviceIdAsync(long query) => await _cardQueries.GetCardInfosByDeviceIdAsync(query);
    }
}
