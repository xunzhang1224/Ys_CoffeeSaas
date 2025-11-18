namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备与分组之间关联的实体类。
    /// 用于实现设备按逻辑分组管理的功能。
    /// </summary>
    public class GroupDevices : BaseEntity
    {
        /// <summary>
        /// 获取或设置关联的分组唯一标识符。
        /// </summary>
        public long GroupsId { get; private set; }

        /// <summary>
        /// 获取或设置关联的设备唯一标识符。
        /// </summary>
        public long DeviceInfoId { get; private set; }

        /// <summary>
        /// 获取与此设备绑定的设备信息实体对象。
        /// 用于导航至设备详情。
        /// </summary>
        public DeviceInfo DeviceInfo { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected GroupDevices() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 GroupDevices 实例。
        /// </summary>
        /// <param name="groupId">设备所属分组的唯一标识。</param>
        /// <param name="deviceId">设备的唯一标识。</param>
        public GroupDevices(long groupId, long deviceId)
        {
            GroupsId = groupId;
            DeviceInfoId = deviceId;
        }
    }
}