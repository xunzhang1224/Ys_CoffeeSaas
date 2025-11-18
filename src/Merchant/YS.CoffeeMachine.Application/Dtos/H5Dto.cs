using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Dtos
{
    /// <summary>
    /// H5设备统计
    /// </summary>
    public class DeviceCountOutput
    {
        /// <summary>
        /// 设备总数
        /// </summary>
        public int DeviceCount { get; set; }

        /// <summary>
        /// 设备在线数
        /// </summary>
        public int DeviceOnlineCount { get; set; }

        /// <summary>
        /// 缺料设备数
        /// </summary>
        public int DeviceNotStockCount { get; set; }

        /// <summary>
        /// 未激活设备数
        /// </summary>
        public int DeviceActionCount { get; set; }
    }

    /// <summary>
    /// 设备销售排行
    /// </summary>
    public class DeviceSaleRank
    {
        /// <summary>
        /// 设备
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 销售额
        /// </summary>
        public decimal SaleCount { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public string Hb { get; set; }
    }

    /// <summary>
    /// 商品销售排行
    /// </summary>
    public class GoodsSaleRank
    {
        /// <summary>
        /// 图片
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string GoodName { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// H5设备统计
    /// </summary>
    public class DeviceReportCountOutput
    {
        /// <summary>
        /// 设备总数
        /// </summary>
        public int DeviceCount { get; set; }

        /// <summary>
        /// 设备在线数
        /// </summary>
        public int DeviceOnlineCount { get; set; }

        /// <summary>
        /// 设备在线占比
        /// </summary>
        public decimal DeviceOnlineProportion
        {
            get
            {
                if (DeviceCount == 0) return 0;
                return Math.Round((decimal)DeviceOnlineCount / DeviceCount * 100, 2);
            }
        }

        /// <summary>
        /// 设备离线占比
        /// </summary>
        public decimal DeviceNotOnlineProportion
        {
            get
            {
                if (DeviceCount == 0) return 0;
                return Math.Round(100 - DeviceOnlineProportion, 2);
            }
        }
    }

    /// <summary>
    /// H5首页统计
    /// </summary>
    public class SyCountOutput
    {
        /// <summary>
        /// 设备总数
        /// </summary>
        public int DeviceCount { get; set; }

        /// <summary>
        /// 订单数
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 今日交易额
        /// </summary>
        public decimal TransactionAmount { get; set; } = 0;

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundOrderCount { get; set; } = 0;
    }

    /// <summary>
    /// 每周经营收入
    /// </summary>
    public class OperatingRevenueOutput
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime time { get; set; }

        /// <summary>
        /// 收入
        /// </summary>
        public decimal Income { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal Refund { get; set; }
    }

    /// <summary>
    /// 今日盈利分析
    /// </summary>
    public class HourlyRevenueStats
    {
        /// <summary>
        /// 时间段
        /// </summary>
        public string TimeSlot { get; set; }
        /// <summary>
        /// 订单量
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 经营收入
        /// </summary>
        public decimal TotalRevenue { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public int StartHour { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public int EndHour { get; set; }
    }
}
