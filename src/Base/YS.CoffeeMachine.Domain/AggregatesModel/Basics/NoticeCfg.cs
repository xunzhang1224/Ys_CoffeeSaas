using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics
{
    /// <summary>
    /// 通知配置
    /// </summary>
    public class NoticeCfg : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 通知类型
        /// 0：预警 1：异常
        /// </summary>
        [Required]
        public int Type { get; private set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public long UserId { get; private set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; private set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        [Required]
        public bool Status { get; private set; } = true;

        /// <summary>
        /// 通知方式
        /// 0：短信 1：邮件
        /// ,隔开
        /// </summary>
        [Required]
        public string Method { get; private set; }

        /// <summary>
        /// a
        /// </summary>
        protected NoticeCfg() { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="status"></param>
        /// <param name="method"></param>
        public NoticeCfg(int type, long userId, string userName, bool status, string method)
        {
            Type = type;
            UserId = userId;
            UserName = userName;
            Status = status;
            Method = method;
        }
    }
}
