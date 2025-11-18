using FreeRedis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Services;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 企业注册邮件验证码发送命令处理器
    /// </summary>
    /// <param name="emailService"></param>
    /// <param name="redisClient"></param>
    /// <param name="httpContextAccessor"></param>
    public class SendVcodeCommandHandler(IEmailServiceProvider emailService, IRedisClient redisClient, IHttpContextAccessor httpContextAccessor) : ICommandHandler<SendVcodeCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(SendVcodeCommand request, CancellationToken cancellationToken)
        {
            // 验证邮箱不能为空
            if (string.IsNullOrWhiteSpace(request.email))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0092)]);

            // 邮箱格式正则验证
            if (string.IsNullOrWhiteSpace(request.email) || !System.Text.RegularExpressions.Regex.IsMatch(request.email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0093)]);

            #region Ip限流

            // 限制每个IP每分钟最多2次请求
            var ip = IpAddressHelper.GetClientIp(httpContextAccessor.HttpContext);/* httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";*/
            // 未获取到IP，后续可以做异常提示，这里暂时不判断
            if (ip != "unknown")
            {
                var ipKey = $"emailverifylimit:ip:{ip}";

                var count = await redisClient.IncrByAsync(ipKey, 1); // 自增
                if (count == 1)
                {
                    await redisClient.ExpireAsync(ipKey, 60); // 第一次时设置过期时间
                }
                if (count > 2)
                {
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0094)]);
                }
            }
            #endregion

            // 生成验证码
            var vcode = new Random().Next(100000, 999999).ToString();

            // 缓存验证码
            var key = string.Format(CacheConst.RegisterEnterpriseEMailVCodeKey, request.email);
            var res = await redisClient.SetNxAsync(key, vcode, TimeSpan.FromMinutes(5));
            if (!res)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0095)]);

            // 组装邮件对象
            var email = new EmailObject
            {
                ToEmail = request.email,
                Subject = L.Text[nameof(ErrorCodeEnum.D0096)],
                MessageBody = string.Format(L.Text[nameof(ErrorCodeEnum.D0097)], vcode)
            };

            // 发送邮件
            var (success, response) = await emailService.SendEmailSingleAsync(email);
            return success;
        }
    }

    /// <summary>
    /// 企业注册发送短信验证码命令处理器
    /// </summary>
    /// <param name="aliyunSmsService"></param>
    /// <param name="redisClient"></param>
    /// <param name="httpContextAccessor"></param>
    public class SendSMSVcodeCommandHandler(IAliyunSmsService aliyunSmsService, IRedisClient redisClient, IHttpContextAccessor httpContextAccessor) : ICommandHandler<SendSMSVcodeCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(SendSMSVcodeCommand request, CancellationToken cancellationToken)
        {
            // 验证邮箱不能为空
            if (string.IsNullOrWhiteSpace(request.phone))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0105)]);

            // 邮箱格式正则验证
            if (string.IsNullOrWhiteSpace(request.phone) || !System.Text.RegularExpressions.Regex.IsMatch(request.phone, @"^1[3-9]\d{9}$"))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0106)]);

            #region Ip限流

            // 限制每个IP每分钟最多2次请求
            var ip = IpAddressHelper.GetClientIp(httpContextAccessor.HttpContext);/* httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";*/
            // 未获取到IP，后续可以做异常提示，这里暂时不判断
            if (ip != "unknown")
            {
                var ipKey = $"emailverifylimit:ip:{ip}";

                var count = await redisClient.IncrByAsync(ipKey, 1); // 自增
                if (count == 1)
                {
                    await redisClient.ExpireAsync(ipKey, 60); // 第一次时设置过期时间
                }
                if (count > 2)
                {
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0094)]);
                }
            }
            #endregion

            // 生成验证码
            var vcode = new Random().Next(100000, 999999).ToString();

            // 缓存验证码
            var key = string.Format(CacheConst.RegisterEnterpriseSMSVCodeKey, request.phone);
            var res = await redisClient.SetNxAsync(key, vcode, TimeSpan.FromMinutes(5));
            if (!res)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0095)]);

            // 组装短信对象
            string templateParamJson = JsonConvert.SerializeObject(new
            {
                vcode = vcode
            });

            // 发送验证码
            await aliyunSmsService.SendSmsAsync(request.phone, SmsConst.RegisterOperator, templateParamJson);
            return true;
        }
    }

    /// <summary>
    /// 企业注册命令处理器
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="redisClient"></param>
    /// <param name="passwordHasher"></param>
    /// <param name="jwtTokenService"></param>
    public class RegisterEnterpriseCommandHandler(CoffeeMachineDbContext dbContext, IRedisClient redisClient, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService) : ICommandHandler<RegisterEnterpriseCommand, LoginResponseDto>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<LoginResponseDto> Handle(RegisterEnterpriseCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.input.EmailVCode) && string.IsNullOrWhiteSpace(request.input.SmsVCode))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            if (!string.IsNullOrWhiteSpace(request.input.EmailVCode))
            {
                // 验证邮箱验证码
                var key = string.Format(CacheConst.RegisterEnterpriseEMailVCodeKey, request.input.Email);
                var vcode = await redisClient.GetAsync<string>(key);
                if (string.IsNullOrWhiteSpace(vcode) || vcode != request.input.EmailVCode)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0098)]);
                await redisClient.DelAsync(key); // 验证通过后删除验证码
            }
            else
            {
                // 验证短信验证码
                var key = string.Format(CacheConst.RegisterEnterpriseSMSVCodeKey, request.input.Phone);
                var vcode = await redisClient.GetAsync<string>(key);
                if (string.IsNullOrWhiteSpace(vcode) || vcode != request.input.SmsVCode)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0109)]);
                await redisClient.DelAsync(key); // 验证通过后删除验证码
            }

            // 检查账号是否已存在
            var existingUser = await dbContext.ApplicationUser
                .FirstOrDefaultAsync(u => u.Account == request.input.Account && u.SysMenuType == SysMenuTypeEnum.Merchant, cancellationToken);
            if (existingUser != null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0083)]);

            var input = request.input;

            var addId = YitIdHelper.NextId();

            var enterpriseType = await dbContext.EnterpriseTypes.FirstOrDefaultAsync(w => w.Name == "服务商");

            // 组装企业信息（临时企业信息，上传企业资质后再更新具体信息）
            var enterprise = new EnterpriseInfo(input.Account + input.Email, enterpriseType == null ? 0 : enterpriseType.Id, null, null, id: addId);
            enterprise.UpdateRegistrationProgress(RegistrationProgress.NotStarted);// 设置注册进度为未开始
            await dbContext.EnterpriseInfo.AddAsync(enterprise);

            // 组装用户信息
            var userInfo = new ApplicationUser(enterprise.Id, input.Account, passwordHasher.HashPassword(input.Password), input.NickName, null, input.Phone, input.Email, UserStatusEnum.Enable, AccountTypeEnum.SuperAdmin, SysMenuTypeEnum.Merchant, "企业注册申请，创建默认用户", null);
            userInfo.SetDefault(); // 设置为默认用户
            await dbContext.ApplicationUser.AddAsync(userInfo);

            dbContext.SaveChanges();

            // 创建企业跟用户关系信息
            var enterpriseUser = new EnterpriseUser(enterprise.Id, userInfo.Id);
            await dbContext.EnterpriseUser.AddAsync(enterpriseUser);

            #region 组装登录信息
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
                new Claim("AllDeviceRole", string.Empty),
                new Claim("TenantName", string.Empty),
                new Claim(ClaimConst.Email,userInfo.Email)
            };

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken(claims);
            var loginResponseDto = new LoginResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(1),
                Username = userInfo.Account,
                NickName = userInfo.NickName,
                PhoneVerification = userInfo.VerifyPhone,
                RegistrationProgress = enterprise.RegistrationProgress,
                IsRegister = true,
                Approved = false,
                Email = userInfo.Email,
                UserId = userInfo.Id,
            };
            #endregion

            return loginResponseDto;
        }
    }

    /// <summary>
    /// 设置企业组织类型命令处理器
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="_user"></param>
    public class SetOrganizationTypeCommandHandler(CoffeeMachineDbContext dbContext, UserHttpContext _user) : ICommandHandler<SetOrganizationTypeCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(SetOrganizationTypeCommand request, CancellationToken cancellationToken)
        {
            var enterprise = await dbContext.EnterpriseInfo.FindAsync(_user.TenantId);
            if (enterprise == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0068)]);

            if (enterprise.RegistrationProgress == RegistrationProgress.InReview || enterprise.RegistrationProgress == RegistrationProgress.Failed || enterprise.RegistrationProgress == RegistrationProgress.Passed)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0099)]);

            // 设置组织类型
            enterprise.SetOrganizationType(request.organizationType);

            // 更新注册进度为已设置组织类型
            //enterprise.UpdateRegistrationProgress(RegistrationProgress.StepOne);

            // 更新企业信息
            dbContext.EnterpriseInfo.Update(enterprise);

            return true;
        }
    }

    /// <summary>
    /// 更新企业资质信息命令处理器
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="_user"></param>
    public class UpdateEnterpriseQualificationInfoCommandHandler(CoffeeMachineDbContext dbContext, UserHttpContext _user) : ICommandHandler<UpdateEnterpriseQualificationInfoCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateEnterpriseQualificationInfoCommand request, CancellationToken cancellationToken)
        {
            var input = request.qualificationInfo;
            var enterprise = await dbContext.EnterpriseInfo.FindAsync(_user.TenantId);
            if (enterprise == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0068)]);

            if (enterprise.RegistrationProgress == RegistrationProgress.InReview || enterprise.RegistrationProgress == RegistrationProgress.Passed)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0099)]);

            // 设置企业名称
            enterprise.SetEnterpriseName(input.EnterpriseName);

            // 设置组织类型
            enterprise.SetOrganizationType(input.organizationType);

            // 更新注册进度为已设置组织类型
            enterprise.UpdateRegistrationProgress(RegistrationProgress.InReview);

            // 验证地区关系Id是否存在
            if (input.AreaRelationId <= 0 || input.AreaRelationId == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0100)]);
            var areaRelation = await dbContext.AreaRelation.FirstOrDefaultAsync(w => w.Id == input.AreaRelationId);
            if (areaRelation == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0101)]);

            // 绑定地区关系Id
            enterprise.BindAreaRelationId(input.AreaRelationId ?? 0);

            // 获取企业资质信息，如果不存在则创建新的
            var qualification = await dbContext.EnterpriseQualificationInfo
                .FirstOrDefaultAsync(q => q.EnterpriseId == _user.TenantId, cancellationToken);
            if (qualification == null)
                // 更新企业资质信息
                qualification = new EnterpriseQualificationInfo(enterprise.Id, input.LegalPersonName, input.LegalPersonIdCardNumber, input.FrontImageUrl, input.BackImageUrl);
            else
                // 更新已有的企业资质信息
                qualification.UpdateEnterpriseQualificationInfo(input.LegalPersonName, input.LegalPersonIdCardNumber, input.FrontImageUrl, input.BackImageUrl);

            // 如果是公司则更新企业资质信息
            if (enterprise.OrganizationType == EnterpriseOrganizationTypeEnum.Company)
                qualification.CompleteEnterpriseQualification(input.CustomerServiceEmail, input.StoreAddress, input.BusinessLicenseUrl, input.Othercertificate);
            // 如果是个人/小微商户，清除企业资质信息
            else
                qualification.ClearCompleteEnterpriseQualification();

            // 更新企业信息和资质信息
            dbContext.EnterpriseInfo.Update(enterprise);
            dbContext.EnterpriseQualificationInfo.Update(qualification);

            return true;
        }
    }
}