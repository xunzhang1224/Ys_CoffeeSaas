using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.Notity
{
    /// <summary>
    /// a
    /// </summary>
    public class NotityCfgOutput
    {
        /// <summary>
        /// 通知类型
        /// 0：预警 1：异常
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 通知方式
        /// 0：短信 1：邮件
        /// ,隔开
        /// </summary>
        public string Method { get; set; }
    }
}
