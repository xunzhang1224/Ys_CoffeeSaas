namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// 1205.VMC向服务器上报程序信息
/// </summary>
public class UplinkEntity1205
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1205;

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
        /// 程序集。
        /// </summary>
        [Required]
        public List<Release> Releases { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 嵌套
        /// </summary>
        [MessagePackObject(true)]
        public class Release
        {
            /// <summary>
            /// 程序类型
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// 程序名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 版本名称
            /// </summary>
            public string VersionName { get; set; }

            /// <summary>
            /// 版本号
            /// </summary>
            public string Version { get; set; }

            /// <summary>
            /// 程序的标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 附加字段
            /// </summary>
            public string Extra { get; set; }
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
    }
}
