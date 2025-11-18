using FreeRedis;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YS.CoffeeMachine.Application.IServices;

namespace YS.CoffeeMachine.API.Application.Services
{
    /// <summary>
    /// JWT服务类
    /// </summary>
    public class JwtTokenService(IConfiguration _configuration, TokenValidationParameters _tokenValidationParameters, IRedisClient redis) : IJwtTokenService
    {
        /// <summary>
        /// 生成 Access Token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string GenerateAccessToken(IEnumerable<Claim> claims, string? suffix = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key" + suffix]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer" + suffix],
                audience: _configuration["Jwt:Audience" + suffix],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpiresMinutes" + suffix])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 生成 Refresh Token
        /// </summary>
        /// <returns></returns>
        public string GenerateRefreshToken(IEnumerable<Claim> claims, string? suffix = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key" + suffix]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer" + suffix],
                audience: _configuration["Jwt:Audience" + suffix],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpiresDays" + suffix])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 从过期的 Token 提取用户声明信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, string? suffix = null)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key" + suffix])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        /// <summary>
        /// 验证 RefreshToken 是否有效
        /// </summary>
        public ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                // 验证 Token
                var principal = tokenHandler.ValidateToken(refreshToken, _tokenValidationParameters, out var validatedToken);

                // 检查是否是 JWT 格式的 Token
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                // 检查 Token 是否已过期
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    throw new SecurityTokenException("Token has expired");
                }

                // 返回 Principal
                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 验证token是否撤销
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<bool> IsTokenRevoked(string refreshToken)
        {
            var revokedInfo = await redis.GetAsync(refreshToken);
            return !string.IsNullOrWhiteSpace(revokedInfo);
        }
    }
}