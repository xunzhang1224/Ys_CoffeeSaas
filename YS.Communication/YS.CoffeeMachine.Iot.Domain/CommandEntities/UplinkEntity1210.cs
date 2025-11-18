namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 1210.VMC向服务器请求HttpApi调用凭证
/// </summary>
public class UplinkEntity1210
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1210;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
    }

    /// <summary>
    /// 接收
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 过期时间。单位：秒
        /// </summary>
        public int Expires { get; set; }
    }
}
