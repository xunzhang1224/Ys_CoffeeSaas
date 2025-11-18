using MessagePack;

using System.Collections.Generic;

namespace YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO;

/// <summary>
/// 出货订单。
/// </summary>
public class Downlink6200
{
    /// <summary>
    /// 机器编号。
    /// </summary>
    public string VendCode { get; set; }

    /// <summary>
    /// 事务号。
    /// </summary>
    public long TransId { get; set; }

    /// <summary>
    /// 货柜编号。
    /// </summary>
    public int CounterNo { get; set; }

    /// <summary>
    /// 货道总数。
    /// </summary>
    public int SlotTotal { get; set; }

    /// <summary>
    /// 货道集合。
    /// </summary>
    public List<Slot> Slots { get; set; }

    /// <summary>
    /// 货道信息。
    /// </summary>
    public class Slot
    {
        /// <summary>
        /// 货道编号。
        /// </summary>
        public int SlotNo { get; set; }

        /// <summary>
        /// 层编号。
        /// </summary>
        public int LayerNo { get; set; }

        /// <summary>
        /// 价格。
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 货道容量
        /// </summary>
        public decimal Capacity { get; set; }

        /// <summary>
        /// 货道库存。
        /// </summary>
        public decimal Inventory { get; set; }

        /// <summary>
        /// 货道光检开关0 关闭 1 开启
        /// </summary>
        public int DropSensor { get; set; }

        /// <summary>
        /// 商品名称。
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 商品描述。
        /// </summary>
        public string ItemDescription { get; set; }

        /// <summary>
        /// 商品图片。
        /// </summary>
        public string ItemImageUrl { get; set; }

        /// <summary>
        /// 加热时间。
        /// </summary>
        public int HotTime { get; set; } = 0;

        /// <summary>
        /// 商品编码SKU
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 状态:正常=1；故障=2；停用=3；缺货=4 不存在 = 255
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 扩展字段JSON
        /// </summary>
        public string Extra { get; set; }

        /// <summary>
        /// 商品过期时间戳，以秒为单位。如果为0，则是没有过期时间
        /// </summary>
        public long ExpireTimeStamp { get; set; } = 0;
    }
}
