namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 1006.VMC向服务器发送登录指令
/// </summary>
public class UplinkEntity1006
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1006;

    /// <summary>
    /// 1006请求实体
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 1006应答实体
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }
    }
}
