using Magicodes.ExporterAndImporter.Core.Filters;
using Magicodes.ExporterAndImporter.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using YS.CoffeeMachine.Domain.Shared.Const;
using YSCore.Base.App;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Provider.DocmentFilter.DeviceOnlineLog
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
