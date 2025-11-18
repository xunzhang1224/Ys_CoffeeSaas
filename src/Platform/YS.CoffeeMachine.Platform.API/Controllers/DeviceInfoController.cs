using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Application.PlatformDto.DeviceDots;
using YS.CoffeeMachine.Application.PlatformQueries.IDevicesQueries;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using YS.CoffeeMachine.Application.Commands.DeviceBaseCommands;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands;
using System.Dynamic;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 设备信息
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="deviceAllocationQueries"></param>
    /// <param name="deviceInfoQueries"></param>
    /// <param name="deviceModelQueries"></param>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class DeviceInfoController(IMediator mediator, IP_DeviceAllocationQueries deviceAllocationQueries,
        IDeviceInfoQueries deviceInfoQueries, IP_DeviceModelQueries deviceModelQueries, IDeviceBaseQueries _deviceBaseQueries) : Controller
    {

        #region 设备分配
        /// <summary>
        /// 获取设备分配分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceAllocationPageList")]
        public async Task<PagedResultDto<DeviceAllocationDto>> GetDeviceAllocationPageListAsync([FromBody] DeviceAllocationInput request) => await deviceAllocationQueries.GetDeviceAllocationPageListAsync(request);
        /// <summary>
        /// 设备分配/用户换绑
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("EnterpriseDeviceAllocation")]
        public async Task<bool> EnterpriseDeviceAllocation([FromBody] P_EnterpriseDeviceAllocationCommand command) => await mediator.Send(command);
        /// <summary>
        /// 设备用户解绑
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ClearDeviceUserAssociation")]
        public async Task<bool> ClearDeviceUserAssociation([FromBody] P_ClearDeviceUserAssociationCommand command) => await mediator.Send(command);

        /// <summary>
        /// 设备解绑企业
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("DeviceUnbindEnterprise")]
        public async Task DeviceUnbindEnterpriseAsync([FromBody] DeviceUnbindEnterpriseCommand command) => await mediator.Send(command);
        #endregion

        #region 设备管理

        /// <summary>
        /// 获取设备分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetDevicePageList")]
        public async Task<PagedResultDto<DeviceListDto>> GetDevicePageListAsync([FromBody] DeviceListInput request) => await deviceInfoQueries.GetDevicePageListAsync(request);

        /// <summary>
        /// 根据Id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceInfoById")]
        public async Task<DeviceInfoDto> GetDeviceInfoById([FromQuery] long id) => await deviceInfoQueries.GetDeviceInfoByIdAsync(id);

        /// <summary>
        /// 设备激活
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ActivationDevice")]
        public async Task ActivationDeviceAsync(ActivationDeviceCommand command) => await mediator.Send(command);
        #endregion

        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateDeviceCapacityCfgs")]
        public Task<bool> UpdateDeviceCapacityCfgs([FromBody] UpdateDeviceCapacityCfgCommand command) => mediator.Send(command);

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceCapacityCfgs")]
        public async Task<List<DeviceCapacityCfg>> GetDeviceCapacityCfgs(long deviceBaseId) => await _deviceBaseQueries.GetDeviceCapacityCfgs(deviceBaseId);

        #region 设备版本管理

        /// <summary>
        /// 创建管理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateDeviceVersionManage")]
        public async Task CreateDeviceVersionManageAsync([FromBody] CreateDeviceVersionManageCommand command) => await mediator.Send(command);

        /// <summary>
        /// 更新管理状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateDeviceVersionManageState")]
        public async Task UpdateDeviceVersionManageStateAsync([FromBody] UpdateDeviceVersionManageStateCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取管理分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceVersionManageList")]
        public async Task<PagedResultDto<DeviceVersionManageDto>> GetDeviceVersionManageListAsync([FromBody] DeviceVersionManageInput input)
        {
            return await _deviceBaseQueries.GetDeviceVersionManageList(input);
        }

        /// <summary>
        /// 设备升级列表New
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceVersionUpdateRecordNew")]
        public async Task<PagedResultDto<ExpandoObject>> GetDeviceVersionUpdateRecordNew([FromBody] DeviceVersionUpdateRecordInput input)
        {
            return await _deviceBaseQueries.GetDeviceVersionUpdateRecordNew(input);
        }

        /// <summary>
        /// 设备升级列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDeviceVersionUpdateRecord")]
        public async Task<PagedResultDto<DeviceVersionUpdateRecordDto>> GetDeviceVersionUpdateRecordAsync([FromBody] DeviceVersionUpdateRecordInput input)
        {
            return await _deviceBaseQueries.GetDeviceVersionUpdateRecord(input);
        }

        /// <summary>
        /// 推送版本到设备
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("PushVersionToDevice")]
        public async Task<List<PushVersionDto>> PushVersionToDeviceAsync([FromBody] PushVersionToDeviceCommand command) => await mediator.Send(command);

        /// <summary>
        /// 更新记录(根据版本)
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetPushDataByVersion")]
        public async Task<PagedResultDto<PushDataByVersionDto>> GetPushDataByVersionAsync([FromBody] PushDataByVersionInput input)
        {
            return await _deviceBaseQueries.GetPushDataByVersion(input);
        }

        /// <summary>
        /// 更新记录(根据设备)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetPushDateByDevice")]
        public async Task<PagedResultDto<PushDataByDeviceDto>> GetPushDateByDeviceAsync([FromBody] PushDataByDeviceInput input)
        {
            return await _deviceBaseQueries.GetPushDateByDevice(input);
        }
        #endregion

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
        /// 获取设备型号列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDeviceModelList")]
        public async Task<List<object>> GetDeviceModelListAsync() => await deviceModelQueries.GetDeviceModelListAsync();

        /// <summary>
        /// 获取设备型号列表（分页）
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDeviceModelListPage")]
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

        /// <summary>
        /// 获取指标
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceMetrics")]
        public async Task<List<DeviceMetrics>> GetDeviceMetrics(long deviceBaseId) => await _deviceBaseQueries.GetDeviceMetrics(deviceBaseId);

        /// <summary>
        /// 获取设备其他信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceOtherMsg")]
        public async Task<List<DeviceOtherMsgDto>> GetDeviceOtherMsgAsync(long deviceId) => await _deviceBaseQueries.GetDeviceOtherMsg(deviceId);

        /// <summary>
        /// 设备统计
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDeivceCount")]
        public async Task<dynamic> GetDeivceCount() => await _deviceBaseQueries.GetDeivceCount();
    }
}
