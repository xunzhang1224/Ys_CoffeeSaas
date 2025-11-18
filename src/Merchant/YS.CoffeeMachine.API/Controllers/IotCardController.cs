using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.API.Queries.IotCardInfoQueries;
using YS.CoffeeMachine.Application.Dtos.IotCardDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.IServices.ILotCartService;
using YS.CoffeeMachine.Application.Queries.IIotCardInfoQueries;
using YSCore.Base.UnifyResult.Attributes;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 物联网卡控制器
    /// </summary>
    /// <param name="_lotCardApi"></param>
    /// <param name="_iotCardInfoQueries"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class IotCardController(ILotCardApi _lotCardApi, IIotCardInfoQuerie _iotCardInfoQueries) : Controller
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [NonUnify]
        [HttpPost("Signin")]
        public async Task<HttpLotCardSingleResult<LotCardUserInfo>> SigninAsync([FromBody] LotCardAccountLoginInput input) => await _lotCardApi.SigninAsync(input);

        /// <summary>
        /// 获取最佳策略
        /// </summary>
        /// <param name="iccid"></param>
        /// <param name="account"></param>
        /// <param name="accountFrom"></param>
        /// <returns></returns>
        [HttpGet("GetBestPackage")]
        public async Task<HttpLotCardResult<PoclicyPackageOut>> GetBestPackage(string iccid, string account = "1", string accountFrom = "1") => await _lotCardApi.GetBestPackage(iccid, account, accountFrom);

        /// <summary>
        /// 获取流量卡锁卡状态
        /// </summary>
        /// <param name="iccid"></param>
        /// <returns></returns>
        [NonUnify]
        [HttpGet("GetCardLockStatus/{iccid}")]
        public async Task<HttpLotCardSingleResult<CardLockStatusOut>> GetCardLockStatus([FromRoute] string iccid) => await _lotCardApi.GetCardLockStatus(iccid);

        /// <summary>
        /// 获取流量卡充值信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [NonUnify]
        [HttpPost("GetPoclicyDetail")]
        public async Task<HttpLotCardResult<List<PoclicyDetailOut>>> GetPoclicyDetail([FromBody] List<IotCardInput> input) => await _lotCardApi.GetPoclicyDetail(input);

        /// <summary>
        /// 获取物联网卡分页信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetVendIotCardPage")]
        public async Task<PagedResultDto<VendIotCardOut>> GetVendIotCardPageAsync([FromBody] IotCardQueryInput input) => await _iotCardInfoQueries.GetVendIotCardPageAsync(input);
    }
}