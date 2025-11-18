using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class ApplicationUser : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseId { get; private set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; private set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; private set; }
        /// <summary>
        /// 手机区号
        /// </summary>
        public string AreaCode { get; private set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; private set; }
        /// <summary>
        /// 手机号是否验证通过
        /// </summary>
        public bool VerifyPhone { get; private set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; private set; }
        /// <summary>
        /// 账号状态
        /// </summary>
        public UserStatusEnum Status { get; private set; }
        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountTypeEnum AccountType { get; private set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public SysMenuTypeEnum SysMenuType { get; private set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; private set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// 关联角色
        /// </summary>
        public List<ApplicationUserRole> ApplicationUserRoles { get; private set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        public ServiceAuthorizationRecord ServiceAuthorizationRecord { get; private set; }
        /// <summary>
        /// 私有构造
        /// </summary>
        protected ApplicationUser()
        {
            ApplicationUserRoles = new List<ApplicationUserRole>();
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="nickName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="accountType"></param>
        /// <param name="remark"></param>
        /// <param name="roleIds"></param>
        public ApplicationUser(long enterpriseId, string account, string password, string nickName, string areaCode, string phone, string email, UserStatusEnum status, AccountTypeEnum accountType, SysMenuTypeEnum sysMenuType, string remark, List<long>? roleIds)
        {
            EnterpriseId = enterpriseId;
            Account = account;
            Password = password;
            NickName = nickName;
            AreaCode = areaCode;
            Phone = phone;
            VerifyPhone = false;
            Email = email;
            Status = status;
            AccountType = accountType;
            SysMenuType = sysMenuType;
            IsDefault = false;
            Remark = remark;
            if (roleIds != null)
            {
                ApplicationUserRoles = new List<ApplicationUserRole>();
                roleIds.ForEach(s =>
                {
                    ApplicationUserRoles.Add(new ApplicationUserRole(Id, s));
                });
            }
        }
        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="remark"></param>
        public void Update(string account, string nickName, string areaCode, string phone, string email, string remark)
        {
            Account = account;
            NickName = nickName;
            AreaCode = areaCode;
            Phone = phone;
            Email = email;
            Remark = remark;
        }
        /// <summary>
        /// 设置默认账号
        /// </summary>
        public void SetDefault()
        {
            IsDefault = true;
        }

        /// <summary>
        /// 验证手机
        /// </summary>
        public void IsVerifyPhone()
        {
            VerifyPhone = true;
        }

        /// <summary>
        /// 清除用户角色关系
        /// </summary>
        public void CelarUserRoles()
        {
            ApplicationUserRoles.Clear();
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="status"></param>
        public void UpdateStatus(UserStatusEnum status)
        {
            Status = status;
        }

        /// <summary>
        /// 修改类型
        /// </summary>
        /// <param name="accountType"></param>
        public void UpdateAccountType(AccountTypeEnum accountType)
        {
            AccountType = accountType;
        }

        /// <summary>
        /// 修改所属系统类型
        /// </summary>
        /// <param name="sysMenuType"></param>
        public void UpdateSysMenuType(SysMenuTypeEnum sysMenuType)
        {
            SysMenuType = sysMenuType;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newPassword"></param>
        public void UpdatePassWord(string newPassword)
        {
            Password = newPassword;
        }

        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <param name="roleIds"></param>
        public void UpdateUserRoleIds(List<long> roleIds)
        {
            ApplicationUserRoles.Clear();
            roleIds.ForEach(s =>
            {
                ApplicationUserRoles.Add(new ApplicationUserRole(Id, s));
            });
        }
    }
}
