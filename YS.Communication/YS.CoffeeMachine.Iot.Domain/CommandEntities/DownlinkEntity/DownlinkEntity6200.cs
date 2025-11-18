namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using MessagePack;

/// <summary>
/// 6200.远程设置货道信息（远程同步货道）
/// </summary>
[MessagePackObject(true)]
public class DownlinkEntity6200 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 6200;

    #region 公共属性

    /// <summary>
    /// 货柜编号
    /// </summary>
    public int CounterNo { get; set; }

    /// <summary>
    /// 货道总数
    /// </summary>
    public int MaxSlot { get; set; }

    /// <summary>
    /// 货道集合
    /// </summary>
    public IEnumerable<Slot> SlotInfo { get; set; }
    #endregion

    #region 嵌套结构

    /// <summary>
    /// 货道信息
    /// </summary>
    [MessagePackObject(true)]
    public class Slot
    {
        /// <summary>
        /// 货道编号。
        /// </summary>
        public int SlotNo { get; set; }

        ///// <summary>
        ///// 货道层号
        ///// </summary>
        //public int LayerNo { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 容量
        /// </summary>
        public decimal Capacity { get; set; }

        /// <summary>
        /// 现存
        /// </summary>
        public decimal Stock { get; set; }

        ///// <summary>
        ///// 货道光检开关0 关闭 1 开启
        ///// </summary>
        //public int DropSensor { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        ///// <summary>
        ///// 商品说明
        ///// </summary>
        //public string Explain { get; set; }

        /// <summary>
        /// 商品图片URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string SKU { get; set; }

        ///// <summary>
        ///// 加热时间
        ///// </summary>
        //public int HotTime { get; set; }

        /// <summary>
        /// 状态:正常=1；故障=2；停用=3；缺货=4 不存在 = 255
        /// </summary>
        public int Status { get; set; }

        ///// <summary>
        ///// 扩展字段
        ///// </summary>
        //public string Extra { get; set; }

        /// <summary>
        /// 商品过期时间戳，以秒为单位。如果为0，则是没有过期时间
        /// </summary>
        public long ExpireTimeStamp { get; set; }
    }
    #endregion

    #region 应答实体

    /// <summary>
    /// 接收
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {

        /// <summary>
        /// 货柜编号
        /// </summary>
        public int CounterNo { get; set; }

        /// <summary>
        /// 货道总数
        /// </summary>
        public int MaxSlot { get; set; }

        /// <summary>
        /// 货道集合
        /// </summary>
        public IEnumerable<Slot> SlotInfo { get; set; }

        /// <summary>
        /// 货道
        /// </summary>
        [MessagePackObject(true)]
        public class Slot
        {
            /// <summary>
            /// 货道编号。
            /// </summary>
            public int SlotNo { get; set; }
        }
    }
    #endregion 应答实体
}