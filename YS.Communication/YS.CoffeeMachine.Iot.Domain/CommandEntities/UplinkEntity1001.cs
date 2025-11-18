namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 1001.VMC向服务器申请下发服务器时间戳
/// </summary>
public class UplinkEntity1001
{
    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {

    }

    #region 应答类

    /// <summary>
    /// 接收
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// TimeSpValue
        /// </summary>
        public string TimeSpValue { get; set; }
    }
    #endregion 应答类
}