namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 7221.订单附件上报
/// </summary>
public class UplinkEntity7221
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 7221;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 货柜编号
        /// </summary>
        public int CounterNo { get; set; }

        /// <summary>
        /// 订单附件
        /// </summary>
        public IEnumerable<Attachment> Attachments { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 嵌套
        /// </summary>
        [MessagePackObject(true)]
        public class Attachment
        {
            /// <summary>
            /// 序号
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// 文件路径
            /// </summary>
            public string FilePath { get; set; }

            /// <summary>
            /// 文件类型
            /// </summary>
            public string FileType { get; set; }

            /// <summary>
            /// 附加参数
            /// </summary>
            public string Argument { get; set; }
        }
        #endregion
    }

    /// <summary>
    /// 接收
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 货柜编号
        /// </summary>
        public int CounterNo { get; set; }
    }
}
