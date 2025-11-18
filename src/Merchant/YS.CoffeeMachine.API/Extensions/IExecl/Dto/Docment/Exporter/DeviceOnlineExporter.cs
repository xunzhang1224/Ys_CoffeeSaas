using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using YS.CoffeeMachine.Provider.DocmentFilter.DeviceOnlineLog;

namespace YS.CoffeeMachine.API.Extensions.IExecl.Dto.Docment.Exporter
{
    /// <summary>
    /// 设备上下线导出
    /// </summary>
    [ExcelExporter(Name = "设备上下线导出", ExporterHeaderFilter = typeof(ExporterHeaderFilter))]
    public class DeviceOnlineExporter
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [ExporterHeader(DisplayName = "MachineStickerCode", IsBold = true)]
        public string MachineStickerCode { get; set; }

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
        /// 上/下线
        /// </summary>
        [ExporterHeader(DisplayName = "IsOnline", IsBold = true)]
        public string IsOnline { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        [ExporterHeader(DisplayName = "记录时间", IsBold = true)]
        public string DateTime { get; set; }
    }
}
