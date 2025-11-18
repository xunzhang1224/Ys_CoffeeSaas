using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Exceptions.Extensions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 刷新Token
    /// </summary>
    /// <param name="queries"></param>
    /// <param name="context"></param>
    /// <param name="jwtTokenService"></param>
    public class RefreshCommandHandler(IApplicationMenuQueries queries, CoffeeMachineDbContext context, IJwtTokenService jwtTokenService) : ICommandHandler<RefreshCommand, LoginResponseDto>
    {
        /// <summary>
        /// 刷新Token
        /// </summary>
        public async Task<LoginResponseDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            // 验证 RefreshToken
            var principal = jwtTokenService.ValidateRefreshToken(request.refreshToken);
            if (principal == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0036)]).StatusCode(StatusCodes.Status401Unauthorized);

            // 验证刷新token是否移除(退出登录)
            if (await jwtTokenService.IsTokenRevoked(request.refreshToken))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0037)]).StatusCode(StatusCodes.Status401Unauthorized);

            // 生成新Account Token
            var newAccessToken = jwtTokenService.GenerateAccessToken(principal.Claims);
            //刷新account Token时，Refresh Toeken不更新
            //var newRefreshToken = jwtTokenService.GenerateRefreshToken(principal.Claims);
            var uid = principal.Claims.FirstOrDefault(c => c.Type == "UId")?.Value;
            if (uid == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0038)]).StatusCode(StatusCodes.Status401Unauthorized);
            var userInfo = await context.ApplicationUser.AsNoTracking().FirstOrDefaultAsync(w => w.Id == Convert.ToInt64(uid));
            if (userInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]).StatusCode(StatusCodes.Status401Unauthorized);

            //获取企业信息
            var enterpriseInfo = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == userInfo.EnterpriseId);
            if (enterpriseInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            //var menuTree = await queries.GetUserMenuTreeAsync(userInfo.TransId);
            var LoginResponseDto = new LoginResponseDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = request.refreshToken,
                Expires = DateTime.UtcNow.AddDays(30),
                Username = userInfo.Account,
                NickName = userInfo.NickName,
                PhoneVerification = userInfo.VerifyPhone,
                RegistrationProgress = enterpriseInfo.RegistrationProgress,
                IsRegister = enterpriseInfo.RegistrationProgress != null,
                Approved = enterpriseInfo.RegistrationProgress == null || enterpriseInfo.RegistrationProgress == RegistrationProgress.Passed,
                Email = userInfo.Email,
                UserId = userInfo.Id,
            };
            return LoginResponseDto;
        }
    }
}
