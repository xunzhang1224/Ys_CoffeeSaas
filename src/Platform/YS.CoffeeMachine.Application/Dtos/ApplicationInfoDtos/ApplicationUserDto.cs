namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

    /// <summary>
    /// 用户信息数据传输对象（DTO），用于在应用程序层和表现层之间传递用户相关数据。
    /// </summary>
    public class ApplicationUserDto
    {
        /// <summary>
        /// 用户唯一标识 ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户账号，用于登录系统
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户昵称，用于界面显示
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 电话区号，如：+86
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 账号状态（启用/禁用）
        /// </summary>
        public UserStatusEnum Status { get; set; }

        /// <summary>
        /// 账号类型（普通用户、管理员等）
        /// </summary>
        public AccountTypeEnum AccountType { get; set; }

        /// <summary>
        /// 备注信息，用于描述用户的额外信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否为默认用户
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 关联的角色 DTO 列表
        /// </summary>
        public List<ApplicationRoleDto> applicationRoleDtos { get; set; }

        /// <summary>
        /// 用户角色列表封装模型，包含角色 ID 和名称列表
        /// </summary>
        public RoleDto Role { get; set; }

        /// <summary>
        /// 关联的设备 ID 列表
        /// </summary>
        public List<long> DevicesIds { get; set; }

        /// <summary>
        /// 默认构造函数，初始化角色集合为空列表
        /// </summary>
        public ApplicationUserDto()
        {
            applicationRoleDtos = new ();
        }

        /// <summary>
        /// 绑定用户角色信息到角色 DTO 列表
        /// </summary>
        /// <param name="userRoles">用户角色实体集合</param>
        public void BindUserRole(List<ApplicationUserRole> userRoles)
        {
            if (userRoles.Count > 0)
            {
                var roles = new List<ApplicationRole>();
                roles = userRoles.Select(s => s.Role).ToList();

                if (roles.Count > 0)
                {
                    applicationRoleDtos = new ();
                    roles.ForEach(x =>
                    {
                        applicationRoleDtos.Add(new ApplicationRoleDto(x.Id, x.Name, x.Status, x.IsDefault, x.Sort, x.HasSuperAdmin, x.Remark));
                    });
                }
            }
        }

        /// <summary>
        /// 带参数的构造函数，用于从实体对象初始化 DTO 对象
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="account">账号</param>
        /// <param name="nickName">昵称</param>
        /// <param name="areaCode">电话区号</param>
        /// <param name="phone">手机号</param>
        /// <param name="email">邮箱</param>
        /// <param name="status">账号状态</param>
        /// <param name="accountType">账号类型</param>
        /// <param name="remark">备注信息</param>
        /// <param name="userRoles">用户角色列表</param>
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

            if (userRoles != null && userRoles.Count > 0)
            {
                var roles = userRoles.Select(s => s.Role).ToList();
                applicationRoleDtos = new ();
                roles.ForEach(x =>
                {
                    applicationRoleDtos.Add(new ApplicationRoleDto(x.Id, x.Name, x.Status, x.IsDefault, x.Sort, x.HasSuperAdmin, x.Remark));
                });
            }
        }
    }

    /// <summary>
    /// 用户列表数据传输对象，用于封装多个 ApplicationUserDto 实例
    /// </summary>
    public class ApplicationUserListDto
    {
        /// <summary>
        /// 用户 DTO 列表
        /// </summary>
        public List<ApplicationUserDto> ApplicationUserDots { get; set; }

        /// <summary>
        /// 受保护的无参构造函数，初始化空的用户 DTO 列表
        /// </summary>
        protected ApplicationUserListDto()
        {
            ApplicationUserDots = new ();
        }

        /// <summary>
        /// 根据 ApplicationUser 实体列表初始化用户 DTO 列表
        /// </summary>
        /// <param name="applicationUsers">用户实体集合</param>
        public ApplicationUserListDto(List<ApplicationUser> applicationUsers)
        {
            if (applicationUsers?.Count > 0)
            {
                ApplicationUserDots = new ();
                applicationUsers.ForEach(x =>
                {
                    ApplicationUserDots.Add(new ApplicationUserDto(
                        x.Id,
                        x.Account,
                        x.NickName,
                        x.AreaCode,
                        x.Phone,
                        x.Email,
                        x.Status,
                        x.AccountType,
                        x.Remark,
                        x.ApplicationUserRoles));
                });
            }
        }
    }

    /// <summary>
    /// 角色信息封装 DTO，用于存储角色 ID 和名称列表
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// 角色 ID 列表
        /// </summary>
        public List<long> Ids { get; set; }

        /// <summary>
        /// 角色名称列表
        /// </summary>
        public List<string> Names { get; set; }
    }
}