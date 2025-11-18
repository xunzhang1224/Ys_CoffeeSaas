using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 切换企业
    /// </summary>
    public class ChangeEnterpriseByIdCommandHandler(CoffeeMachineDbContext context, IJwtTokenService jwtTokenService, UserHttpContext _user) : ICommandHandler<ChangeEnterpriseByIdCommand, LoginResponseDto>
    {
        /// <summary>
        /// 切换企业
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<LoginResponseDto> Handle(ChangeEnterpriseByIdCommand request, CancellationToken cancellationToken)
        {
            var info = await context.EnterpriseInfo.AsQueryable().Where(a => a.Id == request.enterpriseId).FirstOrDefaultAsync();
            if (info == null)
            {
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0068)]);
            }

            var user = await context.ApplicationUser.AsQueryable().Where(a => a.Id == _user.UserId).FirstOrDefaultAsync();
            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UId", _user.UserId.ToString()),
                new Claim("EnterpriseId", request.enterpriseId.ToString()),
                new Claim("Account", _user.Account),
                new Claim("NickName", _user.NickName.ToString()),
                new Claim("SysMenuType", _user.SysMenuType),
                new Claim("AccountType", ((int)user.AccountType).ToString()),
                new Claim("IsDefault", (_user.IsDefault).ToString()),
                new Claim("AllDeviceRole", (_user.AllDeviceRole).ToString()),
                new Claim("TenantName", info.Name),
                new Claim(ClaimConst.Email,_user.Email)
            };

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken(claims);
            //var menuTree = await queries.GetUserMenuTreeAsync(userInfo.TransId);
            var LoginResponseDto = new LoginResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(1),
                Username = _user.Account,
                NickName = _user.NickName,
                PhoneVerification = true,
                RegistrationProgress = info.RegistrationProgress,
                IsRegister = info.RegistrationProgress != null,
                Approved = info.RegistrationProgress == null || info.RegistrationProgress == RegistrationProgress.Passed,
                UserId = _user.UserId,
            };
            return LoginResponseDto;
        }
    }
}
