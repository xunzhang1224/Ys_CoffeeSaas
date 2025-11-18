namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using MessagePack;

/// <summary>
/// 用于6216号指令的协议实体：远程下发通用控制指令.
/// </summary>
[MessagePackObject(true)]
public class DownlinkEntity6216 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 6216;

    #region 公共属性

    /// <summary>
    /// TransId
    /// </summary>
    public string TransId { get; set; }

    /// <summary>
    /// CapabilityId
    /// </summary>
    public int CapabilityId { get; set; }

    /// <summary>
    /// 9
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Parameters
    /// </summary>
    public List<string> Parameters { get; set; }
    #endregion

    #region 响应实体

    /// <summary>
    /// 接收
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 状态 0:收到,1:成功,2:失败.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 能力ID
        /// </summary>
        public int CapabilityId { get; set; }

        /// <summary>
        /// 控制名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 执行后的输出参数
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
    #endregion
}