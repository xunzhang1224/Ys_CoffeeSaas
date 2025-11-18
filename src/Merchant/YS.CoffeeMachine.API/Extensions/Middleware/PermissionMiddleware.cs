using FreeRedis;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.API.Extensions.Middleware
{
    /// <summary>
    /// 权限过滤中间件
    /// </summary>
    /// <param name="_next"></param
    /// <param name="menuQueries"></param>
    public class PermissionMiddleware
    {

        /// <summary>
        /// 定义权限映射
        /// </summary>
        private static readonly Dictionary<string, string[]> MenuTypePermissions = new Dictionary<string, string[]>()
        {
            { ((int)SysMenuTypeEnum.Merchant).ToString(), new[] { "/api" } },
            { ((int)SysMenuTypeEnum.H5).ToString(), new[] { "/api" } },
            { ((int)SysMenuTypeEnum.Platform).ToString(), new[] { "/papi" } },
            { ((int)SysMenuTypeEnum.Consumer).ToString(), new[] { "/capi" } }
        };

        private static string GetLogOutTokenKey(string userId) => $"/ApplicationUsers/LogOut/{userId}";

        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        IRedisClient _redis;
        /// <summary>
        /// 初始构造
        /// </summary>
        /// <param name="next"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="redis"></param>
        public PermissionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory, IRedisClient redis)
        {
            _next = next;
            _scopeFactory = scopeFactory;
            _redis = redis;
        }

        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // token黑名单验证
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader == null || !authorizationHeader.StartsWith("Bearer "))
            {
                // 如果没有 Authorization 头部信息，直接跳过鉴权
                await _next(context);
                return;
            }

            // 提取 token
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            // Token 过期检查
            //if (await IsTokenExpiredAsync(token))
            //{
            //    // Token 过期，拒绝请求
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await context.Response.WriteAsync("Unauthorized: Token is expired.");
            //    return;
            //}

            // 检查 token 是否在 Redis 黑名单中
            //var isTokenBlacklisted = await _redis.GetAsync<bool>(GetLogOutTokenKey(token));
            //if (isTokenBlacklisted)
            //{
            //    // 如果 Token 已经被黑名单中，表示该 Token 已失效，拒绝请求
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await context.Response.WriteAsync("Unauthorized: Token is blacklisted.");
            //    return;
            //}

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

                // 从 Claims 中获取账号类型并验证访问路径权限
                var sysMenuType = user.Claims.FirstOrDefault(c => c.Type == "SysMenuType")?.Value;
                if (sysMenuType == null || !MenuTypePermissions.ContainsKey(sysMenuType))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }

                // 验证请求路径是否在允许的路径列表中
                var allowedPaths = MenuTypePermissions[sysMenuType];
                var requestPath = context.Request.Path.ToString();

                // 检查请求路径是否包含在允许的路径中
                if (!allowedPaths.Any(path => requestPath.Contains(path)))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }

                //// 从 Claims 中获取账号类型
                //var sysMenuType = user.Claims.FirstOrDefault(c => c.Type == "SysMenuType")?.Value;
                //if (sysMenuType == null)
                //{
                //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //    await context.Response.WriteAsync("Forbidden");
                //    return;
                //}

                //// 账号类型不为为商户端账号，禁止访问商户端接口
                //if (sysMenuType != ((int)SysMenuTypeEnum.Merchant).ToString() && sysMenuType != ((int)SysMenuTypeEnum.H5).ToString())
                //{
                //    if (context.Request.Path.ToString().Contains("/api"))
                //    {
                //        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //        await context.Response.WriteAsync("Forbidden");
                //        return;
                //    }
                //}
                //// 账号类型不为平台账号，禁止访问平台接口
                //else if (sysMenuType != ((int)SysMenuTypeEnum.Platform).ToString())
                //{
                //    if (context.Request.Path.ToString().Contains("/papi"))
                //    {
                //        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //        await context.Response.WriteAsync("Forbidden");
                //        return;
                //    }
                //}
                //// 账号类型不为消费者账号，禁止访问消费者接口
                //else if (sysMenuType != ((int)SysMenuTypeEnum.Consumer).ToString())
                //{
                //    if (context.Request.Path.ToString().Contains("/capi"))
                //    {
                //        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //        await context.Response.WriteAsync("Forbidden");
                //        return;
                //    }
                //}

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
                    var menuQueries = scope.ServiceProvider.GetRequiredService<IApplicationMenuQueries>();
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

        private async Task<bool> CheckPermission(long uid, string routeName, IApplicationMenuQueries menuQueries)
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
