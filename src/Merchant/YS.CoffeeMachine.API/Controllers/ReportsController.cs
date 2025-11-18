using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Dtos.ReportsDtos;
using YS.CoffeeMachine.Application.Queries.IReportsQueries;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 报表统计
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class ReportsController(IReportsQuerie _reports) : Controller
    {
        /// <summary>
        /// 设备物料报表(设备信息总览)
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeviceMaterialReport")]
        public async Task<Dictionary<int, DeviceMeterialReportDto>> DeviceMaterialReportAsync([FromBody] ReportsInput input)
        {
            return await _reports.DeviceMaterialReport(input);
        }

        /// <summary>
        /// 制作数趋势
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("ProductionTrendReport")]
        public async Task<ProductionTrendReportDto> ProductionTrendReportAsync([FromBody] ReportsInput input)
        {
            return await _reports.ProductionTrendReport(input);
        }

        /// <summary>
        /// 排名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("RankingReport")]
        public async Task<RankingReportDto> RankingReportAsync([FromBody] ReportsInput input)
        {
            return await _reports.RankingReport(input);
        }

        /// <summary>
        /// 饮品制作量占比
        /// </summary>
        [HttpPost("BeverageProductionReport")]
        public async Task<List<CommonDto>> BeverageProductionReportAsync([FromBody] ReportsInput input)
        {
            return await _reports.BeverageProductionReport(input);
        }

        /// <summary>
        /// 总览
        /// </summary>
        /// <returns></returns>
        [HttpPost("OverviewReport")]
        public async Task<OverviewReportDto> OverviewReportAsync([FromBody] ReportsInput input)
        {
            return await _reports.OverviewReport(input);
        }

        /// <summary>
        /// 设备概览
        /// </summary>
        /// <returns></returns>
        [HttpGet("DeviceOverview")]
        public async Task<DeviceOverview> DeviceOverviewAsync()
        {
            return await _reports.DeviceOverview();
        }

        /// <summary>
        /// 今日制杯及消耗
        /// </summary>
        /// <returns></returns>
        [HttpPost("TodayMeterial")]
        public async Task<List<MeterialValue>> TodayMeterialAsync([FromBody] List<DateTime> dateRange)
        {
            return await _reports.TodayMeterial(dateRange);
        }

        /// <summary>
        /// 今日饮品排行
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        [HttpPost("TodayBeverage")]
        public async Task<List<CommonDto>> TodayBeverageAsync([FromBody] List<DateTime> dateRange)
        {
            return await _reports.TodayBeverage(dateRange);
        }

        /// <summary>
        /// 设备制作数量
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeviceMakeReport")]
        public async Task<DeviceMakeReportDto> GetDeviceMakeReportAsync([FromBody] DeviceMakeReportInput input)
        {
            return await _reports.GetDeviceMakeReport(input);
        }

        /// <summary>
        /// 根据设备id获取饮品制作数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDrinksByDeviceBaseId")]
        public async Task<List<CommonDto>> GetDrinksByDeviceBaseIdAsync(long deviceBaseId)
        {
            return await _reports.GetDrinksByDeviceBaseId(deviceBaseId);
        }

        /// <summary>
        /// 今日设备制作排行top5
        /// </summary>
        [HttpPost("TodayDeviceMakeRanking")]
        public async Task<List<CommonDto>> TodayDeviceMakeRankingAsync([FromBody] List<DateTime> dateRange)
        {
            return await _reports.TodayDeviceMakeRanking(dateRange);
        }

        /// <summary>
        /// 今日设备错误统计top5
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        [HttpPost("TodayDeviceErrorReport")]
        public async Task<List<CommonDto>> TodayDeviceErrorReportAsync([FromBody] List<DateTime> dateRange)
        {
            return await _reports.TodayDeviceErrorReport(dateRange);
        }
    }
}
