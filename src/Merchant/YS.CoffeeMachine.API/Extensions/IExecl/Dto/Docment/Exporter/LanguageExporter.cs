using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using YS.CoffeeMachine.Provider.DocmentFilter.Lnaguage;

namespace YS.CoffeeMachine.Provider.Dto.Docment.Exporter
{
    /// <summary>
    /// 多语言导出
    /// </summary>
    [ExcelExporter(Name = "多语言导出", ExporterHeaderFilter = typeof(ExporterHeaderFilter))]
    public class LanguageExporter
    {
        /// <summary>
        /// 字段标识
        /// </summary>
        [ExporterHeader(DisplayName = "字段标识", IsBold = true)]
        public string LanguageFieldIdentification { get; set; }

        /// <summary>
        /// 中文参照
        /// </summary>
        [ExporterHeader(DisplayName = "中文参照", IsBold = true)]
        public string LanguageChineseReference { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [ExporterHeader(DisplayName = "多语言值", IsBold = true)]
        public string LanguageValue { get; set; }
    }
}
