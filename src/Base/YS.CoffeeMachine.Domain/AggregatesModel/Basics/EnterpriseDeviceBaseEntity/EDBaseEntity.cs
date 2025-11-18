namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics.EnterpriseDeviceBaseEntity
{
    /// <summary>
    /// 企业跟设备基础字段实体
    /// </summary>
    public class EDBaseEntity : EnterpriseBaseEntity
    {
        /// <summary>
        /// 当时的企业Id
        /// </summary>
        public long? BaseEnterpriseId { get; private set; } = null;

        /// <summary>
        /// 当时的企业名称
        /// </summary>
        public string? BaseEnterpriseName { get; private set; } = null;

        /// <summary>
        /// 当时的设备Id
        /// </summary>
        public long? BaseDeviceId { get; private set; } = null;

        /// <summary>
        /// 当时的设备名称
        /// </summary>
        public string? BaseDeviceName { get; private set; } = null;

        /// <summary>
        /// 当时的设备型号Id
        /// </summary>
        public long? BaseDeviceModelId { get; private set; } = null;

        /// <summary>
        /// 当时的设备型号名称
        /// </summary>
        public string? BaseDeviceModelName { get; private set; } = null;

        /// <summary>
        /// 包含构造函数
        /// </summary>
        protected EDBaseEntity() { }

        /// <summary>
        /// 设置基础信息
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <param name="enterpriseName"></param>
        /// <param name="deviceId"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceModelId"></param>
        /// <param name="deviceModelName"></param>
        public void SetEDBaseInfo(long enterpriseId, string enterpriseName, long deviceId, string deviceName, long deviceModelId, string deviceModelName)
        {
            BaseEnterpriseId = enterpriseId;
            BaseEnterpriseName = enterpriseName;
            BaseDeviceId = deviceId;
            BaseDeviceName = deviceName;
            BaseDeviceModelId = deviceModelId;
            BaseDeviceModelName = deviceModelName;
        }
    }
}