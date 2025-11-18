using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Controllers.Aop;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Platform.API.Services;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// IotSendController
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="_capPublish"></param>
    /// <param name="_user"></param>
    /// <param name="_iotBaseService"></param>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.IotSend))]
    public class IotSendController(IMediator mediator, IPublishService _capPublish, UserHttpContext _user, IotBaseService _iotBaseService,ILogger<IotSendController> _log) : Controller
    {
        /// <summary>
        /// 三码绑定
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Bind")]
        [ProducesResponseType(200)]
        //[ServiceFilter(typeof(IsOnlineActionFilter))]
        public async Task<UploadBindingInfoOut> BindAsync([FromBody] UploadBindingInfoInput input)
        {
            _log.LogInformation($"三码绑定：{JsonConvert.SerializeObject(input)}");
            var rsp = new UploadBindingInfoOut();
            try
            {
                string mid = input.SN?.ToString();
                var isOnline = await _iotBaseService.IsOnline(mid);
                if (!isOnline)
                    throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0015)]);
                var command = new CreateDeviceBaseInfoCommand(mid, input.CarStickerCode, input.BoxId,input.BoxType);
                rsp.Product_code = await mediator.Send(command);
            }
            catch (Exception ex)
            {
                _log.LogError($"三码绑定报错：{ex.Message}");
                rsp.Msg = ex.Message;
                rsp.Code = 500;
            }
            _log.LogInformation($"三码绑定返回结果：{JsonConvert.SerializeObject(rsp)}");
            return rsp;
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("UnBind")]
        [ProducesResponseType(200)]
        //[ServiceFilter(typeof(IsOnlineActionFilter))]
        public async Task<bool> UnBind([FromQuery] string mid)
        {
            _log.LogInformation($"解绑：{JsonConvert.SerializeObject(mid)}");

            var isOnline = await _iotBaseService.IsOnline(mid);
            if (!isOnline)
                throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0015)]);
            var command = new UnBindCommand(mid);
            return await mediator.Send(command);
        }

        /// <summary>
        /// 下发指令
        /// 重试传code
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("SendDeviceCommand")]
        [ServiceFilter(typeof(DeviceDownSeedActionFilter))]
        public async Task SendDeviceCommand([FromBody] CommandDownSend input)
        {
            await _capPublish.SendMessage(CapConst.GeneralSeed, input);
        }

        /// <summary>
        /// 下发指令
        /// 重试传code
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("SendDeviceSJCommand")]
        public async Task SendDeviceSJCommand([FromBody] CommandSJDownSends input)
        {
            foreach (var dto in input.Dtos)
            {
                var mid = dto.Mid;
                if (await _iotBaseService.IsOnline(mid))
                {
                    await _capPublish.SendMessage(CapConst.GeneralSeed, new CommandDownSend()
                    {
                        Params = dto.Params,
                        Mid = mid,
                        Method = input.Method,
                        TransId = dto.TransId
                    });
                }
            }
        }

        /// <summary>
        /// 批量下发
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("SendDeviceCommandRangs")]
        public async Task SendDeviceCommandRangs([FromBody] CommandDownSends input)
        {
            var code = YitIdHelper.NextId().ToString();
            var subs = new List<OperationSubLog>();
            foreach (var mid in input.Mids)
            {
                var devicebase = await _iotBaseService.GetBaseInfoAsync(mid);
                var device = await _iotBaseService.GetDeviceInfoAsync(mid);
                var model = await _iotBaseService.GetDeviceModelInfoAsync(devicebase.DeviceModelId ?? 0);
                var sub = new OperationSubLog(model?.Name ?? "", device?.Name ?? "", devicebase?.MachineStickerCode ?? "", null, null, null,
                    OperationResultEnum.CommandIssued, null, input.Params);
                if (await _iotBaseService.IsOnline(mid))
                {
                    await _capPublish.SendMessage(CapConst.GeneralSeed, new CommandDownSend()
                    {
                        Params = input.Params,
                        Mid = mid,
                        Method = input.Method,
                        TransId = code
                    });
                }
                else
                {
                    sub.Update(OperationResultEnum.CommandUnexecuted, L.Text[nameof(ErrorCodeEnum.C0015)]);
                }
                subs.Add(sub);
            }

            if (input.IsRecordLog)
            {
                var devicebase = await _iotBaseService.GetBaseInfoAsync(input.Mid);
                var logInfo = new CreateOperationLogInput(code, devicebase?.MachineStickerCode ?? "", "IOTSend" + input.Method, "/papi/IotSend/SendDeviceCommandRangs",
                       RequestTypeEnum.Iot, "POST", "", _user.UserId, _user.NickName, _user.TenantId, subs);
                await _capPublish.SendMessage(CapConst.CreateOperationLog, logInfo);
            }
        }
    }
}
