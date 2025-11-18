using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 登录响应dto
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
