using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 平台登录
    /// </summary>
    /// <param name="queries"></param>
    /// <param name="context"></param>
    /// <param name="passwordHasher"></param>
    /// <param name="jwtTokenService"></param>
    public class LoginCommandHandler(CoffeeMachinePlatformDbContext context, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, UserHttpContext _httpContext, IPublishService _cap) : ICommandHandler<LoginCommand, LoginResponseDto>
    {
        /// <summary>
        /// 平台登录
        /// </summary>
        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var userInfo = await context.ApplicationUser.FirstOrDefaultAsync(w => w.Account == request.account && w.SysMenuType == SysMenuTypeEnum.Platform);
            //如果用户信息未找到，或不是商户端用户，则不允许登录
            if (userInfo == null || userInfo.SysMenuType != SysMenuTypeEnum.Platform || userInfo.Status == UserStatusEnum.Disable)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);
            var result = passwordHasher.VerifyHashedPassword(userInfo.Password, request.password);
            if (result == PasswordVerificationResult.Failed)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0030)]);
            //用户信息验证通过，添加用户基本信息到jwt票证
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UId", userInfo.Id.ToString()),
                new Claim("EnterpriseId", userInfo.EnterpriseId.ToString()),
                new Claim("Account", request.account),
                new Claim("NickName", userInfo.NickName.ToString()),
                new Claim("SysMenuType", ((int)userInfo.SysMenuType).ToString()),
                new Claim("AccountType", ((int)userInfo.AccountType).ToString())
            };

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken(claims);
            var LoginResponseDto = new LoginResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(30),
                Username = userInfo.Account,
                NickName = userInfo.NickName,
                PhoneVerification = userInfo.VerifyPhone
            };

            var log = new PlatformOperationLogDto()
            {
                OperationUserId = userInfo.Id,
                OperationUserName = userInfo.NickName,
                TrailType = TrailTypeEnum.Not,
                Describe = "登录",
                Result = true,
                Ip = _httpContext.Ip
            };
            await _cap.SendMessage(CapConst.PlatformOperationLog, log);
            return LoginResponseDto;
        }
    }
}
