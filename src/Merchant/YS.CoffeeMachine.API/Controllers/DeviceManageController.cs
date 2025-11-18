using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using YS.CoffeeMachine.API.Application.Services;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.DevicesCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.BindDeviceServiceProvidersCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceBaseCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceUserAssociationCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.EarlyWarningConfigCommands;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.EnterpriseDevicesCommand;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.DeviceDots.Groups;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.ICountryAndRegionQueries;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Domain.Shared.Enum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 设备管理
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="deviceInfoQueries"></param>
    /// <param name="enterpriseDevicesQueries"></param>
    /// <param name="groupsQueries"></param>
    /// <param name="countryInfoQueries"></param>
    /// <param name="deviceModelQueries"></param>
    /// <param name="deviceModelRepository"></param>
    /// <param name="earlyWarningConfigQueries"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class DeviceManageController(IMediator mediator, IDeviceInfoQueries deviceInfoQueries,
        IEnterpriseDevicesQueries enterpriseDevicesQueries, IGroupsQueries groupsQueries, ICountryInfoQueries countryInfoQueries, IDevicePaymentQueries devicePaymentQueries,
        IdeviceModelQueries deviceModelQueries, IDeviceModelRepository deviceModelRepository, IEarlyWarningConfigQueries earlyWarningConfigQueries, IDeviceBaseQueries _deviceBaseQueries) : Controller
    {
        #region 设备型号管理
        /// <summary>
        /// 添加设备型号
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateDeviceModel")]
        public Task<bool> CreateDeviceModel([FromBody] CreateDeviceModelCommand command) => mediator.Send(command);

        /// <summary>
        /// 根据Id获取设备型号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceModelById")]
        public Task<DeviceModelDto> GetDeviceModelById(long id) => deviceModelQueries.GetDeviceModelAsync(id);

        /// <summary>
        /// 获取设备型号列表（分页）
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDeviceModelList")]
        public Task<PagedResultDto<DeviceModelDto>> GetDeviceModelList([FromBody] QueryRequest query) => deviceModelQueries.GetDeviceModelListAsync(query);

        /// <summary>
        /// 编辑设备型号
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateDeviceModel")]
        public Task<bool> UpdateDeviceModel([FromBody] UpdateDeviceModelCommand command) => mediator.Send(command);

        /// <summary>
        /// 删除设备型号
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDeviceModelById")]
        public Task<bool> DeleteDeviceModelById(DeleteDeviceModelCommand command) => mediator.Send(command);
        #endregion

        #region 设备信息相关
        /// <summary>
        /// 批量导入设备Excel模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("download-template")]
        public IActionResult DownloadTemplate()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "device_template.xlsx");
            // 读取 Excel 文件
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            IWorkbook workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(0); // 假设操作第一个工作表

            #region 定义下拉框选项
            // 设备型号
            var deviceModels = deviceModelRepository.GetDeviceModelsAsync().GetAwaiter().GetResult().Select(s => s.Name).ToArray();
            // 国家名称集合
            var countrys = countryInfoQueries.GetCountryInfoListDtosAsync().GetAwaiter().GetResult();
            var countryList = countrys.CountryInfoList.Select(s => s.CountryName).ToArray();
            // 使用场景
            var usageScenarios = EnumHelper.GetEnumDescriptions<UsageScenarioEnum>();
            #endregion
            // 定义下拉框范围（第2行到第100行，B列）
            CellRangeAddressList addDeviceModelList = new CellRangeAddressList(1, 99, 3, 3);
            CellRangeAddressList addCountryList = new CellRangeAddressList(1, 99, 4, 4);
            CellRangeAddressList addUsageScenarioList = new CellRangeAddressList(1, 99, 6, 6);

            // 创建数据验证对象
            IDataValidationHelper validationHelper = sheet.GetDataValidationHelper();
            // 设备型号
            IDataValidationConstraint constraintDeviceModels = validationHelper.CreateExplicitListConstraint(deviceModels);
            IDataValidation validationDeviceModels = validationHelper.CreateValidation(constraintDeviceModels, addDeviceModelList);
            // 国家列表
            IDataValidationConstraint constraintCountrys = validationHelper.CreateExplicitListConstraint(countryList);
            IDataValidation validationCountrys = validationHelper.CreateValidation(constraintCountrys, addCountryList);
            // 场景列表
            IDataValidationConstraint constraintUsageScenarios = validationHelper.CreateExplicitListConstraint(usageScenarios.ToArray());
            IDataValidation validationAddUsageScenarios = validationHelper.CreateValidation(constraintUsageScenarios, addUsageScenarioList);

            // 设置数据验证为强制模式（拒绝手动输入）
            validationDeviceModels.ShowErrorBox = true;
            sheet.AddValidationData(validationDeviceModels);

            validationCountrys.ShowErrorBox = true;
            sheet.AddValidationData(validationCountrys);

            validationAddUsageScenarios.ShowErrorBox = true;
            sheet.AddValidationData(validationAddUsageScenarios);

            // 将修改后的 Excel 写入到内存流
            using var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            //memoryStream.Position = 0;

            // 释放资源
            workbook.Close();

            // 返回文件到前端
            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "device_template.xlsx");
        }

        /// <summary>
        /// 创建设备信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateDeviceInfo")]
        public Task<bool> CreateDeviceInfo([FromBody] CreateDeviceInfoCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 通过Id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceInfoById{id}")]
        public Task<DeviceInfoDto> GetDeviceInfoById(long id)
        {
            return deviceInfoQueries.GetDeviceInfoAsync(id);
        }

        /// <summary>
        /// 根据设备Id获取当前设备下，同型号的设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceInfoListByDeviceId")]
        public Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoListByDeviceIdAsync([FromBody] DeviceInfoListByDeviceIdInput request)
        {
            return deviceInfoQueries.GetDeviceInfoListByDeviceIdAsync(request);
        }

        /// <summary>
        /// 获取可分配的设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetUnDeviceInfoListAsync")]
        public Task<PagedResultDto<DeviceInfoDto>> GetUnDeviceInfoListAsync([FromBody] UnDeviceInput request)
        {
            return deviceInfoQueries.GetUnDeviceInfoListAsync(request);
        }

        /// <summary>
        /// 根据设备Id获取饮品信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPost("GetBeverageInfoByDeviceId")]
        public Task<List<object>> GetBeverageInfoByDeviceIdAsync([FromQuery] long deviceId)
        {
            return deviceInfoQueries.GetBeverageInfoByDeviceIdAsync(deviceId);
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceInfoList")]
        public Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoList([FromBody] DevicesListInput request)
        {
            return deviceInfoQueries.GetDeviceInfoListAsync(request);
        }

        /// <summary>
        /// 根据设备型号获取当前企业设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceInfoListByDeviceModelId")]
        public Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoListByDeviceModelId([FromBody] DeviceInfoListByDeviceModelIdInput request) => deviceInfoQueries.GetDeviceInfoListByDeviceModelId(request);
        /// <summary>
        /// 根据设备Id获取用户Id集合
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPost("GetUserIdsByDeviceId")]
        public Task<List<long>> GetUserIdsByDeviceId(long deviceId) => deviceInfoQueries.GetUserIdsByDeviceId(deviceId);
        /// <summary>
        /// 根据设备Id获取分组Id集合
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPost("GetGroupIdsByDeviceId")]
        public Task<List<long>> GetGroupIdsByDeviceId(long deviceId) => deviceInfoQueries.GetGroupIdsByDeviceId(deviceId);
        /// <summary>
        /// 编辑设备信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateDeviceInfo")]
        public Task<bool> UpdateDeviceInfo([FromBody] UpdateDeviceInfoCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 根据Ids批量删除设备信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDeviceInfoByIds")]
        public Task<bool> DeleteDeviceInfoByIds([FromBody] DeleteDeviceInfoCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 更改设备名称
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateDeviceName")]
        public Task<bool> UpdateDeviceName([FromBody] UpdateDeviceNameCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 更新设备点位
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateDevicePoint")]
        public Task<bool> UpdateDevicePoint([FromBody] UpdateDevicePointCommand command)
        {
            return mediator.Send(command);
        }
        #endregion

        #region 设备分配
        /// <summary>
        /// 设备分配
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateEnterpriseDevices")]
        public Task<bool> CreateEnterpriseDevices([FromBody] CreateEnterpriseDevicesCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 通过企业Id获取设备分配信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetEnterpriseDevicesById")]
        public Task<EnterpriseDevicesDto> GetEnterpriseDevicesById(long id)
        {
            return enterpriseDevicesQueries.GetEnterpriseDevicesAsync(id);
        }

        /// <summary>
        /// 获取设备分配列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetEnterpriseDevicesList")]
        public Task<PagedResultDto<EnterpriseDevicesDto>> GetEnterpriseDevicesList([FromBody] EnterpriseDevicesInput query)
        {
            return enterpriseDevicesQueries.GetEnterpriseDevicesListAsync(query);
        }

        /// <summary>
        /// 编辑设备分配
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateEnterpriseDevices")]
        public Task<bool> UpdateEnterpriseDevices([FromBody] UpdateEnterpriseDevicesCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 根据Id获取预警配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetEarlyWarningConfigByDeviceId")]
        public Task<EarlyWarningConfigDto> GetEarlyWarningConfigById(long deviceId)
        {
            return earlyWarningConfigQueries.GetEarlyWarningConfigByIdAsync(deviceId);
        }

        /// <summary>
        /// 更新预警配置
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateEarlyWarningConfig")]
        public Task<bool> UpdateEarlyWarningConfig([FromBody] UpdateEarlyWarningConfigCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 取消企业设备分配、并删除分配记录
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeletEnterpriseDevicesById")]
        public Task<bool> DeleteEnterpriseDevicesById(DeleteEnterpriseDevicesCommand command)
        {
            return mediator.Send(command);
        }
        #endregion

        #region 设备分组
        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateGroups")]
        public Task<bool> CreateGroups([FromBody] CreateGroupsCommand command) => mediator.Send(command);
        /// <summary>
        /// 根据Id获取分组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetGroupsById")]
        public Task<GroupsTreeDto> GetGroupsById(long id) => groupsQueries.GetGroupsAsync(id);
        /// <summary>
        /// 根据分组Id获取设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceInfoByGroupId")]
        public Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoByGroupId([FromBody] QueryRequest request, long gid) => groupsQueries.GetDeviceInfoByGroupId(request, gid);
        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetGroupsList")]
        public Task<List<GroupsTreeDto>> GetGroupsList([FromQuery] long enterpriseinfoId) => groupsQueries.GetGroupTreeListAsync(enterpriseinfoId);
        /// <summary>
        /// 获取分组分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetGroupPageList")]
        public Task<PagedResultDto<GroupListDto>> GetGroupPageList([FromBody] GroupListInput input) => groupsQueries.GetGroupPageList(input);
        /// <summary>
        /// 编辑分组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateGroups")]
        public Task<bool> UpdateGroups([FromBody] UpdateGroupsCommand command) => mediator.Send(command);
        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteGroupsById")]
        public Task<bool> DeleteGroupsById(DeleteGroupsCommand command) => mediator.Send(command);
        #endregion

        #region 设备绑定用户
        /// <summary>
        /// 设备多对多绑定用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("BindDeviceUserAssociation")]
        public Task<bool> BindDeviceUserAssociation([FromBody] BindDeviceUserAssociationCommand command) => mediator.Send(command);

        /// <summary>
        /// 用户绑定设备一对多
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UserBindDevices")]
        public Task<bool> UserBindDevices([FromBody] UserBindDeviceCommand command) => mediator.Send(command);
        #endregion

        #region 设备绑定分组
        /// <summary>
        /// 设备绑定分组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("DeviceBindGroups")]
        public Task<bool> DeviceBindGroups([FromBody] DeviceBindGroupsCommand command) => mediator.Send(command);
        /// <summary>
        /// 分组绑定设备
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("GroupBindDevices")]
        public Task<bool> GroupBindDevices([FromBody] BindDevicesCommand command) => mediator.Send(command);
        /// <summary>
        /// 分组与设备多对多绑定
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("MultipleGroupsBindDevices")]
        public Task<bool> MultipleGroupsBindDevices([FromBody] MultipleGroupsBindDevicesCommand command) => mediator.Send(command);
        /// <summary>
        /// 移除分组设备
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("RemoveDevices")]
        public Task<bool> RemoveDevices([FromBody] RemoveDevicesCommand command) => mediator.Send(command);
        #endregion

        #region 绑定服务商
        /// <summary>
        /// 设备绑定服务商
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("BindDeviceServiceProviders")]
        public Task<bool> BindDeviceServiceProviders([FromBody] BindDeviceServiceProvidersCommand command) => mediator.Send(command);
        #endregion

        #region 设备支付相关
        /// <summary>
        /// 获取指定支付方式下绑定的设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetBildDeviceList")]
        public async Task<PagedResultDto<DevicePaymentDto>> GetBildDeviceList([FromBody] DevicePaymentInput input)
        {
            return await devicePaymentQueries.GetBildDeviceList(input);
        }

        /// <summary>
        /// 获取指定支付方式下未绑定的设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetNotBildDeviceList")]
        public async Task<PagedResultDto<DevicePaymentDto>> GetNotBildDeviceList([FromBody] DevicePaymentInput input)
        {
            return await devicePaymentQueries.GetNotBildDeviceList(input);
        }
        #endregion

        #region 设备广告

        /// <summary>
        /// 添加设备广告配置
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateDeviceAd")]
        public async Task CreateDeviceAdAsync([FromBody] CreateDeviceAdCommand command) => await mediator.Send(command);

        /// <summary>
        /// 更新设备广告配置
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateDeviceAd")]
        public async Task UpdateDeviceAdAsync([FromBody] UpdateDeviceAdCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取设备广告
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceAd")]
        public async Task<DeviceAdInput> GetDeviceAdAsync(long deviceId)
        {
            return await deviceInfoQueries.GetDeviceAd(deviceId);
        }
        #endregion

        #region 货币信息

        /// <summary>
        /// 获取货币列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrentList")]
        public async Task<List<CurrentDto>> GetCurrentList() => await deviceInfoQueries.GetCurrentList();
        #endregion

        ///// <summary>
        ///// 远程控制设备
        ///// </summary>
        ///// <param name="command"></param>
        ///// <returns></returns>
        //[HttpPut("RemoteControlDevice")]
        //public Task<bool> RemoteControlDevice([FromBody] CommandDownSend command)
        //{
        //     command.TransId = YitIdHelper.NextId().ToString();
        //    httpClientFactory.CreateClient();

        //}

        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateDeviceCapacityCfgs")]
        public Task<bool> UpdateDeviceCapacityCfgs([FromBody] UpdateDeviceCapacityCfgCommand command) => mediator.Send(command);

        /// <summary>
        /// 批量修改支付方式
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateDevicesZFType")]
        public Task<bool> UpdateDevicesZFType([FromBody] UpdateDevicesZFTypeCommand command) => mediator.Send(command);

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceCapacityCfgs")]
        public async Task<List<DeviceCapacityCfg>> GetDeviceCapacityCfgs(long deviceBaseId) => await _deviceBaseQueries.GetDeviceCapacityCfgs(deviceBaseId);

        /// <summary>
        /// 获取指标
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceMetrics")]
        public async Task<List<DeviceMetricsOutput>> GetDeviceMetrics(long deviceBaseId) => await _deviceBaseQueries.GetDeviceMetrics(deviceBaseId);

        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceMaterialInfos")]
        public async Task<List<DeviceMaterialInfo>> GetDeviceMaterialInfos(long deviceBaseId, MaterialTypeEnum? type = null) => await _deviceBaseQueries.GetDeviceMaterialInfos(deviceBaseId, type);

        /// <summary>
        /// 获取物料信息与预警
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceWarnings")]
        public async Task<List<DeviceEarlyWarningsOutput>> GetDeviceWarnings(long deviceBaseId) => await _deviceBaseQueries.GetDeviceWarnings(deviceBaseId);

        /// <summary>
        /// 获取预警
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceWarningsAll")]
        public async Task<List<DeviceEarlyWarningsOutput>> GetDeviceWarningsAll(long deviceBaseId) => await _deviceBaseQueries.GetDeviceWarningsAll(deviceBaseId);

        /// <summary>
        /// 修改预警
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateDeviceWarnings")]
        public Task<bool> UpdateMaterials([FromBody] UpdateDeviceMaterialWarningCommand command) => mediator.Send(command);

        /// <summary>
        /// 获取异常记录
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceAbnormals")]
        public async Task<List<DeviceAbnormal>> GetDeviceAbnormals(long deviceBaseId) => await _deviceBaseQueries.GetDeviceAbnormals(deviceBaseId);

        /// <summary>
        /// 激活设备
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ActiveDevice")]
        public async Task ActiveDeviceAsync([FromBody] ActiveDeviceCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取设备其他信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceOtherMsg")]
        public async Task<List<DeviceOtherMsgDto>> GetDeviceOtherMsgAsync(long deviceId) => await _deviceBaseQueries.GetDeviceOtherMsg(deviceId);

        #region 设备日志

        /// <summary>
        /// 获取设备在线日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceOnlineLog")]
        public async Task<PagedResultDto<DeviceOnlineLogDto>> GetDeviceOnlineLogAsync([FromBody] DeviceOnlineLogInput input)
        {
            return await _deviceBaseQueries.GetDeviceOnlineLog(input);
        }

        /// <summary>
        /// 获取设备事件日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceEventLog")]
        public async Task<PagedResultDto<DeviceEventLogDto>> GetDeviceEventLogAsync([FromBody] DeviceEventLogInput input)
        {
            return await _deviceBaseQueries.GetDeviceEventLog(input);
        }

        /// <summary>
        /// 获取设备异常日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceErrorLog")]
        public async Task<PagedResultDto<DeviceErrorLogDto>> GetDeviceErrorLogAsync([FromBody] DeviceErrorLogInput input)
        {
            return await _deviceBaseQueries.GetDeviceErrorLog(input);
        }
        #endregion

        /// <summary>
        /// 绑定设备
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("BindDevice")]
        public async Task BindDeviceAsync([FromBody] BindDeviceCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取设备base info信息(绑定时需要)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDeviceBaseInfo")]
        public async Task<DeviceBaseInfoForBind> GetDeviceBaseInfoAsync(string code)
        {
            return await _deviceBaseQueries.GetDeviceBaseInfo(code);
        }

        /// <summary>
        /// 清除设备关联
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ClearDeviceRelationships")]
        public async Task<bool> ClearDeviceRelationshipsAsync([FromBody] ClearDeviceRelationshipsCommand command) => await mediator.Send(command);

        /// <summary>
        /// 设备补货
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("DeviceRestock")]
        public async Task<bool> DeviceRestock([FromBody] DeviceRestockCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取设备清洗日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceFlushLogs")]
        public async Task<PagedResultDto<DeviceFlushComponentsLog>> GetDeviceFlushLogs([FromBody] GetDeviceFlushLogInput input)
        {
            return await _deviceBaseQueries.GetDeviceFlushLog(input);
        }

        /// <summary>
        /// 设备补货记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceRestockLogs")]
        public async Task<PagedResultDto<DeviceRestockLogDto>> GetDeviceRestockLogs([FromBody] GetDeviceRestockLogInput input)
        {
            return await _deviceBaseQueries.GetDeviceRestockLogs(input);
        }

        /// <summary>
        /// 获取设备在线状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceOnLineStatus")]
        public async Task<bool> GetDeviceOnLineStatus(long id) => await _deviceBaseQueries.GetDeviceOnLineStatus(id);
    }
}
