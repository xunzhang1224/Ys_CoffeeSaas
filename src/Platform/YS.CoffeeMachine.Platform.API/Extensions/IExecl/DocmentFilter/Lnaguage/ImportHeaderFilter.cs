using Magicodes.ExporterAndImporter.Core.Filters;
using Magicodes.ExporterAndImporter.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using YS.CoffeeMachine.Domain.Shared.Const;
using YSCore.Base.App;

namespace YS.CoffeeMachine.Provider.DocmentFilter.Lnaguage
{
    /// <summary>
    /// 导入头部过滤
    /// </summary>
    public class ImportHeaderFilter : IImportHeaderFilter
    {
        /// <summary>
        /// 导入头部过滤
        /// </summary>
        public List<ImporterHeaderInfo> Filter(List<ImporterHeaderInfo> importerHeaderInfos)
        {
            var _service = AppHelper.RootServices.GetService<IMemoryCache>();
            var langCode = _service.Get<string>(CacheConst.CacheCurrentLanguage);
            foreach (var item in importerHeaderInfos)
            {
                if (item.Header.Name == "多语言值")
                {
                    item.Header.Name = langCode;
                }
            }
            return importerHeaderInfos;
        }
    }
}
