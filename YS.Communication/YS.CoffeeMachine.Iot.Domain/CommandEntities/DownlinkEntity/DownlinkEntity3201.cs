namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using MessagePack;

/// <summary>
/// 3201.远程出货指令的实体
/// </summary>
[MessagePackObject(true)]
public class DownlinkEntity3201 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 3201;

    #region 公共属性

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }

    /// <summary>
    /// 订单出货明细
    /// </summary>
    public List<Order> Orders { get; set; }
    #endregion

    #region 嵌套结构

    /// <summary>
    /// 数据结构
    /// </summary>
    [MessagePackObject(true)]
    public class Order
    {
        /// <summary>
        /// 子订单号
        /// </summary>
        public string SubOrderNo { get; set; }

        /// <summary>
        /// 出货编号
        /// </summary>
        public string DeliveryId { get; set; }

        /// <summary>
        /// 货道号
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; } = string.Empty;

        /// <summary>
        /// 附件参数
        /// </summary>
        public string Extra { get; set; } = string.Empty;

        /// <summary>
        /// 货柜编号
        /// </summary>
        public int CounterNo { get; set; }
    }
    #endregion

    #region 应答实体

    /// <summary>
    /// 应答实体
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 是否可执行出货
        /// </summary>
        public bool Accept { get; set; }

        /// <summary>
        /// 无法执行出货的原因
        /// </summary>
        public string Description { get; set; }
    }
    #endregion
}