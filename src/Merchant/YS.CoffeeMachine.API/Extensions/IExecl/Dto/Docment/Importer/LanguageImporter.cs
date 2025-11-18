using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System.ComponentModel.DataAnnotations;
using YS.CoffeeMachine.Provider.DocmentFilter.Lnaguage;

namespace YS.CoffeeMachine.Provider.Dto.Docment.Importer
{
    /// <summary>
    /// 多语言导入
    /// </summary>
    [ExcelImporter(ImportResultFilter = typeof(ImportResultFilter), ImportHeaderFilter = typeof(ImportHeaderFilter), IsLabelingError = true)]
    public class LanguageImporter
    {
        /// <summary>
        /// 字段标识
        /// </summary>
        [ImporterHeader(Name = "字段标识", IsAllowRepeat = false)]
        [Required]
        public string LanguageFieldIdentification { get; set; }

        /// <summary>
        /// 中文参照
        /// </summary>
        [ImporterHeader(Name = "中文参照")]
        public string LanguageChineseReference { get; set; }

        /// <summary>
        /// 多语言值
        /// </summary>
        [ImporterHeader(Name = "多语言值")]
        public string LanguageValue { get; set; }
    }
}
