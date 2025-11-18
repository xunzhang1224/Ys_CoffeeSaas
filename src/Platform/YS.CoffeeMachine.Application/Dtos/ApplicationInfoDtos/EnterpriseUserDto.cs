using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 用户dto
    /// </summary>
    public class EnterpriseUserDto
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
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
        /// 构造函数
        /// </summary>
        protected EnterpriseUserDto() { }

        /// <summary>
        /// dto
        /// </summary>
        /// <param name="account"></param>
        /// <param name="nickName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="accountType"></param>
        /// <param name="remark"></param>
        public EnterpriseUserDto(string account, string nickName, string phone, string email, UserStatusEnum status, AccountTypeEnum accountType, string remark)
        {
            Account = account;
            NickName = nickName;
            Phone = phone;
            Email = email;
            Status = status;
            AccountType = accountType;
            Remark = remark;
        }
    }
}
