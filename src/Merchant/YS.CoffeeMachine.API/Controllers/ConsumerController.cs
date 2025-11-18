using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.ConsumerCommands.ConsumerUserCommands;
using YS.CoffeeMachine.Application.Commands.MarketingActivitysCommands;
using YS.CoffeeMachine.Application.Dtos.Consumer.MarketingActivitys;
using YS.CoffeeMachine.Application.Dtos.ConsumerDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.Consumer;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;
using static YS.CoffeeMachine.Application.Dtos.Consumer.MarketingActivitys.PromotionOutput;

namespace YS.CoffeeMachine.API.Controllers10
{
    /// <summary>
    /// 消费端接口
    /// </summary>
    /// <param name="_promotionqueries"></param>
    [Authorize]
    [Route("capi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Consumer))]
    public class ConsumerController(IPromotionQueries _promotionqueries, IMediator mediator, IDeviceInfoQueries _deviceInfoQueries) : Controller
    {
        #region 用户相关

        /// <summary>
        /// 消费者端登录
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ClientLogin")]
        public async Task<ClientLoginResponseDto> ClientLogin([FromBody] ClientUserLoginCommand command) => await mediator.Send(command);

        /// <summary>
        /// 消费者端注册
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ClientUserRegister")]
        public async Task<bool> ClientUserRegister([FromBody] ClientUserRegisterCommand command) => await mediator.Send(command);

        /// <summary>
        /// 消费者端刷新token
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ClientUserRefresh")]
        public async Task<ClientLoginResponseDto> ClientUserRefresh([FromBody] ClientUserRefreshCommand command) => await mediator.Send(command);
        #endregion

        /// <summary>
        /// 获取营销活动列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetPromotionPageList")]
        public async Task<PagedResultDto<PromotionOutput>> GetPromotionPageList([FromBody] GetPromotionInput request)
        {
            return await _promotionqueries.GetPromotionPageList(request);
        }

        /// <summary>
        /// 可用优惠劵
        /// </summary>
        /// <param name="enterpriseinfoId">不为空时获取指定租户下的优惠劵</param>
        /// <returns></returns>
        [HttpGet("GetCouponList")]
        public async Task<List<Coupon>> GetCouponList(long? enterpriseinfoId = 0)
        {
            return await _promotionqueries.GetCouponList(enterpriseinfoId);
        }

        /// <summary>
        /// 当前登录用户可领优惠劵列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPromotionList")]
        public async Task<List<Promotion>> GetPromotionList()
        {
            return await _promotionqueries.GetPromotionList();
        }

        /// <summary>
        /// 新增营销活动
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("AddPromotion")]
        public async Task<bool> AddPromotion([FromBody] AddPromotionCommand command) => await mediator.Send(command);

        /// <summary>
        /// 修改营销活动
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdatePromotion")]
        public async Task<bool> UpdatePromotion([FromBody] UpdatePromotionCommand command) => await mediator.Send(command);

        /// <summary>
        /// 点单获取设备
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetDecentlyDevicePageList")]
        public async Task<PagedResultDto<GetDecentlyDevicePageListOutput>> GetDecentlyDevicePageList([FromQuery] GetDecentlyDevicePageListInput request)
        {
            return await _deviceInfoQueries.GetDecentlyDevicePageList(request);
        }

    }
}
