namespace YS.CoffeeMachine.Application.Dtos.BasicDtos
{
    /// <summary>
    /// 企业跟设备基础信息Dto
    /// </summary>
    public class EDBaseEntityDto
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
    }
}