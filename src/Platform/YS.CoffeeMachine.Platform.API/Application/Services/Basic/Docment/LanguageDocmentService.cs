using AutoMapper;
using Magicodes.ExporterAndImporter.Excel;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.LanguageCommands;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Provider.Dto.Docment.Importer;
using YS.CoffeeMachine.Provider.IServices;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.Services.Basic.Docment
{
    /// <summary>
    /// 多语言导入文本服务
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="_mapper"></param>
    /// <param name="_cache"></param>
    public class LanguageDocmentService(IMediator mediator, IMapper _mapper, IMemoryCache _cache) : ExcelImporter, IDocmentService
    {
        /// <summary>
        /// 多语言导入文本服务
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
        /// 多语言导入文本服务
        /// </summary>
        public async Task<object> GetExcleDataToList(Stream file, string key = null)
        {
            _cache.Set(CacheConst.CacheCurrentLanguage, key);
            //假设你有一个Dto类用于映射Excel数据
            var info = await base.Import<LanguageImporter>(file);
            if (info == null || info.Data.Count == null || info.Data.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0007)]);
            if (!info.Data.Any(x => x.LanguageValue != null))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0006)]);
            return info.Data.ToList();
        }
    }
}
