using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.Commands.ConsumerCommands.ConsumerUserCommands;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.ConsumerDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.ConsumerEntitys;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Exceptions.Extensions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ConsumerUserCommandHandlers
{
    /// <summary>
    /// 登录命令处理器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="jwtTokenService"></param>
    public class ClientUserLoginCommandHandler(CoffeeMachineDbContext context, IJwtTokenService jwtTokenService) : ICommandHandler<ClientUserLoginCommand, ClientLoginResponseDto>
    {
        /// <summary>
        /// 处理登录命令
        /// </summary>
        /// <param name="request">登录命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>登录响应</returns>
        public async Task<ClientLoginResponseDto> Handle(ClientUserLoginCommand request, CancellationToken cancellationToken)
        {
            var clientUserInfo = await context.ClientUser.FirstOrDefaultAsync(w => w.Account == request.account);

            // 如果用户信息未找到，或已禁用，则不允许登录
            if (clientUserInfo == null || clientUserInfo.Enabled == EnabledEnum.Disable)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);

            // 用户信息验证通过，添加用户基本信息到jwt票证
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UId", clientUserInfo.Id.ToString()),
                new Claim("Account", request.account),
                new Claim("NickName", clientUserInfo.NickName.ToString()),
                new Claim("Email", (clientUserInfo.Email).ToString()),
                new Claim("Phone", (clientUserInfo.Phone==null?string.Empty:clientUserInfo.Phone).ToString()),
                new Claim(ClaimConst.Email,clientUserInfo.Email)
            };

            var accessToken = jwtTokenService.GenerateAccessToken(claims, "_Client");
            var refreshToken = jwtTokenService.GenerateRefreshToken(claims, "_Client");

            var clientLoginResponseDto = new ClientLoginResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(1),
                UserName = clientUserInfo.Account,
                NickName = clientUserInfo.NickName,
                Email = clientUserInfo.Email,
                Phone = clientUserInfo.Phone,
                UserId = clientUserInfo.Id,
            };
            return clientLoginResponseDto;
        }
    }

    /// <summary>
    /// 消费者端注册命令处理器
    /// </summary>
    public class ClientUserRegisterCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<ClientUserRegisterCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ClientUserRegisterCommand request, CancellationToken cancellationToken)
        {
            // 检查用户是否存在
            var existingUser = await context.ClientUser.AnyAsync(u => u.Account == request.account || u.Email == request.email || u.Phone == request.phone);
            if (existingUser)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0083)]);

            // 添加用户信息
            var clientUserInfo = new ClientUser(request.account, request.nickName, request.password, request.phone, request.email, request.sex);

            // 持久化用户信息
            var res = await context.AddAsync(clientUserInfo, cancellationToken);

            return res.Entity.Id > 0;
        }
    }

    /// <summary>
    /// 刷新Token
    /// </summary>
    /// <param name="queries"></param>
    /// <param name="context"></param>
    /// <param name="jwtTokenService"></param>
    public class ClientUserRefreshCommandHandler(IApplicationMenuQueries queries, CoffeeMachineDbContext context, IJwtTokenService jwtTokenService) : ICommandHandler<ClientUserRefreshCommand, ClientLoginResponseDto>
    {
        /// <summary>
        /// 刷新Token
        /// </summary>
        public async Task<ClientLoginResponseDto> Handle(ClientUserRefreshCommand request, CancellationToken cancellationToken)
        {
            // 验证 RefreshToken
            var principal = jwtTokenService.ValidateRefreshToken(request.refreshToken);
            if (principal == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0036)]).StatusCode(StatusCodes.Status401Unauthorized);

            // 验证刷新token是否移除(退出登录)
            if (await jwtTokenService.IsTokenRevoked(request.refreshToken))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0037)]).StatusCode(StatusCodes.Status401Unauthorized);

            // 生成新Account Token
            var newAccessToken = jwtTokenService.GenerateAccessToken(principal.Claims, "_Client");

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

            var LoginResponseDto = new ClientLoginResponseDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = request.refreshToken,
                Expires = DateTime.UtcNow.AddDays(30),
                UserName = userInfo.Account,
                NickName = userInfo.NickName,
                Email = userInfo.Email,
                Phone = userInfo.Phone,
                UserId = userInfo.Id,
            };
            return LoginResponseDto;
        }
    }
}