using Magicodes.ExporterAndImporter.Core.Filters;
using Magicodes.ExporterAndImporter.Core.Models;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Extensions.IExecl.DocmentFilter.DeviceErrorLog
{
    /// <summary>
    /// 导出标题过滤器
    /// </summary>
    public class ExporterHeaderFilter : IExporterHeaderFilter
    {
        /// <summary>
        /// 导出标题过滤器
        /// </summary>
        public ExporterHeaderInfo Filter(ExporterHeaderInfo exporterHeaderInfo)
        {
            exporterHeaderInfo.DisplayName = L.Text[exporterHeaderInfo.DisplayName];
            return exporterHeaderInfo;
        }
    }
}
