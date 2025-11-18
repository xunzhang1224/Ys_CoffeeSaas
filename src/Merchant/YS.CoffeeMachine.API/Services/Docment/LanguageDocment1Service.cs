using AutoMapper;
using Magicodes.ExporterAndImporter.Excel;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using YS.CoffeeMachine.Application.Commands.BasicCommands.LanguageCommands;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Provider.Dto.Docment.Importer;
using YS.CoffeeMachine.Provider.IServices;

namespace YS.CoffeeMachine.Platform.API.Application.Services.Basic.Docment
{
    /// <summary>
    /// 语言导入
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="_mapper"></param>
    /// <param name="_cache"></param>
    public class LanguageDocment1Service(IMediator mediator, IMapper _mapper, IMemoryCache _cache) : ExcelImporter, IDocmentService
    {
        /// <summary>
        /// 语言导入
        /// </summary>
        public async Task Inserts(object dtos, string key = null)
        {
            var dto = _mapper.Map<List<LanguageImporter>>(dtos);
            var dic = new Dictionary<string, string>();
            foreach (var item in dto)
            {
                dic.Add(item.LanguageFieldIdentification, item.LanguageValue);
            }
            await mediator.Send(new ImportSysLanguageTextCommand(key, dic));
        }

        /// <summary>
        /// GetExcleDataToList
        /// </summary>
        /// <param name="file"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<object> GetExcleDataToList(Stream file, string key = null)
        {
            _cache.Set(CacheConst.CacheCurrentLanguage, key);
            //假设你有一个Dto类用于映射Excel数据
            var info = await base.Import<LanguageImporter>(file);
            return info.Data.ToList();
        }
    }
}
