using MessagePack;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;

/// <summary>
/// 1204.VMC向服务端查询是否有更新版本
/// </summary>
public class UplinkEntity1204
{
    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// Releases
        /// </summary>
        public IEnumerable<Release> Releases { get; set; }

        /// <summary>
        /// Release
        /// </summary>
        [MessagePackObject(true)]
        public class Release
        {
            /// <summary>
            /// 程序类型; 1.安卓应用程序，2.单片机程序，3.安卓固件程序
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// 程序名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 当前版本名称
            /// </summary>
            public string VersionName { get; set; }
        }
    }

    /// <summary>
    /// 接收
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// Releases
        /// </summary>
        public IEnumerable<Release> Releases { get; set; }

        /// <summary>
        /// Releases
        /// </summary>
        [MessagePackObject(true)]
        public class Release
        {
            /// <summary>
            /// 程序类型; 1.安卓应用程序，2.单片机程序，3.安卓固件程序
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// 程序名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 可更新的版本名称
            /// </summary>
            public string VersionName { get; set; }

            /// <summary>
            /// 版本类型 1.公测版 2.内测版 3.稳定版
            /// </summary>
            public int VersionType { get; set; }

            /// <summary>
            /// 版本更新说明
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// 下载地址
            /// </summary>
            public string DownLoadUrl { get; set; }
        }
    }
}
