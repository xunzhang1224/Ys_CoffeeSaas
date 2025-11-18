namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备与服务商之间的关联实体。
    /// 用于管理某台设备所绑定的服务提供商信息。
    /// </summary>
    public class DeviceServiceProviders : BaseEntity
    {
        /// <summary>
        /// 获取或设置关联的服务提供商唯一标识符。
        /// </summary>
        public long ServiceProviderInfoId { get; private set; }

        /// <summary>
        /// 获取或设置关联的设备唯一标识符。
        /// </summary>
        public long DeviceInfoId { get; private set; }

        /// <summary>
        /// 获取与此关联绑定的设备实体对象。
        /// </summary>
        public DeviceInfo DeviceInfo { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceServiceProviders() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 DeviceServiceProviders 实例。
        /// </summary>
        /// <param name="serviceProviderInfoId">服务提供商的唯一标识。</param>
        /// <param name="deviceInfoId">设备的唯一标识。</param>
        public DeviceServiceProviders(long serviceProviderInfoId, long deviceInfoId)
        {
            ServiceProviderInfoId = serviceProviderInfoId;
            DeviceInfoId = deviceInfoId;
        }
    }
}