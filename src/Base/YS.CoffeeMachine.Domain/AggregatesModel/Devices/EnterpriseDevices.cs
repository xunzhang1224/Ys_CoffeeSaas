namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System;
    using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
    using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示企业与设备之间关系的聚合根实体。
    /// 用于记录设备归属于哪个企业的信息，以及分配/回收时间等元数据。
    /// </summary>
    public class EnterpriseDevices : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置关联的设备唯一标识符。
        /// </summary>
        public long DeviceId { get; private set; }

        /// <summary>
        /// 获取与此设备绑定的设备信息实体对象。
        /// 用于导航至设备详情。
        /// </summary>
        public DeviceInfo Device { get; private set; }

        /// <summary>
        /// 获取或设置该设备所属的企业唯一标识符。
        /// </summary>
        public long BelongingEnterpriseId { get; private set; }

        /// <summary>
        /// 获取或设置当前分配给的企业唯一标识符。
        /// </summary>
        public long EnterpriseId { get; private set; }

        /// <summary>
        /// 获取与此设备关联的企业信息实体对象。
        /// 用于导航至企业详情。
        /// </summary>
        public EnterpriseInfo Enterprise { get; private set; }

        /// <summary>
        /// 获取或设置设备的分配方式类型。
        /// 参考 DeviceAllocationEnum 定义。
        /// </summary>
        public DeviceAllocationEnum DeviceAllocationType { get; private set; }

        /// <summary>
        /// 获取或设置设备的回收时间（如果已回收）。
        /// 可为空。
        /// </summary>
        public DateTime? RecyclingTime { get; private set; }

        /// <summary>
        /// 获取或设置设备的分配时间。
        /// 默认值为当前 UTC 时间。
        /// </summary>
        public DateTime? AllocateTime { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected EnterpriseDevices() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 EnterpriseDevices 实例。
        /// </summary>
        /// <param name="belongingEnterpriseId">设备归属的企业ID。</param>
        /// <param name="deviceId">关联的设备唯一标识。</param>
        /// <param name="enterpriseId">当前分配到的企业ID。</param>
        /// <param name="allocationType">设备分配类型。</param>
        /// <param name="recyclingTime">设备预计或实际回收时间。</param>
        public EnterpriseDevices(
            long belongingEnterpriseId,
            long deviceId,
            long enterpriseId,
            DeviceAllocationEnum allocationType,
            DateTime? recyclingTime)
        {
            BelongingEnterpriseId = belongingEnterpriseId;
            DeviceId = deviceId;
            EnterpriseId = enterpriseId;
            DeviceAllocationType = allocationType;
            RecyclingTime = recyclingTime;
            AllocateTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 更新当前实例的企业分配信息。
        /// </summary>
        /// <param name="enterpriseId">新的企业ID。</param>
        /// <param name="allocationType">新的设备分配类型。</param>
        /// <param name="recyclingTime">新的回收时间。</param>
        /// <param name="allocateTime">新的分配时间（可选）。</param>
        public void Update(
            long enterpriseId,
            DeviceAllocationEnum allocationType,
            DateTime? recyclingTime,
            DateTime? allocateTime)
        {
            EnterpriseId = enterpriseId;
            DeviceAllocationType = allocationType;
            RecyclingTime = recyclingTime;
            AllocateTime = allocateTime ?? AllocateTime;
        }

        /// <summary>
        /// 更新所属企业
        /// </summary>
        /// <param name="belongingId"></param>
        public void UpdateEnterpriseId(long belongingId)
        {
            EnterpriseId = belongingId;
        }
    }
}