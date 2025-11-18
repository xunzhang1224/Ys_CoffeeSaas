using MagicOnion.Server;
using System.Diagnostics;
using YS.CoffeeMachine.Iot.Api.Services;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.CoffeeMachine.Iot.Api.Extensions.Grpc.Aop
{
    /// <summary>
    /// HeartbeatFilter
    /// </summary>
    public class HeartbeatFilter : MagicOnionFilterAttribute
    {
        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async ValueTask Invoke(ServiceContext context, Func<ServiceContext, ValueTask> next)
        {
            if (context.MethodName.EndsWith("Uplink1000HandleAsync")|| context.MethodName.EndsWith("GetDeviceInitByMidAsync"))
            {
                await next(context);
                return;
            }
            try
            {
                // 从 MagicOnion 的 DI 中获取服务
                var deviceService = context.ServiceProvider.GetRequiredService<DeviceService>();
                var parameters = context.GetRawRequest() as BaseCmd;
                // 调用方法
                await deviceService.SetDeviceOnline(parameters.Mid);

                // 调用原方法
                await next(context);
            }
            catch (Exception ex)
            {
                // 你可以选择记录日志或处理异常
                throw;
            }
        }
    }
}
