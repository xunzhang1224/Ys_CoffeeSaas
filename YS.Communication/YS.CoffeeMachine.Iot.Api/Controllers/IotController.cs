using AutoMapper;
using DotNetCore.CAP;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using YS.AMP.SDK.Util;
using YS.AMP.Shared.Enum;
using YS.AMP.Shared.Request;
using YS.AMP.Shared.Response;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto;
using YS.CoffeeMachine.Iot.Api.Iot.Interfaces;
using YS.CoffeeMachine.Iot.Api.Services;
using YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO.Base;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

namespace YS.CoffeeMachine.Iot.Api.Controllers
{
    /// <summary>
    /// Iot下发接口
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class IotController(ILogger<IotController> _logger, ICommandSenderService _commandSender, IGrpcCommandService _grpcCommand, DeviceService _deviceService,IMapper _mapper, IPublishService _capPublish, IConfiguration _cfg) : Controller
    {
        /// <summary>
        /// 查询机器是否在线
        /// </summary>
        /// <param name="vendNo">机器编号</param>
        /// <returns>返回是否在线</returns>
        [HttpGet("IsOnline")]
        public async Task<bool> IsOnlineAsync(string mid)
        {
            _logger.LogInformation($"查询设备在线状态{mid}");
            return await _commandSender.IsOnlineAsync(new DownSeedRequestBase<string>("", mid));
        }

        /// <summary>
        ///应用管理平台回调通知
        /// </summary>
        /// <param name="vendCode"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("AmpCallBackNotify")]
        [ProducesResponseType(200)]
        public async Task<dynamic> AmpCallBackNotifyAsync(AmpBackEventInput input)
        {
            try
            {
                // 启动缓冲
                Request.EnableBuffering();
                var accessSecret = _cfg["AmpSdk:AccessSecret"];
                if (accessSecret == null) throw new Exception("未配置密钥！");
                //验签
                //_logger.LogInformation($"accessSecret:{accessSecret};Request:{JsonConvert.SerializeObject(Request)}");
                var ck = HmacAuthValidator.ValidateHmacSignature(Request, accessSecret);
                if (!ck.Item1)
                {
                    //验签失败
                    return new AmpResponse<bool> { Success = false, Code = 401, Message = ck.Item2 };
                }
                switch (input.EventType)
                {
                    //获取机器信息
                    case EventTypeEnum.FetchMachineState:
                        var fetchMachine = JsonConvert.DeserializeObject<FetchMachineInput>(input.Content);

                        //查询机器信息
                        var uploadMachine = await _deviceService.GetUploadMachineAsync(fetchMachine);
                        return new AmpResponse<AmpPagedList<UploadMachineOutput>>()
                        {
                            Message = "获取机器信息成功",
                            Result = uploadMachine,
                            Code = 200,
                            Time = DateTime.Now,
                            Type = "success"
                        };

                    // 机器下发升级
                    case EventTypeEnum.PushUpdate:
                        var pushUpdate = JsonConvert.DeserializeObject<PushUpdateDto>(input.Content);
                        var deviceCode = pushUpdate.ProductionNumber;
                        var device = await _deviceService.GetDeviceBaseAsync(pushUpdate.ProductionNumber);
                        if (device == null) throw new Exception("没有找到机器信息");
                        var releases = pushUpdate.Releases;
                        var isOnLine = await _commandSender.IsOnlineAsync(new DownSeedRequestBase<string>("", device.Mid));
                        if (!isOnLine) throw new Exception("机器不在线，无法下发升级指令！");
                        #region 升级方法调用
                        var i = new GeneralSeedInput()
                        {
                            Mid = device.Mid,
                            Method = "1203",
                            TransId = pushUpdate.Releases.TransId.ToString(),
                            Params = JsonConvert.SerializeObject(pushUpdate)
                        };
                        await _capPublish.SendMessage(CapConst.GeneralSeed, i);

                        _logger.LogInformation($"程序升级---------》{JsonConvert.SerializeObject(i)}");
                        #endregion
                        return new AmpResponse<bool>()
                        {
                            Message = "下发升级成功",
                            Result = true,
                            Code = 200,
                            Time = DateTime.Now,
                            Type = "success"
                        };
                    default:
                        throw new Exception("没有对应回调实现");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"应用管理平台回调通知异常，错误信息：{ex.Message}");
                return new AmpResponse<UploadMachineOutput>()
                {
                    Message = ex.Message,
                    Result = null,
                    Code = 400,
                    Time = DateTime.Now,
                    Type = "error"
                };
            }

        }

        #region 测试

        /// <summary>
        /// 开门
        /// </summary>
        /// <param name="vendNo">机器编号</param>
        /// <returns>返回是否在线</returns>
        [HttpPost("OpenDoor")]
        public async Task<UplinkEntity9030.Response> OpenDoor(UplinkEntity9030.Request request)
        {
            return await _grpcCommand.Uplink9030HandleAsync(request);
        }

        /// <summary>
        /// 清洗预警
        /// </summary>
        /// <param name="vendNo">机器编号</param>
        /// <returns>返回是否在线</returns>
        [HttpPost("FulshYj")]
        public async Task<UplinkEntity9033.Response> FulshYj(UplinkEntity9033.Request request)
        {
            return await _grpcCommand.Uplink9033HandleAsync(request);
        }

        /// <summary>
        /// 测试升级回复
        /// </summary>
        /// <returns>返回是否在线</returns>
        [HttpPost("HFYYPT")]
        public async Task HFYYPT(UpdateResultInput input)
        {
            await _capPublish.SendMessage(CapConst.SeedYYPSoftUpdate, input);
        }

        /// <summary>
        /// 通用下发测试
        /// </summary>
        /// <returns>返回是否在线</returns>
        [HttpPost("TestSeed")]
        public async Task TestSeed(GeneralSeedInput input)
        {
            await _capPublish.SendMessage(CapConst.GeneralSeed, input);
        }

        /// <summary>
        /// 清洗预警
        /// </summary>
        /// <param name="vendNo">机器编号</param>
        /// <returns>返回是否在线</returns>
        [HttpPost("Err")]
        public async Task<bool> Err(UplinkEntity5204.Request request)
        {
            return await _grpcCommand.Uplink5204HandleAsync(request);
        }

        /// <summary>
        /// 清洗预警
        /// </summary>
        /// <param name="vendNo">机器编号</param>
        /// <returns>返回是否在线</returns>
        [HttpPost("Soft")]
        public async Task<UplinkEntity1204.Response> Soft(UplinkEntity1204.Request request)
        {
            return await _grpcCommand.Uplink1204HandleAsync(request);
        }

        /// <summary>
        /// 清洗预警
        /// </summary>
        /// <param name="vendNo">机器编号</param>
        /// <returns>返回是否在线</returns>
        [HttpPost("OutGood")]
        public async Task<UplinkEntity4201.Response> OutGood(UplinkEntity4201.Request request)
        {
            return await _grpcCommand.Uplink4201HandleAsync(request);
        }

        /// <summary>
        /// 饮品上报
        /// </summary>
        /// <param name="vendNo">机器编号</param>
        /// <returns>返回是否在线</returns>
        [HttpPost("UplinkGood")]
        public async Task<UplinkEntity9027.Response> UplinkGood(UplinkEntity9027.Request request)
        {
            return await _grpcCommand.Uplink9027HandleAsync(request);
        }
        #endregion
    }
}