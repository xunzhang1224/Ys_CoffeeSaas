namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using MessagePack;

/// <summary>
/// 用于6002号指令的协议实体：远程清除货道故障并测试货道指令.
/// </summary>
[MessagePackObject(true)]
public class DownlinkEntity6002 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 6002;

    #region 公共属性

    /// <summary>
    /// 开始货道
    /// </summary>
    public int StartSlotNo { get; set; }

    /// <summary>
    /// 结束货道号
    /// </summary>
    public int EndSlotNo { get; set; }

    /// <summary>
    /// 默认1; 0不清除 1清除
    /// </summary>
    public int ClearErr { get; set; } = 1;

    /// <summary>
    /// 默认0; 0不测试 1测试。
    /// </summary>
    public int TestSlot { get; set; } = 0;

    #endregion

    #region 响应实体

    /// <summary>
    /// 响应实体
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
    }
    #endregion
}