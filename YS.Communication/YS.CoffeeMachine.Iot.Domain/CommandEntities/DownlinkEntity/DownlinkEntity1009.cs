using MessagePack;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

/// <summary>
/// 用于1009号指令的协议实体：服务器向VMC下发能力.
/// </summary>
[MessagePackObject(true)]
public class DownlinkEntity1009 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1009;

    /// <summary>
    /// Gets or sets 表示机器硬件能力标识位.
    /// </summary>
    public int HardwareCapability { get; set; }

    /// <summary>
    /// Gets or sets 表示机器软件能力标识位.
    /// </summary>
    public int SoftwareCapability { get; set; }

    /// <summary>
    /// 用于1009号指令的应答实体.
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
    }
}