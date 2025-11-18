using Microsoft.AspNetCore.Http;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Const;

namespace YS.CoffeeMachine.Infrastructure
{
    /// <summary>
    /// 用户HttpContext
    /// </summary>
    public class UserHttpContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 用户HttpContext
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public UserHttpContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId => long.Parse((_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimConst.UserId)?.Value) ?? "0");

        /// <summary>
        /// 租户ID
        /// </summary>
        public long TenantId => long.Parse((_httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.TenantId)?.Value) ?? "0");

        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.Account)?.Value;

        /// <summary>
        /// 用户账号
        /// </summary>
        public string SysMenuType => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.SysMenuType)?.Value;

        /// <summary>
        /// 姓名
        /// </summary>
        public string NickName => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.NickName)?.Value;

        /// <summary>
        /// 租户配置Id
        /// </summary>
        public string ConfigId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.ConfigId)?.Value;

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.RealName)?.Value;

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool SuperAdmin => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.AccountType)?.Value == ((int)AccountTypeEnum.SuperAdmin).ToString();

        /// <summary>
        /// 是否默认管理员
        /// </summary>
        public bool IsDefault => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.IsDefault)?.Value == "True";

        /// <summary>
        /// 是否所有设备角色
        /// </summary>
        public bool AllDeviceRole => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.AllDeviceRole)?.Value == "True";

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public long OrgId => long.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.OrgId)?.Value);

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public string Ip => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";

        /// <summary>
        /// 角色编码
        /// </summary>
        public string[] RoleCode
        {
            get
            {
                var roleCodes = _httpContextAccessor.HttpContext?.User.Claims.Where(p => p.Type == ClaimConst.RoleCodes).Select(p => p.Value)
                    .ToArray();
                // var roleCodes = _httpContextAccessor.HttpContext?.User.FindFirst()?.Value.Split(',');
                return roleCodes;
            }
        }

        /// <summary>
        /// 第三方Id
        /// </summary>
        public string OpenId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.OpenId)?.Value;

        /// <summary>
        /// 平台
        /// </summary>
        public string Platform => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.Platform)?.Value;

        /// <summary>
        /// 租户名称
        /// </summary>
        public string TenantName => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.TenantName)?.Value;

        /// <summary>
        /// 租户电话
        /// </summary>
        public string TenantPhone => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.TenantPhone)?.Value;

        /// <summary>
        /// 租户邮箱
        /// </summary>
        public string Email => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.Email)?.Value;

        /// <summary>
        /// 用户电话
        /// </summary>
        public string Phone => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.Phone)?.Value;
    }
}
