using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Application.Dtos.Cap
{
    /// <summary>
    /// 平台操作日志
    /// </summary>
    public class PlatformOperationLogDto
    {
        /// <summary>
        /// 操作人
        /// </summary>
        [Required]
        public long OperationUserId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [Required]
        public string OperationUserName { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public TrailTypeEnum TrailType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        public string Describe { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        [Required]
        public bool Result { get; set; } = true;

        /// <summary>
        /// IP地址
        /// </summary>
        public string? Ip { get; set; }

    }
}
