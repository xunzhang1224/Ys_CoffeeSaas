using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.API.Services;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Application.Dtos.IotDto;
using YS.CoffeeMachine.Application.Dtos.LogDtos;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Controllers.Aop
{
    /// <summary>
    /// DeviceDownSeedActionFilter
    /// </summary>
    /// <param name="_publish"></param>
    /// <param name="_user"></param>
    /// <param name="_iotBaseService"></param>
    public class DeviceDownSeedActionFilter(IPublishService _publish, UserHttpContext _user, IotBaseService _iotBaseService,ILogger<DeviceDownSeedActionFilter> _log) : IAsyncActionFilter
    {
        /// <summary>
        /// OnActionExecutionAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string code = string.Empty;
            string actionException = string.Empty;
            bool isSuccess = false;
            string mid = string.Empty;
            bool isRecordLog = false;
            // 是否重试
            bool isRetry = false;
            try
            {
                // 从表单数据中获取
                if (context.ActionArguments.TryGetValue("input", out var inputObj) &&
                    inputObj is CommandDownSend commandDownSend)
                {
                    mid = commandDownSend.Mid;
                    isRecordLog = commandDownSend.IsRecordLog;
                    if (string.IsNullOrWhiteSpace(commandDownSend.TransId))
                    {
                        code = YitIdHelper.NextId().ToString();
                        commandDownSend.TransId = code;
                    }
                    else
                    {
                        isRetry = true;
                        code = commandDownSend.TransId;
                        // 重试mid前端传的生产编号，转mid
                        var baseinfo = await _iotBaseService
                            .GetBaseInfoByCodeAsync(mid);
                        commandDownSend.Mid = baseinfo.Mid;
                        mid = baseinfo.Mid;
                    }
                    bool checkmid = await _iotBaseService.CheckMid(mid, _user.UserId);
                    if (!checkmid)
                    {
                        actionException = L.Text[nameof(ErrorCodeEnum.C0017)];
                        throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0017)]);
                    }
                    var isOnline = await _iotBaseService.IsOnline(mid);
                    if (!isOnline)
                    {
                        actionException = L.Text[nameof(ErrorCodeEnum.C0015)];
                        throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0015)]);
                    }

                    // 执行原方法
                    await next();
                    isSuccess = true; // 如果没有异常，标记为成功
                }
            }
            catch (Exception ex)
            {
                actionException = ex.Message;
                isSuccess = false;
                _log.LogError($"下发过滤器错误----->{ex}");
                // 可以选择重新抛出异常，让全局异常处理器处理
                throw;
            }
            finally
            {
                if (isRecordLog)
                {
                    if (!isRetry)
                        await LogActionResult(context, isSuccess, actionException, code);
                    else
                        await UpdateLogActionResult(isSuccess, actionException, code, mid);
                }
            }
        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isSuccess"></param>
        /// <param name="exception"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task LogActionResult(ActionExecutingContext context, bool isSuccess, string exception, string code)
        {
            if (context.ActionArguments.TryGetValue("input", out var inputObj) &&
                   inputObj is CommandDownSend commandDownSend)
            {
                var devicebase = await _iotBaseService.GetBaseInfoAsync(commandDownSend.Mid);
                var model = await _iotBaseService.GetDeviceModelInfoAsync(devicebase.DeviceModelId ?? 0);
                var requestPath = context.HttpContext.Request.Path; // 请求路径
                var requestMethod = context.HttpContext.Request.Method; // HTTP方法
                var device = await _iotBaseService.GetDeviceInfoAsync(commandDownSend.Mid);
                var logsubInfo = new OperationSubLog(model?.Name ?? "", device?.Name ?? " ", devicebase?.MachineStickerCode ?? "", null, null, null,
                    isSuccess ? OperationResultEnum.CommandIssued : OperationResultEnum.CommandUnexecuted, isSuccess ? null : exception, JsonConvert.SerializeObject(inputObj));

                var method = "IOTSend" + commandDownSend.Method;
                if (commandDownSend.Method == "6216")
                {
                    var info = JsonConvert.DeserializeObject<Iot6216>(commandDownSend.Params);
                    method += $"_{info.CapabilityId}";
                    if (info.CapabilityId == 63)
                    {
                        var infosub = JsonConvert.DeserializeObject<Iot6216_63>(info.Parameters[0]);
                        method += $"_{infosub.Lock.ToString().ToUpper()}";
                    }
                }
                var logInfo = new CreateOperationLogInput(code, devicebase?.MachineStickerCode ?? "", method, requestPath,
                  RequestTypeEnum.Iot, requestMethod, "", _user.UserId, _user.NickName, _user.TenantId, new List<OperationSubLog> { logsubInfo });

                await _publish.SendMessage(CapConst.CreateOperationLog, logInfo);
            }
        }

        /// <summary>
        /// 修改操作日志
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="isSuccess"></param>
        /// <param name="exception"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task UpdateLogActionResult(bool isSuccess, string exception, string code, string mid)
        {
            var input = new UpdateOperationLogInput()
            {
                Code = code,
                Mid = mid,
                OperationResult = isSuccess ? OperationResultEnum.CommandIssued : OperationResultEnum.CommandUnexecuted,
                ErrorMsg = isSuccess ? "" : exception
            };
            await _publish.SendMessage(CapConst.UpdateOperationLog, input);
        }
    }
}
