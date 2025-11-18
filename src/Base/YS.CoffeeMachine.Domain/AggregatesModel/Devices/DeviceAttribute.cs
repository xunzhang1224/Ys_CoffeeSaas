namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System.ComponentModel.DataAnnotations;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备属性的聚合根实体，用于管理设备相关的元数据信息。
    /// 该类继承自 BaseEntity，并实现了 IAggregateRoot 接口，表明其在领域模型中作为聚合根的角色。
    /// 主要用于存储和管理设备的扩展属性，包括键值对形式的配置信息。
    /// </summary>
    public class DeviceAttribute : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置关联的平台设备唯一标识符。
        /// 用于标识该属性所属的设备基础信息。
        /// </summary>
        [Required]
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 获取或设置属性的键（Key），用于唯一标识该属性。
        /// 通常用于程序中查找和操作特定属性。
        /// </summary>
        [Required]
        public string Key { get; private set; }

        /// <summary>
        /// 获取或设置属性的可读名称（Name），用于展示给用户。
        /// 可为空，适用于需要显示友好名称的场景。
        /// </summary>
        public string? Name { get; private set; }

        /// <summary>
        /// 获取或设置属性的值（Value），表示该属性的具体内容。
        /// 可为空，用于存储任意文本类型的配置值。
        /// </summary>
        public string? Value { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceAttribute() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 DeviceAttribute 实例。
        /// </summary>
        /// <param name="deviceBaseId">关联的平台设备唯一标识符。</param>
        /// <param name="key">属性的键。</param>
        /// <param name="name">属性的可读名称。</param>
        /// <param name="value">属性的值。</param>
        public DeviceAttribute(long deviceBaseId, string key, string? name, string? value)
        {
            DeviceBaseId = deviceBaseId;
            Key = key;
            Name = name;
            Value = value;
        }

        /// <summary>
        /// 更新当前属性的值。
        /// </summary>
        /// <param name="value">新的属性值。</param>
        public void Update(string value)
        {
            Value = value;
        }
    }
}