using System.Security.Claims;

namespace YS.CoffeeMachine.Application.IServices
{
    /// <summary>
    /// JWT Token 服务接口，用于生成和验证 Token
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// 生成 Access Token
        /// </summary>
        /// <param name="claims">包含的用户声明</param>
        /// <param name="suffix">参数配置名后缀</param>
        /// <returns>生成的 Access Token</returns>
        string GenerateAccessToken(IEnumerable<Claim> claims, string? suffix = null);

        /// <summary>
        /// 生成 Refresh Token
        /// </summary>
        /// <returns>生成的 Refresh Token</returns>
        string GenerateRefreshToken(IEnumerable<Claim> claims, string? suffix = null);

        /// <summary>
        /// 从过期的 Token 提取用户声明信息
        /// </summary>
        /// <param name="token">过期的 Token</param>
        /// <param name="suffix">参数配置名后缀</param>
        /// <returns>用户声明信息</returns>
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, string? suffix = null);

        /// <summary>
        /// 验证 RefreshToken 是否有效
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        ClaimsPrincipal? ValidateRefreshToken(string refreshToken);

        /// <summary>
        /// 验证token是否撤销
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public Task<bool> IsTokenRevoked(string refreshToken);
    }
}