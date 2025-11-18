namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System;
    using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备与用户之间的关联实体。
    /// 用于实现一个设备可被多个用户绑定的多对一关系管理。
    /// </summary>
    public class DeviceUserAssociation : BaseEntity
    {
        /// <summary>
        /// 获取或设置关联的设备唯一标识符。
        /// </summary>
        public long DeviceId { get; private set; }

        /// <summary>
        /// 获取或设置关联的用户唯一标识符。
        /// </summary>
        public long UserId { get; private set; }

        /// <summary>
        /// 获取与此关联绑定的用户实体对象。
        /// 用于导航至用户信息。
        /// </summary>
        public ApplicationUser User { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceUserAssociation() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 DeviceUserAssociation 实例。
        /// </summary>
        /// <param name="deviceId">设备的唯一标识。</param>
        /// <param name="userId">用户的唯一标识。</param>
        public DeviceUserAssociation(long deviceId, long userId)
        {
            DeviceId = deviceId;
            UserId = userId;
        }
    }
}