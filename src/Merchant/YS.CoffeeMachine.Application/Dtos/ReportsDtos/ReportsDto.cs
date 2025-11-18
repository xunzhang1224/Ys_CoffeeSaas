using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.ReportsDtos
{
    /// <summary>
    /// 报表输出参数
    /// </summary>
    public class ReportsDto
    {
    }

    /// <summary>
    /// 设备材料
    /// </summary>
    public class MeterialValue
    {
        /// <summary>
        /// 类型
        /// </summary>
        public MaterialTypeEnum Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int TotalValue { get; set; } = 0;
    }

    /// <summary>
    /// 设备材料报表
    /// </summary>
    public class DeviceMeterialReportDto
    {
        /// <summary>
        /// 环比值
        /// </summary>
        public int MomValue { get; set; } = 0;

        /// <summary>
        /// 同比值
        /// </summary>
        public int YoyValue { get; set; } = 0;

        /// <summary>
        /// 总值
        /// </summary>
        public int TotalValue { get; set; } = 0;
    }

    /// <summary>
    /// 杯子
    /// </summary>
    public class CupDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string DateStr { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    /// 物料
    /// </summary>
    public class MeterialDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string DateStr { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public MaterialTypeEnum Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    /// 制作数趋势
    /// </summary>
    public class ProductionTrendReportDto
    {
        /// <summary>
        /// 杯数报表
        /// </summary>
        public List<CupDto> CupEcharts { get; set; }

        /// <summary>
        /// 物料用量报表
        /// </summary>
        public Dictionary<string, Dictionary<int, int>> MaterialEcharts { get; set; }
    }

    /// <summary>
    /// 通用dto
    /// </summary>
    public class CommonDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    /// 排行榜dto
    /// </summary>
    public class RankingReportDto
    {
        /// <summary>
        /// 设备制作量前10
        /// </summary>
        public List<CommonDto> DeviceTop10 { get; set; }

        /// <summary>
        /// 饮品制作量前10
        /// </summary>
        public List<CommonDto> BeveragesTop10 { get; set; }
    }

    /// <summary>
    /// 概览dto
    /// </summary>
    public class OverviewReportDto
    {
        /// <summary>
        /// 设备数
        /// </summary>
        public int DeviceCount { get; set; }

        /// <summary>
        /// 设备数(年)
        /// </summary>
        public List<CommonDto> DeviceCountByYear { get; set; }
    }

    /// <summary>
    /// 设备概览dto
    /// </summary>
    public class DeviceOverview
    {
        /// <summary>
        /// 设备在线数
        /// </summary>
        public int OnlineCount { get; set; }
    }

    /// <summary>
    /// 临时类
    /// </summary>
    public class Temp
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Day { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public MaterialTypeEnum Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    /// 设备制作报表
    /// </summary>
    public class DeviceMakeReportDto
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 天数
        /// </summary>
        public int DayCount { get; set; } = 0;

        /// <summary>
        /// 昨天数
        /// </summary>
        public int YesterdayCount { get; set; } = 0;

        /// <summary>
        /// 周数
        /// </summary>
        public int WeekCount { get; set; } = 0;

        /// <summary>
        /// 上周数
        /// </summary>
        public int LastWeekCount { get; set; } = 0;

        /// <summary>
        /// 月数
        /// </summary>
        public int MonthCount { get; set; } = 0;

        /// <summary>
        /// 上月数
        /// </summary>
        public int LastMonthCount { get; set; } = 0;
    }

    /// <summary>
    /// 饮品模块总数
    /// </summary>
    public class BeverageTotalDto
    {
        /// <summary>
        /// 饮品库存总数
        /// </summary>
        public int BeverageInventoryCount { get; set; } = 0;

        /// <summary>
        /// 饮品收集总数
        /// </summary>
        public int BeverageCollectionCount { get; set; } = 0;
    }

    /// <summary>
    /// 订单详情总数
    /// </summary>
    public class OrderDetailTotal
    {
        /// <summary>
        /// 支付成功
        /// </summary>
        public int PaySuccess { get; set; } = 0;

        /// <summary>
        /// 支付失败
        /// </summary>
        public int PayFail { get; set; } = 0;

        /// <summary>
        /// 退款
        /// </summary>
        public int Refund { get; set; } = 0;

        /// <summary>
        /// 部分退款
        /// </summary>
        public int PartialRefund { get; set; } = 0;

        /// <summary>
        /// 退款中
        /// </summary>
        public int Refunding { get; set; } = 0;

        /// <summary>
        /// 未支付
        /// </summary>
        public int NotPay { get; set; } = 0;

        /// <summary>
        /// 支付超时
        /// </summary>
        public int Timeout { get; set; } = 0;

        /// <summary>
        /// 取消支付
        /// </summary>
        public int Cancel { get; set; } = 0;

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; } = 0;
    }
}
