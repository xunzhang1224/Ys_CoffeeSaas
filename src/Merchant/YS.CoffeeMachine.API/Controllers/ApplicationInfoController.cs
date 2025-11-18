using FreeRedis;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.API.Extensions.TaskSchedulingBase.BackgroundJobs;
using YS.CoffeeMachine.API.Services.SmsServices;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.MenuCommands;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.RolesCommands;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.Commands.VerificationCodeCommands;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.TermServiceDtos;
using YS.CoffeeMachine.Application.Dtos.TestDtos;
using YS.CoffeeMachine.Application.Dtos.VerificationCodeDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.CoffeeMachine.SignalR.Services;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 企业管理
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="applicationUserQueries"></param>
    /// <param name="applicationRoleQueries"></param>
    /// <param name="applicationMenuQueries"></param>
    /// <param name="enterpriseInfoQueries"></param>
    /// <param name="enterpriseQualificationInfoQueries"></param>
    /// <param name="signalRService"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class ApplicationInfoController(
        IMediator mediator,
        IApplicationUserQueries applicationUserQueries,
        IApplicationRoleQueries applicationRoleQueries,
        IApplicationMenuQueries applicationMenuQueries,
        IEnterpriseInfoQueries enterpriseInfoQueries,
        IEnterpriseQualificationInfoQueries enterpriseQualificationInfoQueries,
        ISignalRService signalRService,
        IAliyunSmsService aliyunSmsService,
        PaymentPlatformUtil paymentPlatformUtil,
        SyncWechatAllApplymentsTJob syncWechatAllApplymentsTJob,
        CoffeeMachineDbContext context,
        //ITimezoneContext _tz,
        IPublishService publish) : Controller
    {
        private static string GetLogOutTokenKey(string userId) => $"/ApplicationUsers/LogOut/{userId}";
        private static string GetLogOutRefreshTokenKey(string userId) => $"/ApplicationUsers/LogOut/RefreshToken/{userId}";
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTest")]
        [AllowAnonymous]
        //[ResponseCache(Duration = 30)]
        public async Task<bool> GetTest([FromBody] TimezoneTestDto input)
        {
            // await signalRService.SendMessageToAllAsync("后端发送的测试消息");

            //var second = 2;
            //await publish.SendDelayMessage(CapConst.DomesticPaymentOrderRefundSyncStatus, new OrderRefundSyncStatusDto
            //{
            //    OrderId = "NB202509082018717491140919301",
            //    OutRefundNo = "4200002812202509084075377395"
            //}, second);

            //var a = await paymentPlatformUtil.GetDeviceBindPaymentMethodDtos("508325000001");
            //var b = await paymentPlatformUtil.GetDeviceBindPaymentMethodDtos(new List<string>() { "508325000001" });

            //var paymentOriginIds = await context.M_PaymentAlipayApplyments
            //    .Where(a => a.FlowStatus == ApplymentFlowStatusEnum.PlatformReview || a.FlowStatus == ApplymentFlowStatusEnum.Initialize || a.FlowStatus == ApplymentFlowStatusEnum.Failed)
            //    .Select(a => a.PaymentOriginId)
            //    .ToListAsync();

            //await paymentPlatformUtil.SyncAlipayApplymentsState(paymentOriginIds.ToArray());

            #region 时区测试
            //var a = _tz.ConvertToLocal(DateTime.UtcNow);
            #endregion

            #region 同步基础地区编码信息
            //var sysProvinceCitys = await context.SysProvinceCity.ToListAsync();

            //var countryRegions = await context.CountryRegion.ToListAsync();

            //countryRegions.ForEach(cr =>
            //{
            //    var provinceCity = sysProvinceCitys.FirstOrDefault(s => s.Name == cr.RegionName);
            //    if (provinceCity != null)
            //    {
            //        cr.SetCodeInfo(provinceCity.Code, provinceCity.ParentCode, provinceCity.Type);
            //    }
            //});

            //context.UpdateRange(countryRegions);

            //await context.SaveChangesAsync();
            #endregion

            //await syncWechatAllApplymentsTJob.Execute();

            #region 短信测试
            //string templateParamJson = JsonConvert.SerializeObject(new
            //{
            //    regist_number = "J729748019359749",
            //    failure_reason = "经营者/负责人证件号码填写错误或与证件类型不匹配，请检查证件类型并重新填写"
            //});

            //string templateParamJson2 = JsonConvert.SerializeObject(new
            //{
            //    payment_method = "微信支付"
            //});

            //// 发送验证码
            //await aliyunSmsService.SendSmsAsync("18153721527", SmsConst.MerchantApplymentFailed, templateParamJson);

            //var res = await aliyunSmsService.SendSmsAsync("111", SmsConst.LoginVerify, JsonConvert.SerializeObject(new { vcode = "123456" }));
            #endregion

            #region 同步企业H5菜单
            //var htMenuIds = await context.ApplicationMenu.AsQueryable().AsNoTracking().Where(w => w.SysMenuType == SysMenuTypeEnum.H5).Select(s => s.Id).ToListAsync();
            //var enterporises = await context.EnterpriseInfo.ToListAsync();
            //enterporises.ForEach(e => e.UpdateH5MenuIds(htMenuIds));
            //context.UpdateRange(enterporises);
            //await context.SaveChangesAsync();
            #endregion
            return true;
        }

        /// <summary>
        /// 测试阿里云短信验证码发送
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestSms")]
        [AllowAnonymous]
        public async Task TestSms()
        {
            throw new NotImplementedException();
            //await aliyunSmsService.SendSmsAsync("18813922797", SmsConst.MerchantPaymentOnboardingVerificationCode);
        }

        #region 登录

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<LoginResponseDto> Login([FromBody] LoginCommand command) => await mediator.Send(command);

        /// <summary>
        /// 发送邮箱登录验证码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SendEmailLoginCode")]
        public async Task<SendVerificationCodeResponseDto> SendEmailLoginCode([FromBody] SendEmailLoginCodeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 邮箱登录
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("EmailLogin")]
        public async Task<LoginResponseDto> EmailLogin([FromBody] EmailLoginCommand command) => await mediator.Send(command);

        /// <summary>
        /// 发送短信登录验证码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SendSMSLoginCode")]
        public async Task<SendVerificationCodeResponseDto> SendSMSLoginCode([FromBody] SendSMSLoginCodeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 短信登录
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SmsLogin")]
        public async Task<LoginResponseDto> SmsLogin([FromBody] SmsLoginCommand command) => await mediator.Send(command);

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<LoginResponseDto> RefreshToken([FromBody] RefreshCommand command) => await mediator.Send(command);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="_user"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("LogOut")]
        public async Task<bool> LogOut([FromServices] IRedisClient redis, [FromServices] UserHttpContext _user, [FromBody] string refreshToken)
        {
            // 当前用户信息不存在
            if (_user == null || _user.UserId <= 0) return false;

            // 获取请求头中的 Authorization Token
            var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader == null || !authorizationHeader.StartsWith("Bearer "))
            {
                // 如果没有提供 Authorization 头或者格式不正确，返回失败
                return false;
            }

            // 提取 Token，去掉 "Bearer " 前缀
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            // 获取 Access Token 的过期时间
            DateTime? accessTokenExpiration = GetTokenExpiration(token);

            // 获取 Refresh Token 的过期时间
            DateTime? refreshTokenExpiration = GetTokenExpiration(refreshToken);

            // 将 Access Token 存储到 Redis，并设置过期时间
            if (accessTokenExpiration.HasValue)
            {
                var accessTokenExpirationTime = accessTokenExpiration.Value - DateTime.UtcNow;
                await redis.SetAsync(GetLogOutTokenKey(_user.UserId.ToString()), token, accessTokenExpirationTime);
            }

            // 将 Refresh Token 存储到 Redis，并设置过期时间
            if (refreshTokenExpiration.HasValue)
            {
                var refreshTokenExpirationTime = refreshTokenExpiration.Value - DateTime.UtcNow;
                await redis.SetAsync(GetLogOutRefreshTokenKey(_user.UserId.ToString()), refreshToken, refreshTokenExpirationTime);
            }

            // 返回退出成功
            return true;
        }

        /// <summary>
        /// 根据token获取token过期时间
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private DateTime? GetTokenExpiration(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // 获取 "exp" 字段，表示过期时间
                var exp = jwtToken?.Payload?.FirstOrDefault(p => p.Key == "exp").Value;

                if (exp != null)
                {
                    // "exp" 是 Unix 时间戳，需要转换成 DateTime
                    var expirationDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp)).UtcDateTime;
                    return expirationDate;
                }
            }
            catch (Exception)
            {
                // 如果解析失败，可以选择记录日志或处理错误
                return null;
            }

            return null;
        }

        /// <summary>
        /// 切换账号
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ChangeAccountBuUserId")]
        public async Task<LoginResponseDto> ChangeAccountBuUserId([FromBody] ChangeAccountBuUserIdCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取同邮箱的用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetChangeUserList")]
        public async Task<List<ChangeUserDto>> GetChangeUserListAsync() => await applicationUserQueries.GetChangeUserListAsync();
        #endregion

        #region 发送验证码/找回密码
        /// <summary>
        /// 发送修改密码的验证码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SendUpdatePwdVCode")]
        public Task<SendVerificationCodeResponseDto> SendVerificationCode(SendVerificationCodeCommand command) => mediator.Send(command);

        /// <summary>
        /// 通过验证码更新密码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdatePassWordByVCode")]
        public Task<bool> UpdatePassWordByVCode([FromBody] UpdatePassWordByVCodeCommand command) => mediator.Send(command);

        /// <summary>
        /// 通过旧密码更新密码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdatePasswordByOldPwd")]
        public Task<bool> UpdatePasswordByOldPwd([FromBody] UpdatePasswordByOldPwdCommand command) => mediator.Send(command);
        #endregion

        #region 企业相关
        /// <summary>
        /// 创建企业信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateEnterpriseInfo")]
        public Task<bool> CreateEnterpriseInfo([FromBody] CreateEnterpriseInfoCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 根据Id获取企业信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isInclude"></param>
        /// <returns></returns>
        [HttpPost("GetEnterpriseInfoInfoById")]
        public Task<EnterpriseInfoDto> GetEnterpriseInfoInfoById(long id, bool isInclude)
        {
            return enterpriseInfoQueries.GetEnterpriseInfoAsync(id, isInclude);
        }

        ///// <summary>
        ///// 根据当前用户所在企业Id获取企业列表
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost("GetEnterpriseListById")]
        //public Task<EnterpriseInfoListDto> GetEnterpriseListById([FromBody] QueryRequest request)
        //{
        //    return enterpriseInfoQueries.GetEnterpriseListByIdAsync(request);
        //}
        ///// <summary>
        ///// 根据企业Id获取企业列表
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="enterpriseId"></param>
        ///// <returns></returns>
        //[HttpPost("GetEnterpriseListByEnterpriseId")]
        //public Task<EnterpriseInfoListDto> GetEnterpriseListByEnterpriseId([FromBody] QueryRequest request, [FromQuery] long enterpriseId)
        //{
        //    return enterpriseInfoQueries.GetEnterpriseListByEnterpriseIdAsync(request, enterpriseId);
        //}
        /// <summary>
        /// 根据企业Id获取企业树
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        [HttpPost("GetEnterpriseTreeByEnterpriseId")]
        public Task<EnterpriseInfoDto> GetEnterpriseTreeByEnterpriseId([FromQuery] long enterpriseId)
        {
            return enterpriseInfoQueries.GetEnterpriseTreeAsync(enterpriseId);
        }
        /// <summary>
        /// 根据企业Id获取下级企业列表
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        [HttpPost("GetEnterpriseListByEnterpriseId")]
        public async Task<List<EnterpriseInfoDto>> GetEnterpriseListByEnterpriseId([FromQuery] long enterpriseId) => await enterpriseInfoQueries.GetEnterpriseInfoDtosByPId(enterpriseId);
        /// <summary>
        /// 根据当前用户所在企业Id获取企业树
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCurUserEnterpriseTree")]
        public Task<EnterpriseInfoDto> GetCurUserEnterpriseTree() => enterpriseInfoQueries.GetEnterpriseTreeAsync();
        /// <summary>
        /// 根据Id查询企业类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetEnterpriseTypeById")]
        public Task<EnterpriseTypesDto> GetEnterpriseTypeByIdAsync([FromQuery] long id) => enterpriseInfoQueries.GetEnterpriseTypeByIdAsync(id);
        /// <summary>
        /// 获取企业类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetEnterpriseTypesList")]
        public Task<List<EnterpriseTypesDto>> GetEnterpriseTypesListAsync() => enterpriseInfoQueries.GetEnterpriseTypesAsync();
        /// <summary>
        /// 更改企业拥有的菜单Ids
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateEnterpriseMenuIds")]
        public Task<bool> UpdateEnterpriseMenuIdsAsync([FromBody] UpdateEnterPriseMenuIdsCommand command) => mediator.Send(command);
        /// <summary>
        /// 编辑企业信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateEnterpriseInfoInfo")]
        public Task<bool> UpdateEnterpriseInfoInfo([FromBody] UpdateEnterpriseInfoCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 根据Id删除企业信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteEnterpriseInfoById")]
        public Task<bool> DeleteEnterpriseInfoById(DeleteEnterpriseInfoCommand command)
        {
            return mediator.Send(command);
        }
        #endregion

        #region 企业注册

        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("IsAccountExists")]
        public async Task<bool?> IsAccountExists(string account) => await enterpriseQualificationInfoQueries.IsAccountExists(account);

        /// <summary>
        /// 企业注册邮件验证码发送
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SendRegisterEnterpriseEmailVCode")]
        public async Task<bool> SendRegisterEnterpriseEmailVCodeAsync([FromBody] SendVcodeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 企业注册短信验证码发送
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SendRegisterEnterpriseSMSVCode")]
        public async Task<bool> SendRegisterEnterpriseSMSVCodeAsync([FromBody] SendSMSVcodeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 企业注册
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("RegisterEnterprise")]
        public async Task<LoginResponseDto> RegisterEnterpriseAsync([FromBody] RegisterEnterpriseCommand command) => await mediator.Send(command);

        /// <summary>
        /// 设置企业组织类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SetEnterpriseQualificationInfo")]
        public async Task<bool> SetOrganizationTypeAsync([FromBody] SetOrganizationTypeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 更新企业资质信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateEnterpriseQualificationInfo")]
        public async Task<bool> UpdateEnterpriseQualificationInfoAsync([FromBody] UpdateEnterpriseQualificationInfoCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取企业注册信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEnterpriseQualificationInfo")]
        public async Task<EnterpriseQualificationInfoOutput> GetEnterpriseQualificationInfoAsync() => await enterpriseQualificationInfoQueries.GetEnterpriseQualificationInfoAsync();

        /// <summary>
        /// 获取地区关系列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAreaRelationAllList")]
        public async Task<List<AreaRelationDto>> GetAreaRelationAllList() => await enterpriseInfoQueries.GetAreaRelationAllList();
        #endregion

        #region 用户相关

        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateApplicationUser")]
        public Task<bool> CreateApplicationUser([FromBody] CreateApplicationUserCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 根据Id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isInclude"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationUserInfoById")]
        public Task<ApplicationUserDto> GetApplicationUserInfoById([FromQuery] long id, [FromQuery] bool isInclude)
        {
            return applicationUserQueries.GetApplicationUserInfoAsync(id, isInclude);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCurrentApplicationUserInfoAsync")]
        public Task<ApplicationUserDto> GetCurrentApplicationUserInfoAsync()
        {
            return applicationUserQueries.GetCurrentApplicationUserInfoAsync();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationUserList")]
        public Task<PagedResultDto<ApplicationUserDto>> GetApplicationUserList([FromBody] QueryRequest request)
        {
            return applicationUserQueries.GetApplicationUserListAsync(request);
        }

        /// <summary>
        /// 根据企业Id获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationUserByEnterpriseIdList")]
        public Task<PagedResultDto<ApplicationUserDto>> GetApplicationUserByEnterpriseIdList([FromBody] ApplicationUserInput input)
        {
            return applicationUserQueries.GetApplicationUserByEnterpriseIdListAsync(input);
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateApplicationUserInfo")]
        public Task<bool> UpdateApplicationUserInfo([FromBody] UpdateApplicationUserCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("ResetUserPassword")]
        public Task<bool> ResetUserPassword([FromBody] ResetUserPasswordCommand command) => mediator.Send(command);

        /// <summary>
        /// 手机号验证通过
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("PhoneVerificationPassed")]
        public Task<bool> PhoneVerificationPassed([FromBody] PhoneVerificationPassedCommand command) => mediator.Send(command);

        /// <summary>
        /// 根据Id删除用户信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteApplicationUserById")]
        public Task<bool> DeleteApplicationUserById(DeleteApplicationUserCommand command)
        {
            return mediator.Send(command);
        }

        #endregion

        #region 用户角色相关

        /// <summary>
        /// 创建角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateApplicationRole")]
        public Task<bool> CreateApplicationRole([FromBody] CreateApplicationRoleCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 根据Id获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeMenu"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationRoleInfoById")]
        public Task<ApplicationRoleDto> GetApplicationRoleInfoById([FromBody] long id, bool includeMenu)
        {
            return applicationRoleQueries.GetApplicationRoleAsync(id, includeMenu);
        }

        /// <summary>
        /// 根据企业Id获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="enterpriseId"></param>
        /// <param name="name"></param>
        /// <param name="roleStatus"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationRoleByEnterpriseIdList")]
        public Task<PagedResultDto<ApplicationRoleDto>> GetApplicationRoleByEnterpriseIdList([FromBody] QueryRequest request, [FromQuery] long enterpriseId, [FromQuery] string name, [FromQuery] RoleStatusEnum? roleStatus)
        {
            return applicationRoleQueries.GetApplicationRoleByEnterpriseIdListAsync(request, enterpriseId, name, roleStatus);
        }

        /// <summary>
        /// 获取当前用户角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEnterpriceRoleList")]
        public async Task<List<ApplicationRoleDto>> GetEnterpriceRoleList(long enterpriseId)
        {
            return await applicationRoleQueries.GetEnterpriceRoleList(enterpriseId);
        }

        /// <summary>
        /// 根据角色Id获取菜单Id集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetUserMenusByRoleId")]
        public Task<List<long>> GetUserMenusByRoleId(long id)
        {
            return applicationRoleQueries.GetUserMenusByRoleIdAsync(id);
        }

        /// <summary>
        /// 根据企业Id获取角色下拉框
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        [HttpPost("GetRoleSelect")]
        public Task<List<object>> GetRoleSelectAsync(long enterpriseId)
        {
            return applicationRoleQueries.GetRoleSelectAsync(enterpriseId);
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationRoleList")]
        public Task<PagedResultDto<ApplicationRoleDto>> GetApplicationRoleList([FromBody] QueryRequest request)
        {
            return applicationRoleQueries.GetApplicationRoleListAsync(request);
        }

        //public Task<List<long>> GetUserMenusIdsByRoleId()

        /// <summary>
        /// 编辑角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateApplicationRole")]
        public Task<bool> UpdateApplicationRole([FromBody] UpdateApplicationRoleCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 根据id删除角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteApplicationRoleInfo")]
        public Task<bool> DeleteApplicationRoleInfo(DeleteApplicationRoleCommand command)
        {
            return mediator.Send(command);
        }

        #endregion

        #region 角色菜单相关

        /// <summary>
        /// 添加菜单信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        //[HttpPost("CreateApplicationMenu")]
        //public Task<bool> CreateApplicationMenu([FromBody] CreateApplicationMenuCommand command)
        //{
        //    return mediator.Send(command);
        //}

        /// <summary>
        /// 获取所有菜单树
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetMenuSelectAsync")]
        public Task<List<MenuSelectDto>> GetMenuSelectAsync()
        {
            return applicationMenuQueries.GetMenuSelectAsync();
        }

        /// <summary>
        /// 根据Id获取菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationMenuInfoById")]
        public Task<ApplicationMenuDto> GetApplicationMenuInfoById(long id)
        {
            return applicationMenuQueries.GetApplicationMenuAsync(id);
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationMenuList")]
        public Task<MenusDto> GetApplicationMenuList([FromBody] QueryRequest request)
        {
            return applicationMenuQueries.GetApplicationMenuListAsync(request);
        }
        /// <summary>
        /// 根据上级企业Id获取菜单列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parentTenantId"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationMenuListByParentTenantId")]
        public Task<MenusDto> GetApplicationMenuListByParentTenantId([FromBody] QueryRequest request, [FromQuery] long parentTenantId, [FromQuery] long? enterpriseTypeId = null)
        {
            return applicationMenuQueries.GetApplicationMenuListByParentTenantIdAsync(request, parentTenantId, enterpriseTypeId);
        }

        /// <summary>
        /// 获取当前用户的菜单权限列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUserMenus")]
        [Authorize]
        public Task<List<object>> GetUserMenus([FromQuery] SysMenuTypeEnum sysMenuType)
        {
            return applicationMenuQueries.GetUserMenuTreeAsync(sysMenuType);
        }

        /// <summary>
        /// 编辑菜单信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateApplicationMenu")]
        public Task<bool> UpdateApplicationMenu([FromBody] UpdateApplicationMenuCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 根据id删除菜单信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteApplicationMenuInfo")]
        public Task<bool> DeleteApplicationMenuInfo(DeleteApplicationMenuCommand command)
        {
            return mediator.Send(command);
        }
        #endregion

        #region 服务条款

        /// <summary>
        /// 根据Id获取服务条款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSingleTermServiceById")]
        public async Task<SingleTermServiceOutput> GetSingleTermServiceById([FromQuery] long id) => await applicationUserQueries.GetSingleTermServiceById(id);

        /// <summary>
        /// 根据Id获取服务条款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetCommonSingleTermService")]
        public async Task<SingleTermServiceOutput> GetCommonSingleTermService([FromQuery] long id) => await applicationUserQueries.GetSingleTermServiceById(id);
        #endregion

        #region 服务授权相关

        #endregion

        #region 切换租户（不重新登录）

        /// <summary>
        /// 切换租户（修改租户信息）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ChangeEnterpriseById")]
        public async Task<LoginResponseDto> ChangeEnterpriseById(ChangeEnterpriseByIdCommand command) => await mediator.Send(command);
        #endregion
    }
}