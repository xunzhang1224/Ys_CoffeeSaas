using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using YS.CoffeeMachine.API.Extensions.IExecl.DocmentFilter.DeviceErrorLog;

namespace YS.CoffeeMachine.API.Extensions.IExecl.Dto.Docment.Exporter
{
    /// <summary>
    /// 设备异常记录导出
    /// </summary>
    [ExcelExporter(Name = "设备异常记录导出", ExporterHeaderFilter = typeof(ExporterHeaderFilter))]
    public class DeviceErrorLogExport
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [ExporterHeader(DisplayName = "Sn", IsBold = true)]
        public string Sn { get; set; }

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
        /// 异常码
        /// </summary>
        [ExporterHeader(DisplayName = "AbnormalCode", IsBold = true)]
        public string AbnormalCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [ExporterHeader(DisplayName = "Status", IsBold = true)]
        public string Status { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        [ExporterHeader(DisplayName = "EnterpriseName", IsBold = true)]
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [ExporterHeader(DisplayName = "CreateTime", IsBold = true)]
        public string CreateTime { get; set; }
    }
}
