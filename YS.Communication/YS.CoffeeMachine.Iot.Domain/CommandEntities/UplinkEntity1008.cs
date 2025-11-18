namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 用于1008号指令的请求实体：VMC向服务器上报能力.
/// </summary>
[MessagePackObject(true)]
public class UplinkEntity1008 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1008;

    /// <summary>
    /// Gets or sets 表示机器硬件能力标识位.
    /// </summary>
    public int HardwareCapability { get; set; }

    /// <summary>
    /// Gets or sets 表示机器软件能力标识位.
    /// </summary>
    public int SoftwareCapability { get; set; }

    /// <summary>
    /// 用于1008号指令的应答实体：VMC向服务器上报能力.
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
    }
}