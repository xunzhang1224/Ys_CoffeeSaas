using YS.CoffeeMachine.API.Services.TimezoneService;

namespace YS.CoffeeMachine.API.Extensions.Middleware
{
    /// <summary>
    /// 时区中间件（设置时区偏移量）
    /// </summary>
    public class TimezoneMiddleware(RequestDelegate _next, ILogger<TimezoneMiddleware> _logger)
    {
        /// <summary>
        /// 执行中间件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var header = context.Request.Headers["x-tz-offset"].FirstOrDefault();
            if (int.TryParse(header, out var offset))
            {
                TimezoneContext.SetOffset(offset);
            }
            else
            {
                TimezoneContext.SetOffset(null);
            }

            await _next(context);
        }
    }
}