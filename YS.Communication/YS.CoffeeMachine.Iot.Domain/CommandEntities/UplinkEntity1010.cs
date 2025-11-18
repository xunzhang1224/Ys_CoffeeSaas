namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;
using System.Collections.Generic;

/// <summary>
/// 用于1010号指令的请求实体：VMC向服务器上报能力配置.
/// </summary>
public class UplinkEntity1010
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1010;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// Gets or sets 能力类型（1-硬件 2-软件）.
        /// </summary>
        public int CapabilityType { get; set; }

        /// <summary>
        /// Gets or sets 配置信息集.
        /// </summary>
        public IEnumerable<ConfigureEntity> CapabilityConfigure { get; set; }

        /// <summary>
        /// 表示配置信息的实体.
        /// </summary>
        [MessagePackObject(true)]
        public class ConfigureEntity
        {
            /// <summary>
            /// Gets or sets 能力号，参考定义值.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets 能力名称.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets ot sets 配置内容.
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// Gets or sets ot sets 权限属性.
            /// </summary>
            public int Premission { get; set; }

            /// <summary>
            /// Gets or sets ot sets 数据结构类型.
            /// </summary>
            public int Structure { get; set; }
        }
    }

    /// <summary>
    /// 用于1010号指令的应答实体：VMC向服务器上报能力配置.
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// Gets or sets 能力类型（1-硬件 2-软件）.
        /// </summary>
        public int CapabilityType { get; set; }

        /// <summary>
        /// Gets or sets 配置信息集.
        /// </summary>
        public IEnumerable<ConfigureEntity> CapabilityConfigure { get; set; }

        /// <summary>
        /// 表示配置信息的实体.
        /// </summary>
        [MessagePackObject(true)]
        public class ConfigureEntity
        {
            /// <summary>
            /// Gets or sets 能力号，参考定义值.
            /// </summary>
            public int Id { get; set; }
        }
    }
}