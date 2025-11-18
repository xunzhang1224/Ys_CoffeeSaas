using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.IServices
{
    /// <summary>
    /// IJwtTokenService
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// 生成 Access Token
        /// </summary>
        /// <param name="claims">包含的用户声明</param>
        /// <returns>生成的 Access Token</returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// 生成 Refresh Token
        /// </summary>
        /// <returns>生成的 Refresh Token</returns>
        string GenerateRefreshToken(IEnumerable<Claim> claims);

        /// <summary>
        /// 从过期的 Token 提取用户声明信息
        /// </summary>
        /// <param name="token">过期的 Token</param>
        /// <returns>用户声明信息</returns>
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

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
