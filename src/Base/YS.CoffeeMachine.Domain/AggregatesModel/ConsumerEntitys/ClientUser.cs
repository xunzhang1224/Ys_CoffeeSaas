using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.ConsumerEntitys
{
    /// <summary>
    /// 消费者用户表
    /// </summary>
    public class ClientUser : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; private set; }

        /// <summary>
        /// 微信关联Id(openId)
        /// </summary>
        public string? WechatId { get; private set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// 性别 0未知 1男 2女
        /// </summary>
        public int Sex { get; private set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; private set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; private set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; private set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; private set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum Enabled { get; private set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; private set; }

        /// <summary>
        /// 保护构造函数
        /// </summary>
        protected ClientUser() { }

        /// <summary>
        /// 创建消费者用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="sex"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="nickName"></param>
        /// <param name="birthday"></param>
        /// <param name="avatar"></param>
        /// <param name="wechatId"></param>
        public ClientUser(string account, string userName, string password, int sex, string? phone, string? email, string nickName, DateTime birthday, string? avatar, string? wechatId)
        {
            Account = account;
            UserName = userName;
            Password = password;
            Sex = sex;
            Phone = phone;
            Email = email;
            NickName = nickName;
            Birthday = birthday;
            Avatar = avatar;
            WechatId = wechatId;
            Enabled = EnabledEnum.Enable;
            RegisterTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="nickName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="sex"></param>
        public ClientUser(string account, string nickName, string password, string? email, string? phone, int? sex)
        {
            Account = account;
            NickName = nickName;
            Password = password;
            Email = email;
            Phone = phone;
            Sex = sex ?? 0;
            Enabled = EnabledEnum.Enable;
            RegisterTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 微信小程序注册用户
        /// </summary>
        /// <param name="wechatId"></param>
        /// <param name="nickName"></param>
        /// <param name="avatar"></param>
        public void WxRegisterUser(string wechatId, string nickName, string? avatar)
        {
            WechatId = wechatId;
            NickName = nickName;
            Avatar = avatar;
            Enabled = EnabledEnum.Enable;
            RegisterTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userNamem"></param>
        /// <param name="sex"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="nickName"></param>
        /// <param name="avatar"></param>
        public void UpdateUserInfo(string userNamem, int sex, string? phone, string? email, string nickName, string? avatar)
        {
            UserName = userNamem;
            Sex = sex;
            Phone = phone;
            Email = email;
            NickName = nickName;
            Avatar = avatar;
        }

        /// <summary>
        /// 设置生日
        /// </summary>
        /// <param name="birthday"></param>
        public void SetBirthday(DateTime birthday)
        {
            if (Birthday == null)
                Birthday = birthday;
        }

        /// <summary>
        /// 更新启用状态
        /// </summary>
        /// <param name="enabled"></param>
        public void UpdateEnabled(EnabledEnum enabled)
        {
            Enabled = enabled;
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="password"></param>
        public void UpdatePassword(string password)
        {
            Password = password;
        }
    }
}