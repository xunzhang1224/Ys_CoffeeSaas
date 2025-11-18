using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Application.Dtos.BasicDtos.Language;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.DictionaryCommand;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.LanguageCommands;
using YS.CoffeeMachine.Application.Queries.BasicQueries.Language;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Dictionary;
using YSCore.Base.Localization;
using YS.CoffeeMachine.API.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using YS.CoffeeMachine.Application.PlatformDto.BasicDtos;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.Provider.OSS.Interface.Base;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 平台通用设置
    /// </summary>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class PlatformSettingController(IMediator mediator, IOSSService oSSService, ILanguageInfoQueries languageInfoQueries, IDictionaryQueries dictionaryQueries) : Controller
    {
        #region 多语言
        /// <summary>
        /// 测试多语言
        /// </summary>
        /// <returns></returns>
        [HttpGet("LanguageTest")]
        public async Task<string> LanguageTest(string textCode)
        {
            //throw ExceptionHelper.AppFriendly(L.Text["LanguageValue"]);
            return L.Text[textCode];
        }

        /// <summary>
        /// 添加语言
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("LanguageInfo")]
        public async Task<bool> LanguageInfoAsync([FromBody] CreateLanguageInfoCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取所有语种类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("LanguageInfo")]
        [DisplayName("获取所有语种类型")]
        public async Task<List<LanguageInfoDto>> LanguageInfoAsync()
        {
            return await languageInfoQueries.GetAllLanguageAsync();
        }

        /// <summary>
        /// 获取语言分页列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("LanguageInfoPage")]
        public async Task<PagedResultDto<LanguageInfoDto>> LanguageInfoPageAsync([FromBody] QueryRequest request)
        {
            return await languageInfoQueries.GetAllLanguagePageAsync(request);
        }

        /// <summary>
        /// 编辑语言信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("LanguageInfo")]
        public async Task<bool> LanguageInfoAsync([FromBody] UpdateLanguageInfoCommand command) => await mediator.Send(command);

        /// <summary>
        /// 删除语言
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("LanguageInfo")]
        public async Task<bool> LanguageInfoAsync([FromBody] DeleteLanguageInfoCommand command) => await mediator.Send(command);

        /// <summary>
        /// 添加语言文本
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("LanguageText")]
        public async Task LanguageText([FromBody] CreateLanguageTextCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取语言文本分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("LanguageTextPage")]
        public async Task<PagedResultDto<LanguageTextDto>> LanguageTextPageAsync([FromBody] LanguageTextQuery request)
        {
            return await languageInfoQueries.GetLanguageTextPageAsync(request);
        }

        /// <summary>
        /// 根据语言文本编码获取语言文本详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("LanguageTextDetail")]
        public async Task<GetLanguageTextDetailDto> LanguageTextDetailAsync(string code)
        {
            return await languageInfoQueries.GetDetailAsync(code);
        }

        /// <summary>
        /// 删除语言文本
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("LanguageText")]
        public async Task LanguageTextAsync([FromBody] DeleteLanguageTextCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取语言文本列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("LanguageTexts")]
        public async Task<Dictionary<string, string>> GetLanguageTextsAsync(LanguageTextInput input)
        {
            return await languageInfoQueries.GetLanguageTextsAsync(input);
        }

        #endregion

        #region 字典

        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("Dictionary")]
        public async Task Dictionary([FromBody] AddDictionaryCommand command) => await mediator.Send(command);

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("Dictionary")]
        public async Task Dictionary([FromBody] UpdateDictionaryCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取字典主表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("DictionaryList")]
        public async Task<PagedResultDto<DictionaryDto>> GetDictionaryByKey([FromBody] DictionaryInput req)
        {
            return await dictionaryQueries.GetDictionaryByKey(req);
        }

        /// <summary>
        /// 获取字典详细
        /// </summary>
        /// <param name="parentKey">父级key</param>
        /// <returns>字典集合</returns>
        [HttpGet("DictionarySub")]
        public async Task<List<DicionaryUseDto>> GetDictionarySubAsync(string parentKey)
        {
            return await dictionaryQueries.GetDictionarySubAsync(parentKey);
        }

        /// <summary>
        /// 根据父级Key获取字典列表，包含禁用
        /// </summary>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        [HttpGet("GetDictionarySubContainDisable")]
        public async Task<List<DicionaryUseDto>> GetDictionarySubContainDisableAsync(string parentKey) => await dictionaryQueries.GetDictionarySubContainDisableAsync(parentKey);

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("Dictionary")]
        public async Task DeleteDictionary(DeleteDictionaryCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取语言字典列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLanguageDic")]
        public async Task<List<DicionaryUseDto>> GetLanguageDicList()
        {
            return await dictionaryQueries.GetLanguageDicList();
        }

        /// <summary>
        /// 获取地区关联所需的字典list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAreaRelationNeedDictionary")]
        public async Task<List<DictionaryDto>> GetAreaRelationNeedDictionaryAsync()
        {
            return await dictionaryQueries.GetAreaRelationNeedDictionaryAsync();
        }

        /// <summary>
        /// 根据父级key的数组返回对应
        /// </summary>
        /// <param name="parentKeys">父级keys</param>
        /// <returns></returns>
        [HttpPost("GetDictionaryByArry")]
        public async Task<List<DictionaryDto>> GetDictionaryByArryAsync(List<string> parentKeys)
        {
            return await dictionaryQueries.GetDictionaryByArry(parentKeys);
        }
        #endregion

        #region 文件预上传地址生成
        /// <summary>
        /// 获取预上传文件地址
        /// </summary>
        /// <param name="curFileName"></param>
        /// <returns></returns>
        [HttpPost("GetPreUploadUrl")]
        public async Task<string> GetPreUploadUrl(string curFileName)
        {
            return await GetMinioPreviewFileUrl(curFileName);
        }
        /// <summary>
        /// 获取上传临时文件地址
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private async Task<string> GetMinioPreviewFileUrl(string fileName)
        {
            var expiryTime = 60 * 60;
            var newFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(fileName);
            return await oSSService.PresignedPutObjectAsync(OssDescribeConst.DefaultBucketName, "coffeetemp/" + newFileName, expiryTime);
        }
        #endregion
    }
}
