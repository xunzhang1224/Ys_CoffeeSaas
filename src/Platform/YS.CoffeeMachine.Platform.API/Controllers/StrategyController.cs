using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Commands.TermServiceCommands;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.TermServiceDtos;
using YS.CoffeeMachine.Application.PlatformCommands.StrategyCommands.AreaRelationCommands;
using YS.CoffeeMachine.Application.PlatformDto.StrategyDtos;
using YS.CoffeeMachine.Application.PlatformQueries.StrategyQueries;
using YS.CoffeeMachine.Platform.API.Extensions;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 策略管理
    /// </summary>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class StrategyController(IMediator mediator, IStrategyQueries strategyQueries) : Controller
    {
        #region 地区关联
        /// <summary>
        /// 创建地区关联
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateAreaRelation")]
        public async Task<bool> CreateAreaRelationAsync([FromBody] CreateAreaRelationCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取地区关联列表
        /// </summary>
        /// <param name="request">分页信息</param>
        /// <returns></returns>
        [HttpPost("GetAreaRelationList")]
        public async Task<PagedResultDto<AreaRelationDto>> GetAreaRelationList([FromBody] QueryRequest request)
        {
            return await strategyQueries.GetAreaRelationList(request);
        }

        /// <summary>
        /// 获取企业所需地区关联列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAreaRelationAllList")]
        public async Task<List<AreaRelationDto>> GetAreaRelationAllListAsync()
        {
            return await strategyQueries.GetAreaRelationAllList();
        }

        /// <summary>
        /// 修改地区关联
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateAreaRelation")]
        public async Task<bool> UpdateAreaRelationAsync([FromBody] UpdateAreaRelationCommand command) => await mediator.Send(command);

        /// <summary>
        /// 根据id删除地区关联
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteAreaRelation")]
        public async Task<bool> DeleteAreaRelationAsync([FromQuery] DeleteAreaRelationCommand command) => await mediator.Send(command);
        #endregion

        #region 服务条款

        /// <summary>
        /// 创建服务条款
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateTermService")]
        public async Task<bool> CreateTermServiceAsync([FromBody] CreateTermServiceCommand command) => await mediator.Send(command);

        /// <summary>
        /// 编辑服务条款
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateTermService")]
        public async Task<bool> UpdateTermServiceAsync([FromBody] UpdateTermServiceCommand command) => await mediator.Send(command);

        /// <summary>
        /// 删除服务条款
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteTermService")]
        public async Task<bool> DeleteTermServiceAsync([FromQuery] DeleteTermServiceCommand command) => await mediator.Send(command);

        /// <summary>
        /// 变更服务条款状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("ChangeTermServiceStatus")]
        public async Task<bool> ChangeTermServiceStatus([FromBody] ChangeTermServiceStatusCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取服务条款分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetTermServicePageList")]
        public async Task<PagedResultDto<TermServiceOutput>> GetTermServicePageList([FromBody] TermServiceInput request) => await strategyQueries.GetTermServicePageList(request);

        /// <summary>
        /// 获取单个服务条款信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTermServiceSelectList")]
        public async Task<List<TermServiceSelectOutput>> GetTermServiceSelectList() => await strategyQueries.GetTermServiceSelectList();

        /// <summary>
        /// 获取单个服务条款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSingleTermServiceById")]
        public async Task<SingleTermServiceOutput> GetSingleTermServiceById([FromQuery] long id) => await strategyQueries.GetSingleTermServiceById(id);
        #endregion
    }
}