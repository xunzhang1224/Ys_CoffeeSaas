namespace YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input
{
    /// <summary>
    /// 注册设备入参
    /// </summary>
    public class RegisterDeviceInput
    {
        //  public string MID { get; set; }

        /// <summary>
        /// IMEI
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// 公钥
        /// </summary>
        public string PubKey { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimeSp { get; set; }

        /// <summary>
        /// 渠道id
        /// </summary>
        public int ChannelId { get; set; } = 1;
    }
    //public class DeviceLineDto
    //{
    //    public string mid { get; set; }
    //    public DateTime? UpdateOnlineTime { get; set; } = DateTime.UtcNow;
    //}
}