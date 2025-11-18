using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace YS.CoffeeMachine.API.Extensions.Features
{
    /// <summary>
    /// 注入时区偏移属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class InjectTimezoneOffsetAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在动作执行之前调用
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;
            if (headers.TryGetValue("x-tz-offset", out var headerVal) && int.TryParse(headerVal, out var offset))
            {
                foreach (var arg in context.ActionArguments.Values)
                {
                    var prop = arg?.GetType()?.GetProperty("TimezoneOffsetHour", BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && prop.CanWrite)
                        prop.SetValue(arg, offset);
                }
            }

            base.OnActionExecuting(context);
        }
    }
}