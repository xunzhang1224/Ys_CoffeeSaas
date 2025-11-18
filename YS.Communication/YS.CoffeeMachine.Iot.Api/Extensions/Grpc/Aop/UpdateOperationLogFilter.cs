using MagicOnion.Server;
using System.Text.Json;
using YS.CoffeeMachine.Iot.Api.Services;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

namespace YS.CoffeeMachine.Iot.Api.Extensions.Grpc.Aop
{
    /// <summary>
    /// UpdateOperationLogFilter
    /// </summary>
    public class UpdateOperationLogFilter : MagicOnionFilterAttribute
    {
        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async ValueTask Invoke(ServiceContext context, Func<ServiceContext, ValueTask> next)
        {
            try
            {
                // 从 MagicOnion 的 DI 中获取服务
                var deviceService = context.ServiceProvider.GetRequiredService<DeviceService>();
                var rawRequest = context.GetRawRequest();
                string? jsonString = rawRequest switch
                {
                    string s => s,
                    byte[] bytes => System.Text.Encoding.UTF8.GetString(bytes),
                    _ => JsonSerializer.Serialize(rawRequest) // fallback
                };
                BaseTransCmd? parameters = JsonSerializer.Deserialize<BaseTransCmd>(
               jsonString!,
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (parameters != null && !string.IsNullOrWhiteSpace(parameters.TransId))
                {
                    bool status = true;
                    var msg = string.Empty;
                   // // 需要修改操作日志的设备回复
                   // DownlinkEntity6216.Response? response =
                   //JsonSerializer.Deserialize<DownlinkEntity6216.Response>(jsonString!);
                   // if (response != null && response.Status == 2)
                   // {
                   //     status = false;
                   //     msg = response.Description;
                   // }
                   // // 需要修改操作日志的设备回复
                   // DownlinkEntity1203.Response? response1 =
                   //JsonSerializer.Deserialize<DownlinkEntity1203.Response>(jsonString!);
                   // if (response1 != null && response1.Status == 3)
                   // {
                   //     status = false;
                   //     msg = response.Description;
                   // }
                    await deviceService.UpdateLogActionResult(status, msg, parameters.TransId, parameters.Mid);
                }
                // 调用原方法
                await next(context);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
