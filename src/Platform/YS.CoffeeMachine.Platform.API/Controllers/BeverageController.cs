using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformQueries.IBeverageQueries.IBeverageInfoQueries;
using MediatR;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries;
using YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
using YS.CoffeeMachine.Application.PlatformQueries.IBeverageQueries;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 饮品管理
    /// </summary>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class BeverageController(IMediator mediator, IFormulaInfosQueries formulaInfosQueries, IP_BeverageInfoQueries p_BeverageInfoQueries, IBeverageCollectionQueries p_BeverageCollectionQueries) : Controller
    {

        /// <summary>
        /// 饮品分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetBeverageInfoList")]
        public async Task<PagedResultDto<P_BeverageInfoDto>> GetBeverageInfoListAsync([FromBody] BeverageInfoInput request) => await p_BeverageInfoQueries.GetBeverageInfoListAsync(request);

        /// <summary>
        /// 创建饮品信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateBeverageInfo")]
        public async Task<bool> CreateBeverageInfo([FromBody] CreateBeverageInfoCommand command) => await mediator.Send(command);

        /// <summary>
        /// 修改饮品信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateBeverageInfo")]
        public async Task<bool> UpdateBeverageInfo([FromBody] UpdateBeverageInfoCommand command) => await mediator.Send(command);

        /// <summary>
        /// 删除饮品
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteBeverageInfo")]
        public async Task<bool> DeleteBeverageInfo([FromBody] DeleteBeverageInfoCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取历史版本（根据饮品id）
        /// </summary>
        /// <param name="beverageInfoId"></param>
        /// <returns></returns>
        [HttpGet("GetBeverageVersionList")]
        public async Task<List<P_BeverageVersionDto>> GetBeverageVersionListAsync(long beverageInfoId) => await p_BeverageInfoQueries.GetBeverageVersionListAsync(beverageInfoId);

        #region 饮品集合相关

        /// <summary>
        /// 创建饮品合集
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateBeverageCollection")]
        public async Task<bool> CreateBeverageCollection([FromBody] CreateP_BeverageCollectionCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取饮品集合分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetBeverageCollectionList")]
        public async Task<PagedResultDto<P_BeverageCollectionDto>> GetBeverageCollectionList([FromBody] P_BeverageCollectionInput request) => await p_BeverageCollectionQueries.GetBeverageCollectionList(request);

        /// <summary>
        /// 修改饮品集合名称
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateBeverageCollectionName")]
        public async Task<bool> UpdateP_BeverageCollectionName(UpdateP_BeverageCollectionNameCommand command) => await mediator.Send(command);

        /// <summary>
        /// 根据id删除饮品集合
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteBeverageCollectionName")]
        public async Task<bool> DeleteP_BeverageCollectionByIds([FromBody] DeleteP_BeverageCollectionCommand command) => await mediator.Send(command);
        #endregion

        #region 拓展
        /// <summary>
        /// 获取配方参数字典集合
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllSpecsJson")]
        public async Task<Dictionary<FormulaTypeEnum, string>> GetAllSpecsJson()
        {
            return await formulaInfosQueries.GetAllSpecsJson();

        }
        #endregion
    }
}
