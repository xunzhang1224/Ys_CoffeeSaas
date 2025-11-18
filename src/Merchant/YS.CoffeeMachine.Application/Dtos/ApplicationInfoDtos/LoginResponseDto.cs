using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 登录响应
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// 用户的访问令牌 (Access Token)
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户的刷新令牌 (Refresh Token)
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 访问令牌的过期时间 (UTC 时间)
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 手机号是否验证
        /// </summary>
        public bool PhoneVerification { get; set; }

        /// <summary>
        /// 注册进度
        /// </summary>
        public bool IsRegister { get; set; } = false;

        /// <summary>
        /// 审核通过
        /// </summary>
        public bool Approved { get; set; }

        /// <summary>
        /// 注册进度
        /// </summary>
        public RegistrationProgress? RegistrationProgress { get; set; } = null;

        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseInfoId { get; set; }

        /// <summary>
        /// 当前企业时区
        /// </summary>
        public string? TimeZone { get; set; } = null;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string? Phone { get; set; } = null;

        /// <summary>
        /// 服务条款Id
        /// </summary>
        public long? TremServiceId { get; set; } = null;

        /// <summary>
        /// 当前用户的菜单树
        /// </summary>
        //public List<object> MenuTree { get; set; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户所属企业Id
        /// </summary>
        public long EnterpriseId { get; set; }

        /// <summary>
        /// 用户显示名（昵称或真实姓名）
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 系统
        /// </summary>
        public string SysMenuType { get; set; }
    }
}
