using Microsoft.AspNetCore.Mvc.Filters;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Services;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Controllers.Aop
{
    /// <summary>
    /// 设备是否在线aop
    /// </summary>
    /// <param name="_iotBaseService"></param>
    public class IsOnlineActionFilter(IotBaseService _iotBaseService) : IAsyncActionFilter
    {
        /// <summary>
        /// OnActionExecutionAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 从表单数据中获取
            if (context.RouteData.Values.TryGetValue("mid", out var midValue))
            {
                string mid = midValue?.ToString();
                if (string.IsNullOrWhiteSpace(mid))
                    throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0016)]);
                var isOnline = await _iotBaseService.IsOnline(mid);
                if (!isOnline)
                    throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0015)]);
            }
            await next();
        }
    }
}
