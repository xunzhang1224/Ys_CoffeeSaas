using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.ReportsDtos;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Queries.IReportsQueries
{
    /// <summary>
    /// 报表查询接口
    /// </summary>
    public interface IReportsQuerie
    {
        /// <summary>
        /// 设备物料报表(设备信息总览)
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<int, DeviceMeterialReportDto>> DeviceMaterialReport(ReportsInput input);

        /// <summary>
        /// 制作数趋势
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ProductionTrendReportDto> ProductionTrendReport(ReportsInput input);

        /// <summary>
        /// 排名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RankingReportDto> RankingReport(ReportsInput input);

        /// <summary>
        /// 饮品制作量占比
        /// </summary>
        Task<List<CommonDto>> BeverageProductionReport(ReportsInput input);

        /// <summary>
        /// 总览
        /// </summary>
        /// <returns></returns>
        Task<OverviewReportDto> OverviewReport(ReportsInput input);

        /// <summary>
        /// 设备概览
        /// </summary>
        /// <returns></returns>
        Task<DeviceOverview> DeviceOverview();

        /// <summary>
        /// 今日制杯及消耗
        /// </summary>
        /// <returns></returns>
        Task<List<MeterialValue>> TodayMeterial(List<DateTime> dateRange);

        /// <summary>
        /// 今日饮品排行
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        Task<List<CommonDto>> TodayBeverage(List<DateTime> dateRange);

        /// <summary>
        /// 设备制作数量
        /// </summary>
        /// <returns></returns>
        Task<DeviceMakeReportDto> GetDeviceMakeReport(DeviceMakeReportInput input);

        /// <summary>
        /// 根据设备id获取饮品制作数量
        /// </summary>
        /// <returns></returns>
        Task<List<CommonDto>> GetDrinksByDeviceBaseId(long deviceBaseId);

        /// <summary>
        /// 今日设备制作排行top5
        /// </summary>
        Task<List<CommonDto>> TodayDeviceMakeRanking(List<DateTime> dateRange);

        /// <summary>
        /// 今日设备错误统计top5
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        Task<List<CommonDto>> TodayDeviceErrorReport(List<DateTime> dateRange);

        /// <summary>
        /// 获取饮品总览
        /// </summary>
        /// <returns></returns>
        Task<BeverageTotalDto> GetBeverageTotal();

        /// <summary>
        /// 获取订单详情总览
        /// </summary>
        /// <returns></returns>
        Task<OrderDetailTotal> GetOrderDetailTotal(List<DateTime> dateRange);
    }
}
