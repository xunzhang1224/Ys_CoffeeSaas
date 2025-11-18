namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备型号的聚合根实体。
    /// 用于定义设备的型号信息，包括型号名称、最大料盒数量、备注等。
    /// 同时维护该型号下所有设备实例的引用集合。
    /// </summary>
    public class DeviceModel : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置型号的唯一标识键（Key），用于程序内部识别。
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 获取或设置设备型号的可读名称，用于展示给用户或系统管理员。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// 获取或设置该型号设备支持的最大料盒数量，用于限制配置上限。
        /// </summary>
        public int MaxCassetteCount { get; private set; }

        /// <summary>
        /// 获取或设置型号的备注信息，用于描述额外说明。
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// 获取与此型号关联的所有设备实例集合。
        /// 用于导航至属于该型号的设备列表。
        /// </summary>
        public ICollection<DeviceInfo> DeviceInfos { get; private set; } = new List<DeviceInfo>();

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceModel() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 DeviceModel 实例。
        /// </summary>
        /// <param name="key">型号的唯一标识键。</param>
        /// <param name="name">型号的可读名称。</param>
        /// <param name="maxCassetteCount">最大料盒数量。</param>
        /// <param name="remark">型号的备注信息。</param>
        public DeviceModel(string key, string name, int maxCassetteCount, string remark, string type)
        {
            Key = key;
            Name = name;
            MaxCassetteCount = maxCassetteCount;
            Remark = remark;
            Type = type;
        }

        /// <summary>
        /// 更新当前型号的基本信息。
        /// </summary>
        /// <param name="name">新的型号名称。</param>
        /// <param name="maxCassetteCount">新的最大料盒数量。</param>
        /// <param name="remark">新的备注信息。</param>
        public void Update(string name, int maxCassetteCount, string remark)
        {
            Name = name;
            MaxCassetteCount = maxCassetteCount;
            Remark = remark;
        }
    }
}