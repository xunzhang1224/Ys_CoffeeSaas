using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.BasicDtos;

namespace YS.CoffeeMachine.Application.Dtos.ReportsDtos
{
    /// <summary>
    /// 报表输入参数
    /// </summary>
    public class ReportsInput : TimezoneOffsetBaseDto
    {
        /// <summary>
        /// 日期范围
        /// </summary>
        public List<DateTime> DateRange { get; set; }

        /// <summary>
        /// 日期范围（同比）
        /// </summary>
        public List<DateTime>? DateRangeT { get; set; }

        /// <summary>
        /// 日期范围（环比）
        /// </summary>
        public List<DateTime>? DateRangeH { get; set; }

        /// <summary>
        /// 企业信息Id
        /// </summary>
        public long EnterpriseInfoId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? DeviceId { get; set; }

        /// <summary>
        /// 分组ids
        /// </summary>
        public List<long>? GroupIds { get; set; }

        /// <summary>
        /// 时间粒度 0：小时  1：天 2:月
        /// </summary>
        public int? TimeType { get; set; }

        /// <summary>
        /// 设备模型Id
        /// </summary>
        public long? DeviceModelId { get; set; }
    }

    /// <summary>
    /// 设备Make报表输入参数
    /// </summary>
    public class DeviceMakeReportInput
    {
        /// <summary>
        /// 日期范围(当天)
        /// </summary>
        public List<DateTime> DayDateRange { get; set; }

        /// <summary>
        /// 日期范围（本周）
        /// </summary>
        public List<DateTime> WeekDateRange { get; set; }

        /// <summary>
        /// 日期范围（本月）
        /// </summary>
        public List<DateTime> MonthDateRange { get; set; }

        /// <summary>
        /// 日期范围（昨日）
        /// </summary>
        public List<DateTime> YesterdayDateRange { get; set; }

        /// <summary>
        /// 日期范围（上周）
        /// </summary>
        public List<DateTime> LastWeekDateRange { get; set; }

        /// <summary>
        /// 日期范围（上月）
        /// </summary>
        public List<DateTime> LastMonthDateRange { get; set; }
    }
}
