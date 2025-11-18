using System.Data;
using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class ApplicationUserDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 电话区号
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 账号状态
        /// </summary>
        public UserStatusEnum Status { get; set; }
        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountTypeEnum AccountType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否默认用户
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 关联角色
        /// </summary>
        public List<ApplicationRoleDto> applicationRoleDtos { get; set; }

        /// <summary>
        /// 用户角色列表
        /// </summary>
        public RoleDto Role { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 关联的设备Ids
        /// </summary>
        public List<long> DevicesIds { get; set; }

        /// <summary>
        /// 是否可以注销
        /// </summary>
        public bool IsCanLogOff { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationUserDto()
        {
            applicationRoleDtos = new List<ApplicationRoleDto>();
        }

        /// <summary>
        /// 绑定用户角色
        /// </summary>
        public void BindUserRole(List<ApplicationUserRole> userRoles)
        {
            if (userRoles.Count > 0)
            {
                var roles = new List<ApplicationRole>();
                roles = userRoles.Select(s => s.Role).ToList();

                if (roles.Count > 0)
                {
                    applicationRoleDtos = new List<ApplicationRoleDto>();
                    roles.ForEach(x =>
                    {
                        applicationRoleDtos.Add(new ApplicationRoleDto(x.Id, x.Name, x.Status, x.IsDefault, x.Sort, x.HasSuperAdmin, x.Remark, x.Code));
                    });
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationUserDto(long id, string account, string nickName, string areaCode, string phone, string email, UserStatusEnum status, AccountTypeEnum accountType, string remark, List<ApplicationUserRole> userRoles)
        {
            Id = id;
            Account = account;
            NickName = nickName;
            AreaCode = areaCode;
            Phone = phone;
            Email = email;
            Status = status;
            AccountType = accountType;
            Remark = remark;
            if (userRoles.Count > 0)
            {
                var roles = new List<ApplicationRole>();
                roles = userRoles.Select(s => s.Role).ToList();

                if (roles.Count > 0)
                {
                    applicationRoleDtos = new List<ApplicationRoleDto>();
                    roles.ForEach(x =>
                    {
                        applicationRoleDtos.Add(new ApplicationRoleDto(x.Id, x.Name, x.Status, x.IsDefault, x.Sort, x.HasSuperAdmin, x.Remark, x.Code));
                    });
                }
            }
        }
    }

    /// <summary>
    /// 用户列表
    /// </summary>
    public class ApplicationUserListDto
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<ApplicationUserDto> ApplicationUserDots { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ApplicationUserListDto() { ApplicationUserDots = new List<ApplicationUserDto>(); }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationUserListDto(List<ApplicationUser> applicationUsers)
        {
            if (applicationUsers.Count > 0)
            {
                ApplicationUserDots = new List<ApplicationUserDto>();
                applicationUsers.ForEach(x =>
                {
                    ApplicationUserDots.Add(new ApplicationUserDto(x.Id, x.Account, x.NickName, x.AreaCode, x.Phone, x.Email, x.Status, x.AccountType, x.Remark, x.ApplicationUserRoles));
                });
            }
        }
    }

    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public List<long> Ids { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public List<string> Names { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        public List<string?>? Codes { get; set; } = null;

        /// <summary>
        /// 角色编码名称
        /// </summary>
        public Dictionary<string, string?> CodeNames { get; set; } = null;
    }

    /// <summary>
    /// 切换用户信息
    /// </summary>
    public class ChangeUserDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 注册进度
        /// </summary>
        public RegistrationProgress? RegistrationProgress { get; set; } = null;
    }

    #region H5_UserInfo_Dtos
    /// <summary>
    /// H5用户信息
    /// </summary>
    public class H5Users
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话区号
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 角色Ids
        /// </summary>
        public List<long> RoleIds { get; set; }

        /// <summary>
        /// 角色名称，多个逗号分隔
        /// </summary>
        public string RoleNames { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public UserStatusEnum Status { get; set; }

        /// <summary>
        /// 是否默认用户
        /// </summary>
        public bool isDefault { get; set; }

        /// <summary>
        /// 是否启用文本
        /// </summary>
        public string StatusText
        {
            get
            {
                return Status.GetDescriptionOrValue();
            }
        }
    }
    #endregion
}