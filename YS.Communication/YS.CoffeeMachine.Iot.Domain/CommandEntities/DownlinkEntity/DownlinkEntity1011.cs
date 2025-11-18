using MessagePack;

using System.Collections.Generic;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

/// <summary>
/// 用于1011号指令的请求实体：服务器向VMC同步能力配置.
/// </summary>
[MessagePackObject(true)]
public class DownlinkEntity1011 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1011;

    #region 公共属性

    /// <summary>
    /// TransId
    /// </summary>
    public string TransId { get; set; }

    /// <summary>
    /// 能力类型（1-硬件 2-软件）.
    /// </summary>
    public int CapabilityType { get; set; }

    /// <summary>
    /// 配置信息集.
    /// </summary>
    public IEnumerable<ConfigureEntity> CapabilityConfigure { get; set; }
    #endregion

    #region 嵌套结构
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
        public int Permission { get; set; }

        /// <summary>
        /// Gets or sets ot sets 数据结构类型.
        /// </summary>
        public int Structure { get; set; }
    }
    #endregion

    #region 响应实体

    /// <summary>
    /// 用于1011号指令的响应实体：服务器向VMC同步能力配置.
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
    #endregion
}