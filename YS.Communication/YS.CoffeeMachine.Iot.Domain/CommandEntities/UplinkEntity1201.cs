namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;

/// <summary>
/// 用于1201号指令的请求实体：VMC向服务器申请时间戳（校时指令）.
/// </summary>
public class UplinkEntity1201 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1201;

    /// <summary>
    /// 用于1201号指令的响应实体：VMC向服务器申请时间戳（校时指令）.
    /// </summary>
    public class Response : BaseCmd
    {
        /// <summary>
        /// Gets or sets 服务器时间戳.
        /// </summary>
        public long ServerTimeSp { get; set; }
    }
}