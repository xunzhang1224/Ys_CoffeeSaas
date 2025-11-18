namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using MessagePack;

/// <summary>
/// 3202.购物车出货命令应答确认
/// </summary>
public class DownlinkEntity3202
{
    /// <summary>
    /// 请求体
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
    }

    /// <summary>
    /// 接收体
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
    }
}
