namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using MessagePack;

/// <summary>
/// 1203.服务器向VMC下发 远程升级指令
/// </summary>
[MessagePackObject(true)]
public class DownlinkEntity1203 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 1203;

    /// <summary>
    /// a
    /// </summary>
    public string TransId { get; set; }

    /// <summary>
    /// 程序集。
    /// </summary>
    public Data Releases { get; set; }

    /// <summary>
    /// 数据结构
    /// </summary>
    [MessagePackObject(true)]
    public class Data
    {
        /// <summary>
        /// 程序类型 1.安卓程序 2.单片机程序 3.安卓固件程序
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 程序名称。
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        public string VersionName { get; set; }

        /// <summary>
        /// 版本类型 1=公测版 2=内测版 3=稳定版
        /// </summary>
        public int VersionType { get; set; }

        /// <summary>
        /// 驱动板序号。当Type为2的时候使用
        /// </summary>
        public string DriveIndex { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownLoadUrl { get; set; }

        /// <summary>
        /// 柜号,默认0主柜。
        /// </summary>
        public int CounterNo { get; set; }

        /// <summary>
        /// 是否强制升级(0:否 1:是,默认值0);强制升级跳过业务逻辑直接升级
        /// </summary>
        public int Forced { get; set; }

        /// <summary>
        /// 是否可降版本(0:否 1:是，默认值0)
        /// </summary>
        public int IsDemote { get; set; }
    }

    /// <summary>
    /// 接收
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// a
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 状态,1:接收成功,2:升级成功,3:升级失败
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Description { get; set; }
    }
}