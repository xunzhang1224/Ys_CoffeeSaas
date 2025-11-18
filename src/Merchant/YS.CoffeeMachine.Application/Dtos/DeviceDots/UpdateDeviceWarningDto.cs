using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设置物料预警
    /// </summary>
    public class UpdateDeviceWarningDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 预警类型
        /// </summary>
        [Required]
        public EarlyWarningTypeEnum WarningType { get; private set; }

        /// <summary>
        /// 预警开关
        /// </summary>
        public bool IsOn { get; set; }

        /// <summary>
        /// 预警值
        /// </summary>
        public string? WarningValue { get; set; }
    }
}
