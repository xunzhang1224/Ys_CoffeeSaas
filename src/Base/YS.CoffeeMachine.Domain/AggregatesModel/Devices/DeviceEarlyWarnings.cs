using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 设备预警表
    /// </summary>
    public class DeviceEarlyWarnings : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 机器
        /// </summary>
        [Required]
        public long DeviceBaseId { get; private set; }

        /// <summary>
        /// 预警类型
        /// </summary>
        [Required]
        public EarlyWarningTypeEnum WarningType { get; private set; }

        /// <summary>
        /// 物料表关联字段
        /// WarningTypeEnum为物料预警才会有值
        /// </summary>
        public long? DeviceMaterialId { get; private set; }

        /// <summary>
        /// 预警开关
        /// </summary>
        public bool IsOn { get; private set; } = false;

        /// <summary>
        /// 预警值
        /// </summary>
        public string? WarningValue { get; private set; }

        /// <summary>
        /// aaa
        /// </summary>
        protected DeviceEarlyWarnings() { }

        /// <summary>
        ///  aa
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <param name="warningType"></param>
        /// <param name="isOn"></param>
        /// <param name="warningValue"></param>
        /// <param name="deviceMaterialId"></param>
        public DeviceEarlyWarnings(long deviceBaseId, EarlyWarningTypeEnum warningType, bool isOn = false, string? warningValue = null, long? deviceMaterialId = null)
        {
            DeviceBaseId = deviceBaseId;
            WarningType = warningType;
            IsOn = isOn;
            WarningValue = warningValue;
            DeviceMaterialId = deviceMaterialId;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="isOn"></param>
        /// <param name="warningValue"></param>
        public void Update(bool isOn = false, string? warningValue = null)
        {
            IsOn = isOn;
            WarningValue = warningValue;
        }

        /// <summary>
        /// 绑定物料信息
        /// </summary>
        /// <param name="deviceMaterialId"></param>
        public void BindMaterial(long deviceMaterialId)
        {
            // 只有物料预警才能绑定
            if (WarningType == EarlyWarningTypeEnum.ShortageWarning)
            {
                DeviceMaterialId = deviceMaterialId;
            }
        }
    }
}
