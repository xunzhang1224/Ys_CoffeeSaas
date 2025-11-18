using AutoMapper.Configuration.Annotations;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using YS.CoffeeMachine.API.Extensions.IExecl.DocmentFilter.Order;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.API.Extensions.IExecl.Dto.Docment.Exporter
{
    /// <summary>
    /// 售卖记录导出
    /// </summary>
    [ExcelExporter(Name = "售卖记录导出", ExporterHeaderFilter = typeof(ExporterHeaderFilter))]
    public class OrderInfoExport
    {
        /// <summary>
        /// 内部订单号
        /// </summary>
        [ExporterHeader(DisplayName = "OrderNo", IsBold = true)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        /// </summary>
        [ExporterHeader(DisplayName = "ThirdOrderNo", IsBold = true)]
        public string? ThirdOrderNo { get; set; } = null;

        /// <summary>
        /// 设备编号
        /// </summary>
        [ExporterHeader(DisplayName = "DeviceCode", IsBold = true)]
        public string DeviceCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [ExporterHeader(DisplayName = "DeviceName", IsBold = true)]
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        [ExporterHeader(DisplayName = "DeviceModelName", IsBold = true)]
        public string DeviceModelName { get; set; }

        /// <summary>
        /// 饮品名称
        /// </summary>
        [ExporterHeader(DisplayName = "BeverageName", IsBold = true)]
        public string BeverageName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [ExporterHeader(DisplayName = "Count", IsBold = true)]
        public int Count { get; set; } = 1;

        /// <summary>
        /// 消费金额(元)
        /// </summary>
        [ExporterHeader(DisplayName = "Amount", IsBold = true)]
        public decimal Amount { get; set; } = 0;

        /// <summary>
        /// 付款方案
        /// </summary>
        [ExporterHeader(DisplayName = "Provider", IsBold = true)]
        public string Provider { get; set; }

        /// <summary>
        /// 支付结果
        /// </summary>
        [ExporterHeader(DisplayName = "SaleResult", IsBold = true)]
        public OrderSaleResult SaleResult { get; set; }

        /// <summary>
        /// 出货结果
        /// </summary>
        [ExporterHeader(DisplayName = "ShipmentResult", IsBold = true)]
        public OrderShipmentResult ShipmentResult { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        [ExporterHeader(DisplayName = "EnterpriseName", IsBold = true)]
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        [ExporterHeader(DisplayName = "EnterpriseId", IsBold = true)]
        public string EnterpriseId { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        [ExporterHeader(DisplayName = "CreateTime", IsBold = true)]
        public string CreateTime { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        /// </summary>
        [ExporterHeader(DisplayName = "PayTime", IsBold = true)]
        public string PayTime { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        [ExporterHeader(DisplayName = "ReturnAmount", IsBold = true)]
        public decimal ReturnAmount { get; set; } = 0;
    }
}
