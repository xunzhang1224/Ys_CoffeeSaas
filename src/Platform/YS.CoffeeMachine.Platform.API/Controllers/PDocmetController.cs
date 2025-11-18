using FreeRedis;
using Magicodes.ExporterAndImporter.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Channels;
using YS.CoffeeMachine.Application.Dtos.BasicDtos.Language;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands;
using YS.CoffeeMachine.Application.Queries.BasicQueries;
using YS.CoffeeMachine.Application.Queries.BasicQueries.Language;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Platform.API.Extensions.IExecl;
using YS.CoffeeMachine.Provider.Dto.Docment;
using YS.CoffeeMachine.Provider.Dto.Docment.Exporter;
using YS.CoffeeMachine.Provider.IServices;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 文件操作
    /// </summary>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class PDocmetController(ILanguageInfoQueries _languageInfoQueries, IServiceProvider _serviceProvider, ExportService exportService,
        IRedisClient _redisClent,
        IExporter _exporter, IMediator mediator, IFileCenterQuerie _fileCenterQuerie,
        IMemoryCache _cache) : Controller
    {
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="type">文件类型</param>
        /// <param name="file">文件</param>
        /// <param name="dicDataId">字典</param>
        /// <returns></returns>
        [HttpPost("ImporAsync")]
        public async Task ImporAsync([FromQuery] DocmentTypeEnum type, [FromQuery] string dicDataId, IFormFile file)
        {
            var _service = _serviceProvider.GetRequiredKeyedService<IDocmentService>(type.ToString());
            using var excelStream = file.OpenReadStream();
            var infos = await _service.GetExcleDataToList(excelStream, dicDataId);
            await _service.Inserts(infos, dicDataId);
        }

        /// <summary>
        /// 多语言导出
        /// </summary>
        /// <param name="input"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpPost("LanguageExportAsync")]
        public async Task ExportAsync([FromQuery] string langCode,
           [FromServices] Channel<FileExcelUploadTask> channel)
        {
            //var redisLanguageInfoDto = await _redisClent.HGetAsync<LanguageInfoDto>(CacheConst.MultilingualAll, langCode);
            //var langInfo = await _languageInfoQueries.GetLanguageAsync(langCode);
            var data = await _languageInfoQueries.GetLanguageTextsAsync(new LanguageTextInput() { LangCode = langCode });
            var chineseDatas = await _languageInfoQueries.GetLanguageTextsAsync(new LanguageTextInput() { LangCode = "zh-CN" });
            // 缓存当前操作的语种，后续导出Dto文件动态映射有用到
            _cache.Set(CacheConst.CacheCurrentLanguage, langCode, TimeSpan.FromMinutes(10));
            var langInfos = new List<LanguageExporter>();
            foreach (var item in chineseDatas)
            {
                var value = data.FirstOrDefault(x => x.Key == item.Key).Value;
                langInfos.Add(new LanguageExporter() { LanguageFieldIdentification = item.Key, LanguageChineseReference = chineseDatas[item.Key], LanguageValue = value });
            }
            await exportService.ExportAsByteArray(langInfos, langCode, DocmentTypeEnum.Language, channel);
        }

        /// <summary>
        /// 多语言导出
        /// </summary>
        /// <param name="input"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpPost("Export1Async")]
        public async Task Export1Async([FromQuery] string langCode,
           [FromServices] Channel<FileExcelUploadTask> channel)
        {
            await exportService.ExportAsByteArray(new List<LanguageExporter>(), langCode, DocmentTypeEnum.Language, channel);
        }

        /// <summary>
        /// 获取文件中心数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetFileCenter")]
        public async Task<PagedResultDto<FileCenter>> GetFileCenterAsync([FromBody] FileCenterInput input)
        {
            return await _fileCenterQuerie.GetFileCenter(input);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteFileById")]
        public Task<bool> DeleteFileById(DeleteFileCenterCommand command) => mediator.Send(command);
    }
}
