using FreeRedis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.ConstantModels;
using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.Dtos.VerificationCodeDtos;
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
    /// 账号密码登录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="passwordHasher"></param>
    /// <param name="jwtTokenService"></param>
    public class LoginCommandHandler(CoffeeMachineDbContext context, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService) : ICommandHandler<LoginCommand, LoginResponseDto>
    {
        /// <summary>
        /// 账号密码登录
        /// </summary>
        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var userInfo = await context.ApplicationUser.Include(i => i.ApplicationUserRoles).ThenInclude(ti => ti.Role).FirstOrDefaultAsync(w => w.Account == request.account && !w.IsDelete);
            // 如果用户信息未找到，或不是商户端用户，则不允许登录
            if (userInfo == null || userInfo.SysMenuType != SysMenuTypeEnum.Merchant || userInfo.Status == UserStatusEnum.Disable)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);
            var result = passwordHasher.VerifyHashedPassword(userInfo.Password, request.password);
            if (result == PasswordVerificationResult.Failed)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0030)]);
            // 当前用户是否是默认用户或拥有超级管理员权限
            var AllDeviceRole = false;
            if (userInfo.ApplicationUserRoles != null)
            {
                var hasSuperAdmin = userInfo.ApplicationUserRoles.Select(s => s.Role).FirstOrDefault(w => w.HasSuperAdmin == true) != null;
                AllDeviceRole = userInfo.IsDefault || hasSuperAdmin;
            }

            // 获取企业信息
            var enterpriseInfo = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == userInfo.EnterpriseId);
            if (enterpriseInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            var areaRelationInfo = await context.AreaRelation.FirstOrDefaultAsync(w => w.Id == enterpriseInfo.AreaRelationId);

            userInfo.LastModifyTime = DateTime.UtcNow;
            context.Update(userInfo);

            // 用户信息验证通过，添加用户基本信息到jwt票证
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UId", userInfo.Id.ToString()),
                new Claim("EnterpriseId", userInfo.EnterpriseId.ToString()),
                new Claim("Account", request.account),
                new Claim("NickName", userInfo.NickName.ToString()),
                new Claim("SysMenuType", ((int)userInfo.SysMenuType).ToString()),
                new Claim("AccountType", ((int)userInfo.AccountType).ToString()),
                new Claim("IsDefault", (userInfo.IsDefault).ToString()),
                new Claim("AllDeviceRole", (AllDeviceRole).ToString()),
                new Claim("TenantName", (enterpriseInfo.Name).ToString()),
                new Claim("Email", (userInfo.Email).ToString()),
                new Claim("Phone", (userInfo.Phone==null?string.Empty:userInfo.Phone).ToString()),
                new Claim(ClaimConst.Email,userInfo.Email)
            };

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken(claims);
            // var menuTree = await queries.GetUserMenuTreeAsync(userInfo.TransId);
            var LoginResponseDto = new LoginResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(1),
                Username = userInfo.Account,
                NickName = userInfo.NickName,
                PhoneVerification = userInfo.VerifyPhone,
                EnterpriseInfoId = userInfo.EnterpriseId,
                RegistrationProgress = enterpriseInfo.RegistrationProgress,
                IsRegister = enterpriseInfo.RegistrationProgress != null,
                Approved = enterpriseInfo.RegistrationProgress == null || enterpriseInfo.RegistrationProgress == RegistrationProgress.Passed,
                TimeZone = areaRelationInfo == null ? null : areaRelationInfo.TimeZone,
                Email = userInfo.Email,
                Phone = userInfo.Phone,
                UserId = userInfo.Id,
                TremServiceId = areaRelationInfo == null ? null : areaRelationInfo.TermServiceUrl == null ? null : long.TryParse(areaRelationInfo.TermServiceUrl, out long res) ? res : null
            };
            return LoginResponseDto;
        }
    }

    /// <summary>
    /// 发送登录短信验证码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    /// <param name="_aliyunSmsService"></param>
    public class SendSMSLoginCodeCommandHandler(CoffeeMachineDbContext context, IRedisClient _redisClient, IAliyunSmsService _aliyunSmsService) : ICommandHandler<SendSMSLoginCodeCommand, SendVerificationCodeResponseDto>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<SendVerificationCodeResponseDto> Handle(SendSMSLoginCodeCommand request, CancellationToken cancellationToken)
        {
            // 验证电话是否存在
            var userInfo = context.ApplicationUser.AsNoTracking().FirstOrDefault(w => w.Phone == request.phone && w.Status == UserStatusEnum.Enable);
            if (userInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);

            // 生成验证码
            var vCode = new Random().Next(100000, 999999).ToString();

            // 缓存验证码，有效期5分钟
            await _redisClient.SetAsync(RedisKeyConstants.GetSmsLoginVCodeKey(request.phone), new EmailVCodeDto() { Code = vCode, Status = 0 }, 5 * 60);

            var res = await _aliyunSmsService.SendSmsAsync(request.phone, SmsConst.LoginVerify, JsonConvert.SerializeObject(new { vcode = vCode }));

            return new SendVerificationCodeResponseDto() { IsSuccess = res.Item1, Message = res.Item2, ExpireTime = 5 };
        }
    }

    /// <summary>
    /// 短信登录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    /// <param name="jwtTokenService"></param>
    public class SmsLoginCommandHandler(CoffeeMachineDbContext context, IRedisClient _redisClient, IJwtTokenService jwtTokenService) : ICommandHandler<SmsLoginCommand, LoginResponseDto>
    {
        /// <summary>
        /// 邮箱登录
        /// </summary>
        public async Task<LoginResponseDto> Handle(SmsLoginCommand request, CancellationToken cancellationToken)
        {
            // 验证电话是否存在
            var userInfo = context.ApplicationUser.OrderByDescending(o => o.LastModifyTime).FirstOrDefault(w => w.Phone == request.phone && w.Status == UserStatusEnum.Enable);
            if (userInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);

            // 核对验证码
            var vCode = await _redisClient.GetAsync<EmailVCodeDto>(RedisKeyConstants.GetSmsLoginVCodeKey(request.phone));
            if (vCode == null || string.IsNullOrEmpty(vCode.Code))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0034)]);
            if (vCode.Code != request.vCode.ToString() || vCode.Status == 1)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0035)]);

            // 当前用户是否是默认用户或拥有超级管理员权限
            var AllDeviceRole = false;
            if (userInfo.ApplicationUserRoles != null)
            {
                var hasSuperAdmin = userInfo.ApplicationUserRoles.Select(s => s.Role).FirstOrDefault(w => w.HasSuperAdmin == true) != null;
                AllDeviceRole = userInfo.IsDefault || hasSuperAdmin;
            }

            // 获取企业信息
            var enterpriseInfo = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == userInfo.EnterpriseId);
            if (enterpriseInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            var areaRelationInfo = await context.AreaRelation.FirstOrDefaultAsync(w => w.Id == enterpriseInfo.AreaRelationId);

            userInfo.LastModifyTime = DateTime.UtcNow;
            context.Update(userInfo);

            // 验证码验证通过，添加用户基本信息到jwt票证
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UId", userInfo.Id.ToString()),
                new Claim("EnterpriseId", userInfo.EnterpriseId.ToString()),
                new Claim("Account", userInfo.Account),
                new Claim("NickName", userInfo.NickName.ToString()),
                new Claim("SysMenuType", ((int)userInfo.SysMenuType).ToString()),
                new Claim("AccountType", ((int)userInfo.AccountType).ToString()),
                new Claim("IsDefault", (userInfo.IsDefault).ToString()),
                new Claim("AllDeviceRole", (AllDeviceRole).ToString()),
                new Claim("TenantName", (enterpriseInfo.Name).ToString()),
                new Claim("Email", (userInfo.Email).ToString()),
                new Claim("Phone", (userInfo.Phone==null?string.Empty:userInfo.Phone).ToString()),
                new Claim(ClaimConst.Email,userInfo.Email)
            };

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken(claims);
            // var menuTree = await queries.GetUserMenuTreeAsync(userInfo.TransId);
            var LoginResponseDto = new LoginResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(1),
                Username = userInfo.Account,
                NickName = userInfo.NickName,
                PhoneVerification = userInfo.VerifyPhone,
                EnterpriseInfoId = userInfo.EnterpriseId,
                RegistrationProgress = enterpriseInfo.RegistrationProgress,
                IsRegister = enterpriseInfo.RegistrationProgress != null,
                Approved = enterpriseInfo.RegistrationProgress == null || enterpriseInfo.RegistrationProgress == RegistrationProgress.Passed,
                TimeZone = areaRelationInfo == null ? null : areaRelationInfo.TimeZone,
                Email = userInfo.Email,
                Phone = userInfo.Phone,
                UserId = userInfo.Id,
                TremServiceId = areaRelationInfo.TermServiceUrl == null ? null : long.TryParse(areaRelationInfo.TermServiceUrl, out long res) ? res : null
            };
            return LoginResponseDto;
        }
    }

    /// <summary>
    /// 发送邮箱登录验证码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    /// <param name="emailServiceProvider"></param>
    public class SendEmailLoginCodeCommandHandler(CoffeeMachineDbContext context, IRedisClient _redisClient, IEmailServiceProvider emailServiceProvider) : ICommandHandler<SendEmailLoginCodeCommand, SendVerificationCodeResponseDto>
    {
        /// <summary>
        /// 发送邮箱登录验证码
        /// </summary>
        public async Task<SendVerificationCodeResponseDto> Handle(SendEmailLoginCodeCommand request, CancellationToken cancellationToken)
        {
            // 验证邮箱是否存在
            var userInfo = context.ApplicationUser.AsNoTracking().FirstOrDefault(w => w.Email == request.email && w.Status == UserStatusEnum.Enable);
            if (userInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0031)]);

            // 生成验证码
            var vCode = new Random().Next(100000, 999999).ToString();

            // 缓存验证码，有效期5分钟
            await _redisClient.SetAsync(RedisKeyConstants.GetEmailLoginVCodeKey(request.email), new EmailVCodeDto() { Code = vCode, Status = 0 }, 5 * 60);

            // 发送验证码
            // TODO:发送验证码多语言配置
            var res = await emailServiceProvider.SendEmailSingleAsync(new EmailObject() { ToEmail = request.email, MessageBody = string.Format(L.Text[nameof(ErrorCodeEnum.D0032)], vCode), Subject = L.Text[nameof(ErrorCodeEnum.D0033)] });

            return new SendVerificationCodeResponseDto() { IsSuccess = res.success, Message = res.response, ExpireTime = 5 };
        }
    }

    /// <summary>
    /// 邮箱登录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    /// <param name="jwtTokenService"></param>
    public class EmailLoginCommandHandler(CoffeeMachineDbContext context, IRedisClient _redisClient, IJwtTokenService jwtTokenService) : ICommandHandler<EmailLoginCommand, LoginResponseDto>
    {
        /// <summary>
        /// 邮箱登录
        /// </summary>
        public async Task<LoginResponseDto> Handle(EmailLoginCommand request, CancellationToken cancellationToken)
        {
            // 验证邮箱是否存在
            var userInfo = context.ApplicationUser.OrderByDescending(o => o.LastModifyTime).FirstOrDefault(w => w.Email == request.email && w.Status == UserStatusEnum.Enable);
            if (userInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0031)]);

            // 核对验证码
            var vCode = await _redisClient.GetAsync<EmailVCodeDto>(RedisKeyConstants.GetEmailLoginVCodeKey(request.email));
            if (vCode == null || string.IsNullOrEmpty(vCode.Code))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0034)]);
            if (vCode.Code != request.eCode.ToString() || vCode.Status == 1)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0035)]);

            // 当前用户是否是默认用户或拥有超级管理员权限
            var AllDeviceRole = false;
            if (userInfo.ApplicationUserRoles != null)
            {
                var hasSuperAdmin = userInfo.ApplicationUserRoles.Select(s => s.Role).FirstOrDefault(w => w.HasSuperAdmin == true) != null;
                AllDeviceRole = userInfo.IsDefault || hasSuperAdmin;
            }

            // 获取企业信息
            var enterpriseInfo = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == userInfo.EnterpriseId);
            if (enterpriseInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            var areaRelationInfo = await context.AreaRelation.FirstOrDefaultAsync(w => w.Id == enterpriseInfo.AreaRelationId);

            userInfo.LastModifyTime = DateTime.UtcNow;
            context.Update(userInfo);

            // 验证码验证通过，添加用户基本信息到jwt票证
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UId", userInfo.Id.ToString()),
                new Claim("EnterpriseId", userInfo.EnterpriseId.ToString()),
                new Claim("Account", userInfo.Account),
                new Claim("NickName", userInfo.NickName.ToString()),
                new Claim("SysMenuType", ((int)userInfo.SysMenuType).ToString()),
                new Claim("AccountType", ((int)userInfo.AccountType).ToString()),
                new Claim("IsDefault", (userInfo.IsDefault).ToString()),
                new Claim("AllDeviceRole", (AllDeviceRole).ToString()),
                new Claim("TenantName", (enterpriseInfo.Name).ToString()),
                new Claim("Email", (userInfo.Email).ToString()),
                new Claim("Phone", (userInfo.Phone==null?string.Empty:userInfo.Phone).ToString()),
                new Claim(ClaimConst.Email,userInfo.Email)
            };

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken(claims);
            // var menuTree = await queries.GetUserMenuTreeAsync(userInfo.TransId);
            var LoginResponseDto = new LoginResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(1),
                Username = userInfo.Account,
                NickName = userInfo.NickName,
                PhoneVerification = userInfo.VerifyPhone,
                EnterpriseInfoId = userInfo.EnterpriseId,
                RegistrationProgress = enterpriseInfo.RegistrationProgress,
                IsRegister = enterpriseInfo.RegistrationProgress != null,
                Approved = enterpriseInfo.RegistrationProgress == null || enterpriseInfo.RegistrationProgress == RegistrationProgress.Passed,
                TimeZone = areaRelationInfo == null ? null : areaRelationInfo.TimeZone,
                Email = userInfo.Email,
                Phone = userInfo.Phone,
                UserId = userInfo.Id,
                TremServiceId = areaRelationInfo.TermServiceUrl == null ? null : long.TryParse(areaRelationInfo.TermServiceUrl, out long res) ? res : null
            };
            return LoginResponseDto;
        }
    }

    /// <summary>
    /// 切换账号
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    /// <param name="jwtTokenService"></param>
    /// <param name="_user"></param>
    public class ChangeAccountBuUserIdCommandHandler(CoffeeMachineDbContext context, IRedisClient _redisClient, IJwtTokenService jwtTokenService, UserHttpContext _user) : ICommandHandler<ChangeAccountBuUserIdCommand, LoginResponseDto>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<LoginResponseDto> Handle(ChangeAccountBuUserIdCommand request, CancellationToken cancellationToken)
        {
            var userInfo = await context.ApplicationUser.Include(i => i.ApplicationUserRoles).ThenInclude(ti => ti.Role).FirstOrDefaultAsync(w =>
            w.Id == request.id && (w.Email == _user.Email
            || (w.Phone == _user.Phone && !string.IsNullOrEmpty(w.Phone)))
            && !w.IsDelete && w.Status == UserStatusEnum.Enable);

            if (userInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);

            // 当前用户是否是默认用户或拥有超级管理员权限
            var AllDeviceRole = false;
            if (userInfo.ApplicationUserRoles != null)
            {
                var hasSuperAdmin = userInfo.ApplicationUserRoles.Select(s => s.Role).FirstOrDefault(w => w.HasSuperAdmin == true) != null;
                AllDeviceRole = userInfo.IsDefault || hasSuperAdmin;
            }

            // 获取企业信息
            var enterpriseInfo = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == userInfo.EnterpriseId);
            if (enterpriseInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            var areaRelationInfo = await context.AreaRelation.FirstOrDefaultAsync(w => w.Id == enterpriseInfo.AreaRelationId);

            userInfo.LastModifyTime = DateTime.UtcNow;
            context.Update(userInfo);

            // 验证码验证通过，添加用户基本信息到jwt票证
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UId", userInfo.Id.ToString()),
                new Claim("EnterpriseId", userInfo.EnterpriseId.ToString()),
                new Claim("Account", userInfo.Account),
                new Claim("NickName", userInfo.NickName.ToString()),
                new Claim("SysMenuType", ((int)userInfo.SysMenuType).ToString()),
                new Claim("AccountType", ((int)userInfo.AccountType).ToString()),
                new Claim("IsDefault", (userInfo.IsDefault).ToString()),
                new Claim("AllDeviceRole", (AllDeviceRole).ToString()),
                new Claim("TenantName", (enterpriseInfo.Name).ToString()),
                new Claim("Email", (userInfo.Email).ToString()),
                new Claim("Phone", (userInfo.Phone==null?string.Empty:userInfo.Phone).ToString()),
                new Claim(ClaimConst.Email,userInfo.Email)
            };

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken(claims);
            var LoginResponseDto = new LoginResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(1),
                Username = userInfo.Account,
                NickName = userInfo.NickName,
                PhoneVerification = userInfo.VerifyPhone,
                EnterpriseInfoId = userInfo.EnterpriseId,
                RegistrationProgress = enterpriseInfo.RegistrationProgress,
                IsRegister = enterpriseInfo.RegistrationProgress != null,
                Approved = enterpriseInfo.RegistrationProgress == null || enterpriseInfo.RegistrationProgress == RegistrationProgress.Passed,
                TimeZone = areaRelationInfo == null ? null : areaRelationInfo.TimeZone,
                Email = userInfo.Email,
                Phone = userInfo.Phone,
                UserId = userInfo.Id,
                TremServiceId = areaRelationInfo.TermServiceUrl == null ? null : long.TryParse(areaRelationInfo.TermServiceUrl, out long res) ? res : null
            };
            return LoginResponseDto;
        }
    }
}