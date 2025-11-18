using System.ComponentModel.DataAnnotations;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 表示设备能力配置的聚合根实体。
    /// 该类继承自 BaseEntity，并实现了 IAggregateRoot 接口，表明其在领域模型中作为聚合根的角色。
    /// 主要用于管理设备的能力配置信息，包括硬件或软件能力、权限控制、数据结构定义等。
    /// </summary>
    public class DeviceCapacityCfg : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置关联的设备基础信息唯一标识符。
        /// </summary>
        [Required]
        public long DeviceBaseId { get; private set; }

        /// <summary>
        /// 获取或设置能力的唯一标识（枚举类型），用于区分不同的能力项。
        /// </summary>
        public CapabilityIdEnum CapacityId { get; private set; }

        /// <summary>
        /// 获取或设置能力的可读名称，用于展示给用户或系统管理员。
        /// 可为空。
        /// </summary>
        public string? Name { get; private set; }

        /// <summary>
        /// 获取或设置能力的类型，0:未知，1:硬件能力，2:软件能力。
        /// </summary>
        public CapacityTypeEnum CapacityType { get; private set; }

        /// <summary>
        /// 获取或设置配置信息的具体内容，通常表示能力的当前值或参数设置。
        /// 可为空。
        /// </summary>
        public string? CfgInfo { get; private set; }

        /// <summary>
        /// 获取或设置权限属性，用于控制该能力的访问和修改权限。
        /// 0:可读可写，1:可读不可写，2:不可读可写，3:不可读不可写。
        /// </summary>
        public PremissionTypeEnum Premission { get; private set; }

        /// <summary>
        /// 获取或设置配置信息的数据结构类型。
        /// 0:整型，1:布尔型，2:字符串，3:JSON对象，4:JSON数组，5:浮点型，6:String数组型。
        /// </summary>
        public StructureTypeEnum Structure { get; private set; }

        /// <summary>
        /// 使用指定参数初始化一个新的 DeviceCapacityCfg 实例。
        /// </summary>
        /// <param name="deviceBaseId">关联的设备基础信息唯一标识。</param>
        /// <param name="capacityId">能力的唯一标识。</param>
        /// <param name="name">能力的可读名称。</param>
        /// <param name="capacityType">能力的类型。</param>
        /// <param name="cfgInfo">配置信息的具体内容。</param>
        /// <param name="premission">权限属性。</param>
        /// <param name="structure">数据结构类型。</param>
        public DeviceCapacityCfg(long deviceBaseId, CapabilityIdEnum capacityId, string? name, CapacityTypeEnum capacityType,
            string? cfgInfo, PremissionTypeEnum premission, StructureTypeEnum structure)
        {
            DeviceBaseId = deviceBaseId;
            CapacityId = capacityId;
            Name = name;
            CapacityType = capacityType;
            CfgInfo = cfgInfo;
            Structure = structure;
            Premission = premission;
        }

        /// <summary>
        /// 更新当前配置的参数值及相关属性。
        /// </summary>
        /// <param name="cfgInfo">新的配置信息。</param>
        /// <param name="premission">可选的新权限属性，默认保留原值。</param>
        /// <param name="structure">可选的新数据结构类型，默认保留原值。</param>
        public void Update(string? cfgInfo, PremissionTypeEnum? premission = null, StructureTypeEnum? structure = null)
        {
            CfgInfo = cfgInfo;
            if (structure != null)
                Structure = structure ?? StructureTypeEnum.Int;
            if (premission != null)
                Premission = premission ?? PremissionTypeEnum.Rw;
        }
    }
}