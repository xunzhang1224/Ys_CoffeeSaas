using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Services;
using YSCore.Base.Localization;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Platform.API.Controllers.Aop
{
    /// <summary>
    /// DeviceDownSeedActionFilter
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <param name="_configuration"></param>
    public class DeviceDownSeedActionFilter(IPublishService _publish, UserHttpContext _user, IotBaseService _iotBaseService) : IAsyncActionFilter
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
            // 是否重试
            bool isRetry = false;
            try
            {
                // 从表单数据中获取
                if (context.ActionArguments.TryGetValue("input", out var inputObj) &&
                    inputObj is CommandDownSend commandDownSend)
                {
                    mid = commandDownSend.Mid;
                    if (string.IsNullOrWhiteSpace(commandDownSend.TransId))
                    {
                        code = YitIdHelper.NextId().ToString();
                        commandDownSend.TransId = code;
                    }
                    else
                    {
                        isRetry = true;
                        code = commandDownSend.TransId;
                        mid = await _iotBaseService.GetMidAsync(mid);
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

                // 可以选择重新抛出异常，让全局异常处理器处理
                throw;
            }
            finally
            {
                if (!isRetry)
                    await LogActionResult(context, isSuccess, actionException, code);
                else
                    await UpdateLogActionResult(isSuccess, actionException, code, mid);
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
                var device = await _iotBaseService.GetDeviceInfoAsync(commandDownSend.Mid);
                var model = await _iotBaseService.GetDeviceModelInfoAsync(devicebase.DeviceModelId ?? 0);
                var requestPath = context.HttpContext.Request.Path; // 请求路径
                var requestMethod = context.HttpContext.Request.Method; // HTTP方法
                var logsubInfo = new OperationSubLog(model?.Name ?? "", device?.Name ?? " ", devicebase?.MachineStickerCode ?? "", null, null, null,
                    isSuccess ? OperationResultEnum.CommandIssued : OperationResultEnum.CommandUnexecuted, isSuccess ? null : exception, JsonConvert.SerializeObject(inputObj));
                var logInfo = new CreateOperationLogInput(code, devicebase?.MachineStickerCode ?? "", "IOTSend" + commandDownSend.Method, requestPath,
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

    ///// <summary>
    ///// OnActionExecutionAsync
    ///// </summary>
    ///// <param name="context"></param>
    ///// <param name="next"></param>
    ///// <returns></returns>
    ///// <exception cref="ArgumentException"></exception>
    //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //{
    //    // 从表单数据中获取
    //    if (context.RouteData.Values.TryGetValue("mid", out var midValue))
    //    {
    //        string mid = midValue?.ToString();
    //        if (string.IsNullOrWhiteSpace(mid))
    //            throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0016)]);
    //        var url = _configuration["NotityUrl:Url"] + "/api/Iot/IsOnline?mid={0}";
    //        var client = httpClientFactory.CreateClient();
    //        //client.Timeout = TimeSpan.FromSeconds(2);
    //        var content = await client.GetStringAsync(string.Format(url, mid));
    //        var respone = JsonConvert.DeserializeObject<bool>(content);
    //        if (!respone)
    //            throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0015)]);
    //    }
    //    await next();
    //}
}
