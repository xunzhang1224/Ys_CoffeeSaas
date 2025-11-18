namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using MessagePack;

/// <summary>
/// 用于10025号指令的协议实体：机器质检指令.
/// </summary>
[MessagePackObject(true)]
public class DownlinkEntity10025 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 10025;

    /// <summary>
    /// 设备类型
    /// </summary>
    public string MType { get; set; }

    /// <summary>
    /// 机器型号
    /// </summary>
    public string MV { get; set; }

    /// <summary>
    /// 柜
    /// </summary>
    public List<IdAndInclude> Counters { get; set; }

    /// <summary>
    /// 门
    /// </summary>
    public List<IdAndInclude> Doors { get; set; }

    /// <summary>
    /// 层
    /// </summary>
    public List<IdAndInclude> Layers { get; set; }

    /// <summary>
    /// 托盘
    /// </summary>
    public List<IdAndInclude> Trays { get; set; }

    /// <summary>
    /// 传感器
    /// </summary>
    public List<IdAndModel> Snos { get; set; }

    /// <summary>
    /// 门与传感器关系
    /// </summary>
    public List<IdAndGSon> DoorAndSnos { get; set; }

    /// <summary>
    /// 不同机型不同类型定义内容
    /// </summary>
    public string Extra { get; set; }

    /// <summary>
    /// 硬件有无配置，值按照序号顺序配置，例如 {“Config”: “01” } 表示 LED无，主摄像头有
    /// </summary>
    public string Config { get; set; }
}

/// <summary>
/// 门、柜、层、托盘关系模型
/// </summary>
[MessagePackObject(true)]
public class IdAndInclude
{
    /// <summary>
    /// id 代表对应动作
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Include
    /// </summary>
    public string Include { get; set; }
}

/// <summary>
/// 传感器关系模型
/// </summary>
[MessagePackObject(true)]
public class IdAndModel
{
    /// <summary>
    /// 代表对应动作
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Mode
    /// </summary>
    public string Mode { get; set; }
}

/// <summary>
/// 门与传感器关系模型
/// </summary>
[MessagePackObject(true)]
public class IdAndGSon
{
    /// <summary>
    /// 代表对应动作
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// GSnos
    /// </summary>
    public string GSnos { get; set; }
}
