using Magicodes.ExporterAndImporter.Core.Filters;
using Magicodes.ExporterAndImporter.Core.Models;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Provider.DocmentFilter.Lnaguage
{
    /// <summary>
    /// 导入结果过滤器
    /// </summary>
    public class ImportResultFilter : IImportResultFilter
    {
        /// <summary>
        /// 修改数据错误验证结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="importResult"></param>
        /// <returns></returns>
        public ImportResult<T> Filter<T>(ImportResult<T> importResult) where T : class, new()
        {
            var items = importResult.RowErrors.ToList();

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < items[i].FieldErrors.Keys.Count; j++)
                {
                    var key = items[i].FieldErrors.Keys.ElementAt(j);
                    var value = items[i].FieldErrors[key];
                    items[i].FieldErrors[key] = value?.Replace("存在数据重复，请检查！所在行：", L.Text[nameof(ErrorCodeEnum.D0074)]);
                }
            }
            return importResult;
        }
    }
}
