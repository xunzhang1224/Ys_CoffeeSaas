using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.SettringDtos
{
    /// <summary>
    /// a
    /// </summary>
    public class NoticeCfgInput
    {
        /// <summary>
        /// 通知类型
        /// 0：预警 1：异常
        /// </summary>
        [Required]
        public int Type { get; set; }

        /// <summary>
        /// d
        /// </summary>
        public List<NoticeCfgDto> NoticeCfgs { get; set; }
    }

    /// <summary>
    /// /
    /// </summary>
    public class NoticeCfgDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        [Required]
        public bool Status { get; set; } = true;

        /// <summary>
        /// 通知方式
        /// 0：短信 1：邮件
        /// ,隔开
        /// </summary>
        [Required]
        public string Method { get; set; }
    }
}
