using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Runtime;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Controllers.Aop;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.API.Services;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Application.Dtos.IotDto;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;
using static YS.CoffeeMachine.API.Extensions.Cap.Dtos.Drinks9026Dto;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// iotsend控制器
    /// </summary>
    /// <param name="_capPublish"></param>
    /// <param name="_user"></param>
    /// <param name="_iotBaseService"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.IotSend))]
    public class IotSendController(IPublishService _capPublish, UserHttpContext _user, IotBaseService _iotBaseService) : Controller
    {
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
        [HttpPost("SendDeviceCommandBydic")]
        public async Task SendDeviceCommandBydic([FromBody] CommandDownSendBydic input)
        {
            var code = YitIdHelper.NextId().ToString();
            var subs = new List<OperationSubLog>();
            foreach (var item in input.Params)
            {
                var mid = item.Key;
                var p = item.Value;
                var device = await _iotBaseService.GetDeviceInfoAsync(mid);
                var devicebase = await _iotBaseService.GetBaseInfoAsync(mid);
                var model = await _iotBaseService.GetDeviceModelInfoAsync(devicebase.DeviceModelId ?? 0);
                var sub = new OperationSubLog(model?.Name ?? "", device?.Name ?? " ", devicebase?.MachineStickerCode ?? "", null, null, null,
                    OperationResultEnum.CommandIssued, null, p);
                if (await _iotBaseService.IsOnline(mid))
                {
                    await _capPublish.SendMessage(CapConst.GeneralSeed, new CommandDownSend()
                    {
                        Params = p,
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
                var method = "IOTSend" + input.Method;
                if (input.Method == "6216")
                {
                    var info = JsonConvert.DeserializeObject<Iot6216>(p);
                    method += $"_{info.CapabilityId}";
                    if (info.CapabilityId == 63)
                    {
                        var infosub = JsonConvert.DeserializeObject<Iot6216_63>(info.Parameters[0]);
                        method += $"_{infosub.Lock.ToString().ToUpper()}";
                    }
                }
                var logInfo = new CreateOperationLogInput(code, devicebase?.MachineStickerCode ?? "", method, "/api/IotSend/SendDeviceCommandBydic",
                       RequestTypeEnum.Iot, "POST", "", _user.UserId, _user.NickName, _user.TenantId, subs);
                await _capPublish.SendMessage(CapConst.CreateOperationLog, logInfo);
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
                var info = new Drinks9026Dto()
                {
                    IsApply = true,
                    Mid = mid
                };
                info.CoffeeInfo = JsonConvert.DeserializeObject<List<Beverage>>(input.Params);
                var device = await _iotBaseService.GetDeviceInfoAsync(mid);
                var devicebase = await _iotBaseService.GetBaseInfoAsync(mid);
                var model = await _iotBaseService.GetDeviceModelInfoAsync(devicebase.DeviceModelId ?? 0);
                var sub = new OperationSubLog(model?.Name ?? "", device?.Name ?? " ", devicebase?.MachineStickerCode ?? "", null, null, null,
                    OperationResultEnum.CommandIssued, null, input.Params);
                if (await _iotBaseService.IsOnline(mid))
                {
                    await _capPublish.SendMessage(CapConst.GeneralSeed, new CommandDownSend()
                    {
                        Params = JsonConvert.SerializeObject(info),
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
                var logInfo = new CreateOperationLogInput(code, devicebase?.MachineStickerCode ?? "", "IOTSend" + input.Method, "/api/IotSend/SendDeviceCommandRangs",
                       RequestTypeEnum.Iot, "POST", "", _user.UserId, _user.NickName, _user.TenantId, subs);
                await _capPublish.SendMessage(CapConst.CreateOperationLog, logInfo);
            }
        }

        /// <summary>
        /// 批量下发
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("SendDeviceDrinkRangs")]
        public async Task SendDeviceDrinkRangs([FromBody] DrinkCommandDownSends input)
        {
            var code = YitIdHelper.NextId().ToString();
            var subs = new List<OperationSubLog>();
            foreach (var data in input.Datas)
            {
                var info = JsonConvert.DeserializeObject<Drinks9026Dto>(input.Params);
                if (input.Datass.TryGetValue(data.Key, out string d))
                {
                    if (!string.IsNullOrWhiteSpace(d))
                    {
                        info = JsonConvert.DeserializeObject<Drinks9026Dto>(d);
                    }
                }
                if (string.IsNullOrWhiteSpace(data.Value))
                {
                    info.IsApply = false;
                }
                else
                {
                    info.IsApply = true;
                    info.Sku = data.Value;
                }
                info.Mid = data.Key;
                var device = await _iotBaseService.GetDeviceInfoAsync(data.Key);
                var devicebase = await _iotBaseService.GetBaseInfoAsync(data.Key);
                var model = await _iotBaseService.GetDeviceModelInfoAsync(devicebase.DeviceModelId ?? 0);
                var yy = input.Yys[data.Key];
                var sub = new OperationSubLog(model?.Name ?? "", device?.Name ?? " ", devicebase?.MachineStickerCode ?? "", yy.AppliedType, null, yy.ReplaceTarget,
                    OperationResultEnum.CommandIssued, null, JsonConvert.SerializeObject(info));
                if (await _iotBaseService.IsOnline(data.Key))
                {
                    // 如果新增没有sku（逻辑）的商品，需要特殊处理
                    if (input.Yys != null && input.Yys[data.Key] != null && input.Yys[data.Key].NewSku != null && info.CoffeeInfo.Count > 0)
                    {
                        info.CoffeeInfo[0].Sku = input.Yys[data.Key].NewSku.ToString();
                    }

                    await _capPublish.SendMessage(CapConst.GeneralSeed, new CommandDownSend()
                    {
                        Params = JsonConvert.SerializeObject(info),
                        Mid = data.Key,
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
                var logInfo = new CreateOperationLogInput(code, devicebase?.MachineStickerCode ?? "", "IOTSend" + input.Method, "/api/IotSend/SendDeviceDrinkRangs",
                       RequestTypeEnum.Iot, "POST", "", _user.UserId, _user.NickName, _user.TenantId, subs);
                await _capPublish.SendMessage(CapConst.CreateOperationLog, logInfo);
            }
        }
    }
}