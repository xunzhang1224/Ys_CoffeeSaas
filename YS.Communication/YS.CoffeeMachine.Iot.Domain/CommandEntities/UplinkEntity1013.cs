namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 用于1013号指令的协议实体：VMC向服务器上报属性.
/// </summary>
public class UplinkEntity1013
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1013;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// 属性
        /// </summary>
        public IEnumerable<Attribute> Attributes { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        [MessagePackObject(true)]
        public class Attribute
        {
            /// <summary>
            /// key
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 值
            /// </summary>
            public string Value { get; set; }
        }
    }

    /// <summary>
    /// 接收
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
    }
}