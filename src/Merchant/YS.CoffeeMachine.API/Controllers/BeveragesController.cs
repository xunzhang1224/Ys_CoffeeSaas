using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.BeverageWarehouseDtos;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries.IBeverageCollectionQueries;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries.IBeverageInfoTemplateQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 饮品管理
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="formulaInfosQueries"></param>
    /// <param name="beverageInfoQueries"></param>
    /// <param name="beverageInfoTemplateQueries"></param>
    /// <param name="beverageCollectionInfoQueries"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class BeveragesController(IMediator mediator, IFormulaInfosQueries formulaInfosQueries, IBeverageInfoQueries beverageInfoQueries, IBeverageInfoTemplateQueries beverageInfoTemplateQueries, IBeverageCollectionInfoQueries beverageCollectionInfoQueries) : Controller
    {
        #region 饮品信息

        /// <summary>
        /// 创建饮品信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateBeverageInfo")]
        public Task<string> CreateBeverageInfo([FromBody] CreateBeverageInfoCommand command) => mediator.Send(command);

        /// <summary>
        /// 饮品添加到饮品库
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("AddBeverageInfoTemplate")]
        public Task<bool> AddBeverageInfoTemplate([FromBody] AddBeverageInfoTemplateCommand command) => mediator.Send(command);

        /// <summary>
        /// 根据Id获取饮品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetBeverageInfoById")]
        public Task<BeverageInfoDto> GetBeverageInfoById([FromQuery] long id) => beverageInfoQueries.GetBeverageInfoAsync(id);

        /// <summary>
        /// 应用指定设备所有饮品信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("AppliedAllBevergeInfoByDeviceId")]
        public Task<CommandDownSends> AppliedAllBevergeInfoByDeviceId([FromBody] AppliedAllBevergeInfoCommand command) => mediator.Send(command);

        /// <summary>
        /// 原饮品应用到设备饮品
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("AppliedBeverageToBeverage")]
        public Task<DrinkCommandDownSends> AppliedBeverageToBeverage([FromBody] B_AppliedBeverageInfoCommand command) => mediator.Send(command);

        /// <summary>
        /// 根据设备Id获取饮品列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPost("GetBeverageListByDeviceId")]
        public Task<PagedResultDto<BeveragePageListDto>> GetBeverageListByDeviceId([FromBody] QueryRequest request, [FromQuery] long deviceId) => beverageInfoQueries.GetBeverageInfoListAsync(request, deviceId);

        /// <summary>
        /// 根据设备Id获取饮品价格信息列表
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPost("GetPriceInfoList")]
        public Task<List<PriceInfoDot>> GetPriceInfoListAsync([FromQuery] long deviceId) => beverageInfoQueries.GetPriceInfoListAsync(deviceId);

        /// <summary>
        /// 更新饮品价格
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateMultiBeveragePrice")]
        public Task<bool> UpdateBeveragePrice([FromBody] UpdateBevergePricesCommand command) => mediator.Send(command);

        /// <summary>
        /// 编辑饮品信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateBeverageInfo")]
        public Task<bool> UpdateBeverageInfo([FromBody] UpdateBeverageInfoCommand command) => mediator.Send(command);

        /// <summary>
        /// 删除饮品信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteBeverageInfo")]
        public Task<bool> DeleteBeverageInfo([FromBody] DeleteBeverageInfoCommand command) => mediator.Send(command);

        /// <summary>
        /// 批量修改设备下饮品的价格
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateBeveragePrice")]
        public async Task<bool> UpdateBeveragePriceAsync([FromBody] UpdateBeveragePriceCommand command) => await mediator.Send(command);

        /// <summary>
        /// 修改设备下饮品排序
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateBeverageSort")]
        public async Task<bool> UpdateBeverageSortAsync([FromBody] UpdateBeverageSortCommand command) => await mediator.Send(command);
        #endregion

        #region 饮品库

        /// <summary>
        /// 创建饮品模板信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateBeverageInfoTemplate")]
        public Task<bool> CreateBeverageInfoTemplate([FromBody] CreateBeverageInfoTemplateCommand command) => mediator.Send(command);

        /// <summary>
        /// 根据Id获取饮品模板信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetBeverageInfoTemplateById")]
        public Task<BeverageInfoTemplateDto> GetBeverageInfoTemplateById([FromQuery] long id) => beverageInfoTemplateQueries.GetBeverageInfoTemplateAsync(id);

        /// <summary>
        /// 根据企业Id获取饮品模板列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="enterpriseInfoId"></param>
        /// <param name="formulaType"></param>
        /// <returns></returns>
        [HttpPost("GetBeverageTemplateListByentErpriseInfoId")]
        public Task<PagedResultDto<BeverageInfoTemplateDto>> GetBeverageTemplateListByentErpriseInfoId([FromBody] QueryRequest request, [FromQuery] long enterpriseInfoId, [FromQuery] FormulaTypeEnum? formulaType)
            => beverageInfoTemplateQueries.GetBeverageInfoTemplateListAsync(request, enterpriseInfoId, formulaType);

        /// <summary>
        /// 根据饮品Id验证SKU是否存在饮品库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("VerifySkuByBeverageId")]
        public Task<bool?> VerifySkuByBeverageId([FromQuery] long id) => beverageInfoTemplateQueries.VerifySkuByBeverageIdAsync(id);

        /// <summary>
        /// 饮品库饮品应用到设备饮品
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("AppliedTemplateToBeverage")]
        public Task<DrinkCommandDownSends> AppliedTemplateToBeverage([FromBody] AppliedBeverageInfoCommand command) => mediator.Send(command);

        /// <summary>
        /// 编辑饮品模板信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateBeverageInfoTemplate")]
        public Task<bool> UpdateBeverageInfoTemplate([FromBody] UpdateBeverageInfoTemplateCommand command) => mediator.Send(command);

        /// <summary>
        /// 删除饮品模板信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteBeverageInfoTemplate")]
        public Task<bool> DeleteBeverageInfoTemplate([FromBody] DeleteBeverageInfoTemplateCommand command) => mediator.Send(command);
        #endregion

        #region 饮品合集

        /// <summary>
        /// 添加饮品合集
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateBeverageCollection")]
        public Task<bool> CreateBeverageCollection([FromBody] CreateBeverageCollectionCommand command) => mediator.Send(command);

        /// <summary>
        /// 获取饮品集合
        /// </summary>
        /// <param name="request"></param>
        /// <param name="enterpriseInfoId"></param>
        /// <returns></returns>
        [HttpPost("GetBeverageColltionListByentErpriseInfoId")]
        public Task<PagedResultDto<BeverageCollectionDto>> GetBeverageColltionListByentErpriseInfoId([FromBody] QueryRequest request, [FromQuery] long enterpriseInfoId)
            => beverageCollectionInfoQueries.GetBeverageColltionListAsync(request, enterpriseInfoId);

        /// <summary>
        /// 获取平台端饮品集合下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetP_BeverageCollections")]
        public async Task<List<P_BeverageCollectionSelectedDto>> GetP_BeverageCollectionsAsync() => await beverageCollectionInfoQueries.GetP_BeverageCollectionsAsync();

        /// <summary>
        /// 应用默认饮品集合配方到指定设备
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("AppliedDefaultBeverageCollection")]
        public async Task<CommandDownSends> AppliedDefaultBeverageCollectionAsync([FromBody] AppliedDefaultBeverageCollectionCommand command) => await mediator.Send(command);

        /// <summary>
        /// 饮品合集应用到指定设备
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("AppliedBeverageByCollection")]
        public Task<CommandDownSends> AppliedBeverageByCollection([FromBody] AppliedBeverageCollectionCommand command) => mediator.Send(command);

        /// <summary>
        /// 编辑饮品集合
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateBeverageColltion")]
        public Task<bool> UpdateBeverageColltion([FromBody] UpdateBeverageCollectionCommand command) => mediator.Send(command);

        /// <summary>
        /// 删除饮品集合
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteBeverageColltion")]
        public Task<bool> DeleteBeverageColltion([FromBody] DeleteBeverageCollectionCommand command) => mediator.Send(command);
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