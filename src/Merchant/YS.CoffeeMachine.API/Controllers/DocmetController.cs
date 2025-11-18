using Aop.Api.Domain;
using Autofac.Core;
using AutoMapper;
using FreeRedis;
using Magicodes.ExporterAndImporter.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Channels;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.API.Extensions.IExecl;
using YS.CoffeeMachine.API.Extensions.IExecl.Dto.Docment.Exporter;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.BasicCommands.DocmentCommands;
using YS.CoffeeMachine.Application.Dtos.BasicDtos.Language;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Application.Dtos.OrderDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.BasicQueries.Language;
using YS.CoffeeMachine.Application.Queries.FileResourceQueries;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Application.Queries.IOrderQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Provider.Dto.Docment;
using YS.CoffeeMachine.Provider.Dto.Docment.Exporter;
using YS.CoffeeMachine.Provider.IServices;

namespace QualityApp.Host.Controllers
{
    /// <summary>
    /// 文件操作
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class DocmetController(ILanguageInfoQueries _languageInfoQueries, IServiceProvider _serviceProvider, ExportService exportService, IDeviceBaseQueries _deviceBaseQueries,
        IRedisClient _redisClent,
        IExporter _exporter, IMapper mapper, IOrderInfoQueries orderInfoQueries, CommonHelper _commonHelper,
        IMediator mediator, IFileCenterQuerie _fileCenterQuerie,UserHttpContext _user,
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
        /// 导入
        /// </summary>
        /// <param name="type">文件类型</param>
        /// <param name="file">文件</param>
        /// <param name="dicDataId">字典</param>
        /// <returns></returns>
        [HttpGet("Impor1Async")]
        public async Task Impor1Async(DocmentTypeEnum type)
        {
            var _service = _serviceProvider.GetRequiredKeyedService<IDocmentService>(type.ToString());
        }

        /// <summary>
        /// 多语言导出
        /// </summary>
        /// <param name="input"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpPost("LanguageExportAsync")]
        public async Task LanguageExportAsync([FromQuery] string langCode,
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
            await exportService.ExportAsByteArray(langInfos, DocmentTypeEnum.Language, channel);
        }

        /// <summary>
        /// 上下线导出
        /// </summary>
        /// <param name="input"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpPost("DeviceOnlineLogExportAsync")]
        public async Task DeviceOnlineLogExportAsync([FromBody] DeviceOnlineLogInput input,
           [FromServices] Channel<FileExcelUploadTask> channel)
        {
            var offset = await _commonHelper.GetTimeZoneOffset(_user.TenantId);
            var datas = await _deviceBaseQueries.GetDeviceOnlineLog(input);
            var dtos = mapper.Map<List<DeviceOnlineExporter>>(datas.Items, x =>
            {
                x.Items["TimeZoneOffset"] = offset;
            });
            await exportService.ExportAsByteArray(dtos, DocmentTypeEnum.DeviceOnline, channel);
        }

        /// <summary>
        /// 设备事件记录导出
        /// </summary>
        /// <param name="input"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpPost("DeviceEventLogExportAsync")]
        public async Task DeviceEventLogExportAsync([FromBody] DeviceEventLogInput input,
           [FromServices] Channel<FileExcelUploadTask> channel)
        {
            var offset = await _commonHelper.GetTimeZoneOffset(_user.TenantId);
            var datas = await _deviceBaseQueries.GetDeviceEventLog(input);

            var dtos = mapper.Map<List<DeviceEventLogExport>>(datas.Items, x =>
            {
                x.Items["TimeZoneOffset"] = offset;
            });
            await exportService.ExportAsByteArray(dtos, DocmentTypeEnum.DeviceEvent, channel);
        }

        /// <summary>
        /// 设备异常记录导出
        /// </summary>
        /// <param name="input"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpPost("DeviceErrorLogExportAsync")]
        public async Task DeviceErrorLogExportAsync([FromBody] DeviceErrorLogInput input,
           [FromServices] Channel<FileExcelUploadTask> channel)
        {
            var offset = await _commonHelper.GetTimeZoneOffset(_user.TenantId);
            var datas = await _deviceBaseQueries.GetDeviceErrorLog(input);
            var dtos = mapper.Map<List<DeviceErrorLogExport>>(datas.Items, x =>
            {
                x.Items["TimeZoneOffset"] = offset;
            });
            await exportService.ExportAsByteArray(dtos, DocmentTypeEnum.DeviceError, channel);
        }

        /// <summary>
        /// 订单记录导出
        /// </summary>
        /// <param name="input"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpPost("OrderInfosExportAsync")]
        public async Task OrderInfosExportAsync([FromBody] OrderInfoInput input,
           [FromServices] Channel<FileExcelUploadTask> channel)
        {
            var offset = await _commonHelper.GetTimeZoneOffset(_user.TenantId);
            var datas = await orderInfoQueries.GetOrderInfosPageAsync(input);
            var dtos = mapper.Map<List<OrderInfoExport>>(datas.Items, x =>
            {
                x.Items["TimeZoneOffset"] = offset;
            });
            await exportService.ExportAsByteArray(dtos, DocmentTypeEnum.OrderLog, channel);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteFileById")]
        public Task<bool> DeleteFileById(DeleteFileCenterCommand command) => mediator.Send(command);

        /// <summary>
        /// 获取文件中心数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetFileCenter")]
        public async Task<PagedResultDto<FileCenter>> GetFileCenterAsync([FromBody] FileCenterInput input)
        {
            return await _fileCenterQuerie.GetFileCenter(input);
        }
    }
}
