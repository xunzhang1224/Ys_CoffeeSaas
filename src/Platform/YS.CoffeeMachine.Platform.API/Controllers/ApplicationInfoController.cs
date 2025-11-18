using FreeRedis;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseTypesCommands;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.MenuCommands;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.RolesCommands;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Cap.IServices;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 平台应用信息管理
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="enterpriseInfoQueries"></param>
    /// <param name="applicationRoleQueries"></param>
    /// <param name="enterpriseTypesQueries"></param>
    /// <param name="applicationMenuQueries"></param>
    /// <param name="applicationUserQueries"></param>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class ApplicationInfoController(IMediator mediator, IP_EnterpriseInfoQueries enterpriseInfoQueries, IP_ApplicationRoleQueries applicationRoleQueries,
        IP_EnterpriseTypesQueries enterpriseTypesQueries, IP_ApplicationMenuQueries applicationMenuQueries, IP_ApplicationUserQueries applicationUserQueries, UserHttpContext _httpContext, IPublishService _cap) : Controller
    {
        /// <summary>
        /// 测试(平台)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTest")]
        public Task<string> GetTest() => Task.FromResult("_OK");
        #region 登录
        private static readonly string GetBasketKey = "/ApplicationUsers/LogOut";
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public Task<LoginResponseDto> Login([FromBody] LoginCommand command) => mediator.Send(command);
        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public Task<LoginResponseDto> RefreshToken([FromBody] RefreshCommand command) => mediator.Send(command);
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="refreshToekn"></param>
        /// <returns></returns>
        [HttpPost("LogOut")]
        public async Task<bool> LogOut([FromServices] IRedisClient redis, [FromBody] string refreshToekn)
        {
            await redis.SetAsync(GetBasketKey, refreshToekn);

            var log = new PlatformOperationLogDto()
            {
                OperationUserId = _httpContext.UserId,
                OperationUserName = _httpContext.NickName,
                TrailType = TrailTypeEnum.Not,
                Describe = "登出",
                Result = true,
                Ip = _httpContext.Ip
            };
            await _cap.SendMessage(CapConst.PlatformOperationLog, log);
            return true;
        }
        #endregion

        #region 企业类型
        /// <summary>
        /// 创建企业类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateEnterpriseType")]
        public Task<bool> CreateEnterpriseTypesAsync([FromBody] CreateEnterpriseTypesCommand command) => mediator.Send(command);
        /// <summary>
        /// 根据Id查询企业类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetEnterpriseTypeById")]
        public Task<P_EnterpriseTypesDto> GetEnterpriseTypeByIdAsync([FromQuery] long id) => enterpriseTypesQueries.GetEnterpriseTypeByIdAsync(id);
        /// <summary>
        /// 获取企业类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetEnterpriseTypesList")]
        public Task<List<P_EnterpriseTypesDto>> GetEnterpriseTypesListAsync() => enterpriseTypesQueries.GetEnterpriseTypesAsync();
        /// <summary>
        /// 根据企业类型Id获取菜单树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetMenuSelectByEnterpriseTypeId")]
        public Task<AllMenuSelectDto> GetMenuSelectByEnterpriseTypeIdAsync(long id) => applicationMenuQueries.GetMenuSelectByEnterpriseTypeIdAsync(id);
        /// <summary>
        /// 编辑企业类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateEnterpriseType")]
        public Task<bool> UpdateEnterpriseTypesAsync([FromBody] UpdateEnterpriseTypesCommand command) => mediator.Send(command);
        /// <summary>
        /// 根据Id删除企业类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteEnterpriseTypeById")]
        public Task<bool> DeleteEnterpriseTypesByIdAsync([FromBody] DeleteEnterpriseTypesCommand command) => mediator.Send(command);
        #endregion

        #region 企业管理
        /// <summary>
        /// 创建默认企业信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateDefaultEnterpriseInfo")]
        public Task<bool> CreateDefaultEnterpriseInfo([FromBody] CreateEnterpriseInfoCommand command) => mediator.Send(command);
        /// <summary>
        /// 查询主企业下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetEnterpriseSelectList")]
        public Task<List<P_EnterpriseSelect>> GetEnterpriseSelectListAsync() => enterpriseInfoQueries.GetEnterpriseSelectListAsync();
        /// <summary>
        /// 获取企业信息分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetEnterpriseInfoListAsync")]
        public Task<PagedResultDto<P_EnterpriseInfoDto>> GetEnterpriseInfoListAsync([FromBody] P_EnterpriseInfoInput request) => enterpriseInfoQueries.GetEnterpriseInfoListAsync(request);
        /// <summary>
        /// 编辑企业信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateEnterpriseInfo")]
        public Task<bool> UpdateEnterpriseInfo([FromBody] UpdateEnterpriseCommand command) => mediator.Send(command);
        /// <summary>
        /// 重置企业管理员密码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("ResetAdministratorPassword")]
        public Task<bool> ResetAdministratorPassword([FromBody] ResetAdministratorPasswordCommand command) => mediator.Send(command);
        /// <summary>
        /// 注销企业及企业下的用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteEnterpriseInfo")]
        public Task<bool> DeleteEnterpriseInfo([FromBody] DeleteEnterpriseInfoCommand command) => mediator.Send(command);

        /// <summary>
        /// 企业审核
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateRegistrationAudit")]
        public Task<bool> UpdateRegistrationAuditAsync([FromBody] UpdateRegistrationAuditCommand command) => mediator.Send(command);
        #endregion

        #region 企业注册审核

        /// <summary>
        /// 企业注册审核列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetEnterpriseRegisterList")]
        public async Task<PagedResultDto<P_EnterpriseRegisterDto>> GetEnterpriseRegisterListAsync([FromBody] P_EnterpriseRegisterInput input) => await enterpriseInfoQueries.GetEnterpriseRegisterListAsync(input);

        /// <summary>
        /// 企业注册审核资质信息
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        [HttpGet("GetEnterpriseQualificationInfo")]
        public async Task<EnterpriseQualificationInfoDto> GetEnterpriseQualificationInfo(long enterpriseId) => await enterpriseInfoQueries.GetEnterpriseQualificationInfoAsync(enterpriseId);

        /// <summary>
        /// 根据Id获取企业信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetNormalEnterpriseInfoById")]
        public async Task<P_EnterpriseInfoDto> GetNormalEnterpriseInfoByIdAsync(long id)
        {
            return await enterpriseInfoQueries.GetNormalEnterpriseInfoById(id);
        }
        #endregion

        #region 用户管理
        /// <summary>
        /// 创建平台用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreatePlatformUser")]
        public Task<bool> CreatePlatformUserAsync([FromBody] P_CreateUserCommand command) => mediator.Send(command);
        /// <summary>
        /// 根据Id查询平台用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IncludeRole"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationUserInfoById")]
        public Task<ApplicationUserDto> GetApplicationUserInfoAsync([FromQuery] long id, [FromQuery] bool IncludeRole) => applicationUserQueries.GetApplicationUserInfoAsync(id, IncludeRole);
        /// <summary>
        /// 根据企业Id获取默认用户列表
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        [HttpPost("GetDefaultUserByEnterpriseId")]
        public Task<List<P_ApplicationUserDto>> GetDefaultUserByEnterpriseIdAsync([FromQuery] long enterpriseId) => applicationUserQueries.GetDefaultUserByEnterpriseIdAsync(enterpriseId);
        /// <summary>
        /// 获取平台用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationUserList")]
        public Task<PagedResultDto<ApplicationUserDto>> GetApplicationUserListAsync([FromBody] IP_ApplicationUserInput request) => applicationUserQueries.GetApplicationUserListAsync(request);
        /// <summary>
        /// 编辑平台用户信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdatePlatformUser")]
        public Task<bool> UpdatePlatformUserAsync([FromBody] P_UpdateUserCommand command) => mediator.Send(command);
        /// <summary>
        /// 修改平台用户状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdatePlatformUserStatus")]
        public Task<bool> UpdatePlatformUserStatusAsync([FromBody] P_UpdateUserStatusCommand command) => mediator.Send(command);
        /// <summary>
        /// 修改平台用户账户类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdatePlatformUserAccountType")]
        public Task<bool> UpdatePlatformUserAccountTypeAsync([FromBody] P_UpdateUserAccountTypeCommand command) => mediator.Send(command);
        /// <summary>
        /// 重置平台用户密码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("ResetPlatformUserPassword")]
        public Task<bool> UpdatePlatformUserPasswordAsync([FromBody] P_ResetUserPasswordCommand command) => mediator.Send(command);
        /// <summary>
        /// 删除平台用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeletePlatformUser")]
        public Task<bool> DeletePlatformUserAsync([FromBody] P_DeleteUserCommand command) => mediator.Send(command);
        #endregion

        #region 默认角色管理
        /// <summary>
        /// 创建默认角色
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateDefaultApplicationRole")]
        public Task<bool> CreateDefaultApplicationRole([FromBody] P_CreateRoleCommand command) => mediator.Send(command);

        #region 平台角色
        /// <summary>
        /// 根据Id查询平台角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isInclude"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationRoleInfoById")]
        public Task<ApplicationRoleDto> GetApplicationRoleInfoById([FromQuery] long id, [FromQuery] bool isInclude) => applicationRoleQueries.GetApplicationRoleAsync(id, isInclude);
        /// <summary>
        /// 获取平台角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="roleStatus"></param>
        /// <returns></returns>
        [HttpPost("GetPlatformRoleList")]
        public Task<PagedResultDto<ApplicationRoleDto>> GetPlatformRoleList([FromBody] QueryRequest request, [FromQuery] RoleStatusEnum? roleStatus) => applicationRoleQueries.GetPlatformRoleListAsync(request, roleStatus);
        /// <summary>
        /// 根据角色Id获取平台菜单Ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost("GetPlatformMenusByRoleId")]
        public Task<List<long>> GetPlatformMenusByRoleIdAsync([FromQuery] long roleId) => applicationRoleQueries.GetPlatformMenusByRoleIdAsync(roleId);
        #endregion

        #region 商户角色
        /// <summary>
        /// 获取商户角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetMerchantRoleList")]
        public Task<PagedResultDto<ApplicationRoleDto>> GetMerchantRoleListAsync([FromBody] QueryRequest request) => applicationRoleQueries.GetMerchantRoleListAsync(request);
        /// <summary>
        /// 根据角色Id获取商户菜单Ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost("GetMerchantMenusByRoleId")]
        public Task<List<long>> GetMerchantMenusByRoleIdAsync([FromQuery] long roleId) => applicationRoleQueries.GetMerchantMenusByRoleIdAsync(roleId);
        #endregion

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateApplicationRole")]
        public Task<bool> UpdateApplicationRole([FromBody] P_UpdateRoleCommand command) => mediator.Send(command);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteApplicationRole")]
        public Task<bool> DeleteApplicationRole([FromBody] P_DeleteRoleCommand command) => mediator.Send(command);
        #endregion

        #region 菜单管理
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateMenu")]
        public Task<bool> CreateMenuAsync([FromBody] P_CreateMenuCommand command) => mediator.Send(command);
        /// <summary>
        /// 根据Id获取菜单权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationMenuById")]
        public Task<ApplicationMenuDto> GetApplicationMenuAsync([FromQuery] long id) => applicationMenuQueries.GetApplicationMenuAsync(id);
        /// <summary>
        /// 获取所有菜单树
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllMenuTree")]
        public Task<List<MenuSelectDto>> GetAllMenuTreeAsync() => applicationMenuQueries.GetAllMenuTreeAsync();
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sysMenuType"></param>
        /// <returns></returns>
        [HttpPost("GetApplicationMenuList")]
        public Task<ApplicationMenuListDto> GetApplicationMenuListAsync([FromBody] QueryRequest request, [FromQuery] SysMenuTypeEnum sysMenuType) => applicationMenuQueries.GetApplicationMenuListAsync(request, sysMenuType);
        /// <summary>
        /// 获取当前用户菜单树
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUserMenuTree")]
        public Task<List<object>> GetUserMenuTreeAsync() => applicationMenuQueries.GetUserMenuTreeAsync();
        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateMenu")]
        public Task<bool> UpdateMenuAsync([FromBody] P_UpdateMenuCommand command) => mediator.Send(command);
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMenu")]
        public Task<bool> DeleteMenuAsync([FromBody] P_DeleteMenuCommand command) => mediator.Send(command);
        #endregion
    }
}