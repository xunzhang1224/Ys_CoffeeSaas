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
    /// 预警
    /// </summary>
    public class DeviceEarlyWarningsOutput
    {
        /// <summary>
        /// 预警id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 物料表id
        /// </summary>
        public long? DeviceMaterialId { get; set; }

        /// <summary>
        /// 预警类型
        /// </summary>
        public EarlyWarningTypeEnum WarningType { get; set; }

        /// <summary>
        /// 类型
        /// 1咖啡豆、2水量、3料盒、4杯盖、5杯子
        /// </summary>
        public MaterialTypeEnum? Type { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 预警开关
        /// </summary>
        public bool IsOn { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string? Capacity { get; set; }

        /// <summary>
        /// 预警值
        /// </summary>
        public string? WarningValue { get; set; }
    }
}
