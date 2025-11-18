namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 1014.VMC向服务器询问能力配置
/// </summary>
public class UplinkEntity1014
{
    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 能力类型
        /// </summary>
        public int CapabilityType { get; set; }

        /// <summary>
        /// 查查询的配置集
        /// </summary>
        public IEnumerable<ConfigureEntity> CapabilityConfigure { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 嵌套
        /// </summary>
        [MessagePackObject(true)]
        public class ConfigureEntity
        {
            /// <summary>
            /// Gets or sets 能力号，参考定义值.
            /// </summary>
            public int Id { get; set; }
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
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 能力类型
        /// </summary>
        public int CapabilityType { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public List<ConfigureEntity> CapabilityConfigure { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 嵌套
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
        #endregion
    }
}
