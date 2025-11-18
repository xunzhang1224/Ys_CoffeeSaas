namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 1000.初始 VMC向服务器申请 ID 和 私钥密钥
/// </summary>
public class UplinkEntity1000
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1000;

    /// <summary>
    /// 1000请求实体
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// IMEI
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// PubKey
        /// </summary>
        public string PubKey { get; set; }
    }

    #region 应答类

    /// <summary>
    /// 接收
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// 永久编号
        /// </summary>
        public string PMid { get; set; }
        /// <summary>
        /// 设备私钥
        /// </summary>
        public string PriKey { get; set; } = string.Empty;

    }

    #endregion 应答类
}