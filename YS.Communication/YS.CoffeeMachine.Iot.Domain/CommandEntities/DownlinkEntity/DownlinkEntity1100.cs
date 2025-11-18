namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using MessagePack;

/// <summary>
/// 1100 设备激活下发生产编号
/// </summary>
[MessagePackObject(true)]
public class DownlinkEntity1100 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1100;

    /// <summary>
    /// 车贴码/生产编号
    /// </summary>
    public string PMid { get; set; }

    /// <summary>
    /// 事务号
    /// </summary>
    public string TransId { get; set; }

    /// <summary>
    /// 私钥
    /// </summary>
    public string PriKey { get; set; }

    /// <summary>
    /// 接收Response
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {

        /// <summary>
        /// 车贴码/生产编号
        /// </summary>
        public string MachineStickerCode { get; set; }

        /// <summary>
        /// 箱体id
        /// </summary>
        public string BoxId { get; set; }
    }
}