namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 5204.VMC向服务器上报错误
/// </summary>
public class UplinkEntity5204
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 5204;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// a
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// CounterNo
        /// </summary>
        public int CounterNo { get; set; }

        /// <summary>
        /// CodeType
        /// </summary>
        public int CodeType { get; set; }

        /// <summary>
        /// SlotInfo
        /// </summary>
        public IEnumerable<SlotData> Info { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 嵌套
        /// </summary>
        [MessagePackObject(true)]
        public class SlotData
        {
            /// <summary>
            /// 货道号。非货道故障为0
            /// </summary>
            public int Slot { get; set; }

            /// <summary>
            /// 故障代码
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// 故障描述
            /// </summary>
            public string Desc { get; set; }
        }
        #endregion
    }

    /// <summary>
    /// 响应
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
    }
}