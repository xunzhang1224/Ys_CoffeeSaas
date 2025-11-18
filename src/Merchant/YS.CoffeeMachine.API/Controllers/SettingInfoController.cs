using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.AdvertisementsCommands.AdvertisementsInfoCommands;
using YS.CoffeeMachine.Application.Commands.SettingsCommands;
using YS.CoffeeMachine.Application.Commands.SettingsCommands.InterfaceStylesCommands;
using YS.CoffeeMachine.Application.Commands.SettingsCommands.SettingInfoCommands;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.LogDtos;
using YS.CoffeeMachine.Application.Dtos.Notity;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.Queries.BasicQueries.Language;
using YS.CoffeeMachine.Application.Queries.IAdvertisementQueries;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Application.Queries.ISettingQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Localization;
using YS.Provider.OSS.Interface.Base;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 设置管理
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="oSSService"></param>
    /// <param name="settingInfoQueries"></param>
    /// <param name="timeZoneInfoQueries"></param>
    /// <param name="interfaceStyleQueries"></param>
    /// <param name="languageInfoQueries"></param>
    /// <param name="advertisementInfoQueries"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class SettingInfoController(IMediator mediator, IOSSService oSSService, ISettingInfoQueries settingInfoQueries,
        ITimeZoneInfoQueries timeZoneInfoQueries, IInterfaceStyleQueries interfaceStyleQueries, ILanguageInfoQueries languageInfoQueries,
        IAdvertisementInfoQueries advertisementInfoQueries, IDictionaryQueries dictionaryQueries, INoticeCfgQueries noticeCfgQueries) : Controller
    {
        /// <summary>
        /// 查询通知
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>字典集合</returns>
        [HttpGet("LanguageText")]
        public async Task<string> LanguageText(int type)
        {
            return L.Text[nameof(ErrorCodeEnum.D0001)];
        }

        /// <summary>
        /// 查询语种
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("LanguageCodeList")]
        public async Task<List<LanguageInfo>> LanguageCodeList()
        {
            return await languageInfoQueries.GetAllLanguageAsync();
        }

        #region 设置信息管理
        /// <summary>
        /// 添加设置信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateSettingInfoAsync")]
        public Task<bool> CreateSettingInfoAsync([FromBody] CreateSettingInfoCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 根据设备Id获取设置信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPost("GetSettingInfoDtoByDeviceIdAsync")]
        public Task<SettingInfoDto> GetSettingInfoDtoByDeviceIdAsync(long deviceId) => settingInfoQueries.GetSettingInfoDtoByDeviceIdAsync(deviceId);
        /// <summary>
        /// 更新货币代码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateCurrencyCode")]
        public Task<bool> UpdateCurrencyCode([FromBody] UpdateCurrencyCodeCommand command) => mediator.Send(command);
        /// <summary>
        /// 编辑设置信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateSettingInfoAsync")]
        public Task<bool> UpdateSettingInfoAsync([FromBody] UpdateSettingInfoCommand command) => mediator.Send(command);
        #endregion

        #region 通用配置管理
        /// <summary>
        /// 获取时区列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTimeZoneInfoAsync")]
        public Task<List<TimeZoneInfoDto>> GetTimeZoneInfoAsync() => timeZoneInfoQueries.GetAllTimeZoneInfoAsync();

        /// <summary>
        /// 创建界面风格
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateInterfaceStyle")]
        public Task<bool> CreateInterfaceStyle([FromBody] CreateInterfaceStylesCommand command) => mediator.Send(command);

        /// <summary>
        /// 获取风格列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetInterfaceStylesAsync")]
        public Task<List<InterfaceStylesDto>> GetInterfaceStylesAsync() => interfaceStyleQueries.GetAllInterfaceStylesAsync();

        /// <summary>
        /// 编辑界面风格
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateInterfaceStyle")]
        public Task<bool> UpdateInterfaceStyle([FromBody] UpdateInterfaceStylesCommand command) => mediator.Send(command);

        /// <summary>
        /// 删除界面风格
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteInterfaceStyle")]
        public Task<bool> DeleteInterfaceStyle(DeleteInterfaceStylesCommand command) => mediator.Send(command);

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
            var dateTime = DateTime.Now.ToString("yyyyMMdd");
            return await oSSService.PresignedPutObjectAsync(OssDescribeConst.DefaultBucketName, "coffeetemp/" + dateTime + "/" + newFileName, expiryTime);
        }
        #endregion

        #region 广告信息管理
        /// <summary>
        /// 创建广告信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateAdvertisementInfo")]
        public Task<bool> CreateAdvertisementInfo([FromBody] CreateAdvertisementInfoCommand command) => mediator.Send(command);
        /// <summary>
        /// 根据设备Id获取广告信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPost("GetAdvertisementInfoDtoByDeviceId")]
        public Task<AdvertisementInfoDto> GetAdvertisementInfoDtoByDeviceId(long deviceId) => advertisementInfoQueries.GetAdvertisementInfoDtoByDeviceIdAsync(deviceId);
        /// <summary>
        /// 修改广告信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateAdvertisementInfo")]
        public Task<bool> UpdateAdvertisementInfo([FromBody] UpdateAdvertisementInfoCommand command) => mediator.Send(command);
        #endregion

        #region 字典

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
        #endregion

        /// <summary>
        /// 设置通知
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SetNoticeCfgAsync")]
        public Task<bool> SetNoticeCfgAsync([FromBody] SetNoticeCfgCommand command) => mediator.Send(command);

        /// <summary>
        /// 查询通知
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>字典集合</returns>
        [HttpGet("GetNoticeCfgAsync")]
        public async Task<List<NotityCfgOutput>> GetNoticeCfgAsync(int type)
        {
            return await noticeCfgQueries.GetNoticeCfgsAsync(type);
        }

        /// <summary>
        /// 通知消息查询
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>字典集合</returns>
        [HttpPost("GetNotityMsgAsync")]
        public async Task<PagedResultDto<NotityMsg>> GetNotityMsgAsync([FromBody] NotityMsgInput input)
        {
            return await noticeCfgQueries.GetNotityMsgAsync(input);
        }
    }
}