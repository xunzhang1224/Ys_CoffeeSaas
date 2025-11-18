using System.Net;

namespace YS.CoffeeMachine.API.Services
{
    /// <summary>
    /// IP 地址帮助类
    /// </summary>
    public static class IpAddressHelper
    {
        /// <summary>
        /// 获取客户端真实IP（带多代理处理、IPv6转换、扩展方法）
        /// </summary>
        public static string GetClientIp(HttpContext httpContext, bool useForwarded = true)
        {
            if (httpContext == null) return "unknown";

            string ip = null;

            // Step 1: 检查 Cloudflare IP 头（优先）
            ip = httpContext.Request.Headers["CF-Connecting-IP"].FirstOrDefault();

            // Step 2: 读取 X-Forwarded-For（代理链），多个用逗号分隔
            if (useForwarded && string.IsNullOrWhiteSpace(ip))
            {
                var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(forwardedFor))
                {
                    ip = forwardedFor.Split(',').Select(p => p.Trim()).FirstOrDefault();
                }
            }

            // Step 3: 检查 X-Real-IP（部分代理使用）
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            }

            // Step 4: 回退到 RemoteIpAddress
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = httpContext.Connection?.RemoteIpAddress?.ToString();
            }

            // Step 5: 标准化（IPv6 转 IPv4）
            return NormalizeIp(ip);
        }

        /// <summary>
        /// IHttpContextAccessor 扩展方法，获取客户端真实 IP
        /// </summary>
        public static string GetClientIp(this IHttpContextAccessor accessor, bool useForwarded = true)
        {
            return GetClientIp(accessor?.HttpContext, useForwarded);
        }

        /// <summary>
        /// 标准化 IP，处理 ::ffff: 表示 IPv4 in IPv6
        /// </summary>
        private static string NormalizeIp(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip)) return "unknown";

            if (IPAddress.TryParse(ip, out var address))
            {
                if (address.IsIPv4MappedToIPv6)
                {
                    return address.MapToIPv4().ToString();
                }
                return address.ToString();
            }

            return ip;
        }
    }
}