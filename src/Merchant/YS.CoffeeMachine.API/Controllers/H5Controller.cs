using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceBaseCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceUserAssociationCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands;
using YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands;
using YS.CoffeeMachine.Application.Dtos;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.DeviceDots.Groups;
using YS.CoffeeMachine.Application.Dtos.OrderDtos;
using YS.CoffeeMachine.Application.Dtos.OrderDtos.H5OrderDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.ReportsDtos;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Application.Queries.IOrderQueries;
using YS.CoffeeMachine.Application.Queries.IReportsQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// H5接口
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.H5))]
    public class H5Controller(IDeviceInfoQueries deviceInfoQueries, IGroupsQueries groupsQueries,
        IApplicationUserQueries applicationUserQueries, IBeverageInfoQueries beverageInfoQueries,
        IMediator mediator, IDeviceBaseQueries _deviceBaseQueries, IOrderInfoQueries orderInfoQueries,
        IDictionaryQueries dictionaryQueries, IReportsQuerie reportsQuerie) : Controller
    {
        /// <summary>
        /// 设备下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDeviceSelectList")]
        public async Task<List<DeviceSelectDto>> GetDeviceSelectListAsync() => await deviceInfoQueries.GetDeviceSelectListAsync();

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceInfoList")]
        public Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoList([FromBody] DevicesListInput request)
        {
            return deviceInfoQueries.GetDeviceInfoListAsync(request);
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceH5InfoList")]
        public Task<PagedResultDto<DeviceH5Dto>> GetDeviceH5InfoList([FromBody] DevicesH5ListInput request)
        {
            return deviceInfoQueries.GetDeviceInfoH5ListAsync(request);
        }

        /// <summary>
        /// 获取设备列表统计
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDeviceCount")]
        public Task<DeviceCountOutput> GetDeviceCount()
        {
            return deviceInfoQueries.GetDeviceCountOutput();
        }

        /// <summary>
        /// 运营概况
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOperationCount")]
        public Task<SyCountOutput> GetOperationCount(List<DateTime> times)
        {
            return deviceInfoQueries.GetSyCount(times);
        }

        /// <summary>
        /// 今日盈利统计
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHourlyRevenueStatsFromDbAsync")]
        public Task<List<HourlyRevenueStats>> GetHourlyRevenueStatsFromDbAsync(List<DateTime> times, int offset, int hoursPerSlot = 1)
        {
            return deviceInfoQueries.GetHourlyRevenueStatsFromDbAsync(times, offset, hoursPerSlot);
        }

        /// <summary>
        /// 每月经营收入
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOperatingRevenueByMonthCount")]
        public Task<List<OperatingRevenueOutput>> GetOperatingRevenueByMonth(List<DateTime> times, int offset)
        {
            return deviceInfoQueries.GetOperatingRevenueByMonth(times, offset);
        }

        /// <summary>
        /// 设备统计
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDeviceReportCount")]
        public Task<DeviceReportCountOutput> GetDeviceReportCount()
        {
            return deviceInfoQueries.GetDeviceReportCount();
        }

        /// <summary>
        /// 设备销售排行
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceSaleRank")]
        public Task<List<DeviceSaleRank>> GetDeviceSaleRank(List<DateTime> times)
        {
            return deviceInfoQueries.GetDeviceSaleRank(times);
        }

        /// <summary>
        /// 商品销售排行
        /// </summary>
        /// <param name="times"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet("GetGoodsSaleRank")]
        public Task<List<GoodsSaleRank>> GetGoodsSaleRank(List<DateTime> times, int top)
        {
            return deviceInfoQueries.GetGoodsSaleRank(times, top);
        }

        /// <summary>
        /// 根据设备Id获取饮品列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPost("GetBeverageListByDeviceId")]
        public Task<PagedResultDto<BeveragePageListDto>> GetBeverageListByDeviceId([FromBody] QueryRequest request, [FromQuery] long deviceId) => beverageInfoQueries.GetBeverageInfoListAsync(request, deviceId);

        /// <summary>
        /// 设备补货
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("DeviceRestock")]
        public async Task<bool> DeviceRestock([FromBody] DeviceRestockCommand command) => await mediator.Send(command);

        /// <summary>
        /// 设备补货记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceRestockLogs")]
        public async Task<PagedResultDto<DeviceRestockLogDto>> GetDeviceRestockLogs([FromBody] GetDeviceRestockLogInput input)
        {
            return await _deviceBaseQueries.GetDeviceRestockLogs(input);
        }

        /// <summary>
        /// 设备补货详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceRestockSubLogs/{id}")]
        public async Task<DeviceRestockLog> GetDeviceRestockSubLogs(long id)
        {
            return await _deviceBaseQueries.GetDeviceRestockSubLogs(id);
        }

        /// <summary>
        /// 获取物料信息与预警
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceWarnings")]
        public async Task<List<DeviceEarlyWarningsOutput>> GetDeviceWarnings(long deviceBaseId) => await _deviceBaseQueries.GetDeviceWarnings(deviceBaseId);

        /// <summary>
        /// 获取设备其他信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceOtherMsg")]
        public async Task<List<DeviceOtherMsgDto>> GetDeviceOtherMsgAsync(long deviceId) => await _deviceBaseQueries.GetDeviceOtherMsg(deviceId);

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceCapacityCfgs")]
        public async Task<List<DeviceCapacityCfg>> GetDeviceCapacityCfgs(long deviceBaseId) => await _deviceBaseQueries.GetDeviceCapacityCfgs(deviceBaseId);

        /// <summary>
        /// 获取指标
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceMetrics")]
        public async Task<List<DeviceMetricsOutput>> GetDeviceMetrics(long deviceBaseId) => await _deviceBaseQueries.GetDeviceMetrics(deviceBaseId);

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

        #region H5订单信息

        /// <summary>
        /// H5订单分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetH5OrderInfosPage")]
        public async Task<PagedResultDto<H5OrderInfoDto>> GetH5OrderInfosPageAsync([FromBody] H5OrderInfoInput input) => await orderInfoQueries.GetH5OrderInfosPageAsync(input);

        /// <summary>
        /// 获取H5订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetH5OrderRefundDetailsByOrderId")]
        public async Task<H5OrderDetailsDto> GetH5OrderRefundDetailsByOrderIdAsync([FromQuery] long id) => await orderInfoQueries.GetH5OrderRefundDetailsByOrderIdAsync(id);

        /// <summary>
        /// 根据主订单Id获取订单退款列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetH5OrderRefundDetailListByOrderId")]
        public async Task<List<H5OrderRefundDetailListDto>> GetH5OrderRefundDetailListByOrderIdAsync([FromQuery] long id) => await orderInfoQueries.GetH5OrderRefundDetailListByOrderIdAsync(id);

        /// <summary>
        /// 国内支付订单退款
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("DomesticPaymentOrderRefund")]
        public async Task<bool> DomesticPaymentOrderRefundAsync([FromBody] OrderRefundCommand command) => await mediator.Send(command);
        #endregion

        /// <summary>
        /// 编辑设备信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateDeviceInfoByDic")]
        public Task<bool> UpdateDeviceInfoByDic([FromBody] UpdateDeviceInfoByDicCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 通过Id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceInfoById/{id}")]
        public Task<DeviceInfoDto> GetDeviceInfoById(long id)
        {
            return deviceInfoQueries.GetDeviceAsync(id);
        }

        /// <summary>
        /// 激活设备
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ActiveDevice")]
        public async Task ActiveDeviceAsync([FromBody] ActiveDeviceCommand command) => await mediator.Send(command);

        /// <summary>
        /// 修改设备下饮品排序
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateBeverageSort")]
        public async Task<bool> UpdateBeverageSortAsync([FromBody] UpdateBeverageSortCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取分组分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetGroupPageList")]
        public Task<PagedResultDto<GroupListDto>> GetGroupPageList([FromBody] GroupListInput input) => groupsQueries.GetGroupPageList(input);

        /// <summary>
        /// 根据企业Id获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationUserByEnterpriseIdList")]
        public Task<PagedResultDto<ApplicationUserDto>> GetApplicationUserByEnterpriseIdList([FromBody] ApplicationUserInput input)
        {
            return applicationUserQueries.GetApplicationUserByEnterpriseIdListAsync(input);
        }

        /// <summary>
        /// 设备多对多绑定用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("BindDeviceUserAssociation")]
        public Task<bool> BindDeviceUserAssociation([FromBody] BindDeviceUserAssociationCommand command) => mediator.Send(command);

        /// <summary>
        /// 设备绑定分组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("DeviceBindGroups")]
        public Task<bool> DeviceBindGroups([FromBody] DeviceBindGroupsCommand command) => mediator.Send(command);

        /// <summary>
        /// 创建饮品信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateBeverageInfo")]
        public Task<string> CreateBeverageInfo([FromBody] CreateBeverageInfoCommand command) => mediator.Send(command);

        /// <summary>
        /// 编辑饮品信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateBeverageInfo")]
        public Task<bool> UpdateBeverageInfo([FromBody] UpdateBeverageInfoCommand command) => mediator.Send(command);

        /// <summary>
        /// 原饮品应用到设备饮品
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("AppliedBeverageToBeverage")]
        public Task<DrinkCommandDownSends> AppliedBeverageToBeverage([FromBody] B_AppliedBeverageInfoCommand command) => mediator.Send(command);

        /// <summary>
        /// 编辑分组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateGroups")]
        public Task<bool> UpdateGroups([FromBody] UpdateGroupsCommand command) => mediator.Send(command);

        /// <summary>
        /// 获取H5用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetH5UsersList")]
        public async Task<PagedResultDto<H5Users>> GetH5UsersListAsync([FromBody] QueryRequest request) => await applicationUserQueries.GetH5UsersListAsync(request);

        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateApplicationUser")]
        public Task<bool> CreateApplicationUser([FromBody] CreateApplicationUserCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateApplicationUserInfo")]
        public Task<bool> UpdateApplicationUserInfo([FromBody] UpdateApplicationUserCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 获取设备在线状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceOnLineStatus")]
        public async Task<bool> GetDeviceOnLineStatus(long id) => await _deviceBaseQueries.GetDeviceOnLineStatus(id);

        #region 字典

        /// <summary>
        /// 获取字典详细
        /// </summary>
        /// <param name="parentKey">父级key</param>
        /// <returns>字典集合</returns>
        [HttpGet("DictionarySub")]
        public async Task<List<DicionaryUseDto>> GetDictionarySubAsync(string parentKey)
        {
            return await dictionaryQueries.GetDictionarySubAsync(parentKey);
        }
        #endregion

        /// <summary>
        /// 获取饮品总览
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBeverageTotal")]
        public async Task<BeverageTotalDto> GetBeverageTotalAsync()
        {
            return await reportsQuerie.GetBeverageTotal();
        }

        /// <summary>
        /// 获取订单详情总览
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetOrderDetailTotal")]
        public async Task<OrderDetailTotal> GetOrderDetailTotalAsync([FromBody] List<DateTime> dateRange)
        {
            return await reportsQuerie.GetOrderDetailTotal(dateRange);
        }
    }
}
