using Magicodes.ExporterAndImporter.Core.Filters;
using Magicodes.ExporterAndImporter.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using YS.CoffeeMachine.Domain.Shared.Const;
using YSCore.Base.App;

namespace YS.CoffeeMachine.Provider.DocmentFilter.Lnaguage
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
            if (exporterHeaderInfo.DisplayName == "多语言值")
            {
                var _service = AppHelper.RootServices.GetService<IMemoryCache>();
                var langCode = _service.Get<string>(CacheConst.CacheCurrentLanguage);
                exporterHeaderInfo.DisplayName = langCode;
            }
            return exporterHeaderInfo;
        }
    }
}
