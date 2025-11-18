namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备初始化信息的聚合根实体。
    /// 该类用于记录设备在初次注册或初始化阶段的相关数据，包括通信密钥、绑定状态、设备标识等。
    /// 主要用于设备激活流程中的身份验证与安全绑定。
    /// </summary>
    public class DeviceInitialization : Entity, IAggregateRoot
    {
        /// <summary>
        /// 获取设备的临时通讯编码（MID），作为唯一标识符和外键使用。
        /// </summary>
        public string Mid { get; private set; }

        /// <summary>
        /// 获取设备的生产编号或唯一设备号，用于设备识别。
        /// </summary>
        public string EquipmentNumber { get; private set; }

        /// <summary>
        /// 获取或设置设备的国际移动设备识别码（IMEI），可为空。
        /// </summary>
        public string? IMEI { get; private set; }

        /// <summary>
        /// 获取或设置设备是否已绑定的状态，默认为 false（未绑定）。
        /// </summary>
        public bool IsBind { get; private set; } = false;

        /// <summary>
        /// 获取设备的私钥信息，用于安全通信的身份认证。
        /// </summary>
        public string PriKey { get; private set; }

        /// <summary>
        /// 获取设备的公钥信息，用于加密通信。
        /// </summary>
        public string PubKey { get; private set; }

        /// <summary>
        /// 获取设备所属的通道 ID，用于消息路由或服务区分。
        /// </summary>
        public string ChanneId { get; private set; }

        /// <summary>
        /// 获取设备初始化记录的创建时间，UTC 时间。
        /// </summary>
        public DateTime CreateDate { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// 使用指定参数初始化一个新的 DeviceInitialization 实例。
        /// </summary>
        /// <param name="mid">设备的临时通讯编码。</param>
        /// <param name="equipmentNumber">设备的生产编号。</param>
        /// <param name="iMEI">设备的 IMEI 号。</param>
        /// <param name="priKey">设备的私钥。</param>
        /// <param name="pubKey">设备的公钥。</param>
        /// <param name="channeId">设备对应的通道 ID。</param>
        public DeviceInitialization(string mid, string equipmentNumber, string iMEI, string priKey, string pubKey, string channeId)
        {
            Mid = mid;
            EquipmentNumber = equipmentNumber;
            IMEI = iMEI;
            PriKey = priKey;
            PubKey = pubKey;
            ChanneId = channeId;
        }

        /// <summary>
        /// 更新设备的绑定状态。
        /// </summary>
        /// <param name="isBind">新的绑定状态值。</param>
        public void UpdateBind(bool isBind)
        {
            IsBind = isBind;
        }

        /// <summary>
        /// 返回此实体的主键集合，用于 EF Core 或其他 ORM 框架识别实体标识。
        /// </summary>
        /// <returns>包含主键值的对象数组。</returns>
        public override object[] GetKeys() => new object[] { Mid };
    }
}