using YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Platform.API.Extensions.Middleware
{
    /// <summary>
    /// 权限过滤中间件
    /// </summary>
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        /// <summary>
        /// 初始构造
        /// </summary>
        /// <param name="next"></param>
        /// <param name="scopeFactory"></param>
        public PermissionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                // 从 Claims 中获取用户Id
                var uid = user.Claims.FirstOrDefault(c => c.Type == "UId")?.Value;
                if (uid == null)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }
                // 从 Claims 中获取账号类型
                var sysMenuType = user.Claims.FirstOrDefault(c => c.Type == "SysMenuType")?.Value;
                //账号类型为平台账号，禁止访问前端接口
                if (sysMenuType == ((int)SysMenuTypeEnum.Platform).ToString())
                {
                    if (context.Request.Path.ToString().Contains("/api"))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }
                }
                //账号类型为前端账号，禁止访问平台接口
                else
                {
                    if (context.Request.Path.ToString().Contains("/papi"))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }
                }

                //超级管理员直接通过验证
                var accountType = user.Claims.FirstOrDefault(c => c.Type == "AccountType")?.Value;
                if (accountType == ((int)AccountTypeEnum.SuperAdmin).ToString())
                {
                    await _next(context);
                    return;
                }

                // 根据标识集合验证权限
                // 路由名称
                var routeName = context.Request.Path.StartsWithSegments("/api")
                    ? context.Request.Path.Value[5..].Replace("/", ":")
                    : context.Request.Path.Value[6..].Replace("/", ":");
                // 使用 IServiceScopeFactory 创建作用域
                using (var scope = _scopeFactory.CreateScope())
                {
                    var menuQueries = scope.ServiceProvider.GetRequiredService<IP_ApplicationMenuQueries>();
                    var hasPermission = await CheckPermission(Convert.ToInt64(uid), routeName, menuQueries);
                    if (!hasPermission)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }
                }
            }

            await _next(context);
        }

        private async Task<bool> CheckPermission(long uid, string routeName, IP_ApplicationMenuQueries menuQueries)
        {
            // 获取当前用户拥有的标识集合
            var ownBtnPermList = await menuQueries.GetOwnBtnPermList(uid);
            if (ownBtnPermList.Exists(u => routeName.Equals(u, StringComparison.CurrentCultureIgnoreCase)))
                return true;
            // 获取所有权限标识集合,如果当前路由未配置权限标识则默认通过验证
            var allBtnPermList = await menuQueries.GetAllBtnPermList();
            return allBtnPermList.TrueForAll(u => !routeName.Equals(u, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
