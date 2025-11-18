namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 2000.心跳（机器状态, 网络状态, 版本号等参数上报）
/// </summary>
public class UplinkEntity2000 : BaseCmd
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 2000;

    /// <summary>
    /// 1000请求实体
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// 温度: 温度正常显示温度值, 异常显示”—”
        /// </summary>
        public string Temp { get; set; }

        /// <summary>
        /// 硬件版本号
        /// </summary>
        public string HV { get; set; }

        /// <summary>
        /// 驱动版本号
        /// </summary>
        public string BV { get; set; }

        /// <summary>
        /// 软件版本号
        /// </summary>
        public string SV { get; set; }

        /// <summary>
        /// 机器型号
        /// </summary>
        public string MV { get; set; }

        /// <summary>
        /// ICCID
        /// </summary>
        public string ICCID { get; set; }

        /// <summary>
        /// IMEI
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// LBS
        /// </summary>
        public string LBS { get; set; }

        /// <summary>
        /// 运行内存
        /// </summary>
        public int? UseRam { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        public int Volume { get; set; } = 0;

        /// <summary>
        /// 0-33信号强度
        /// </summary>
        public int? Signal { get; set; }

        /// <summary>
        /// 门状态 0关 1开
        /// </summary>
        public int? Door { get; set; }

        /// <summary>
        /// 锁状态 0关 1开
        /// </summary>
        public int? Lock { get; set; }
        /// <summary>
        /// 工作状态 0:正常 1:定时开关机关机时间 2:温度超限锁机 3:离线记录满锁机 4:内存异常锁机 5: 连续故障锁机
        /// </summary>
        public int? Service { get; set; }
        /// <summary>
        /// 售货状态
        /// </summary>
        public int? IsOpen { get; set; }

        /// <summary>
        /// 机器密码
        /// </summary>
        public string MchPswd { get; set; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        public string AdminPswd { get; set; }

        /// <summary>
        /// 补货员密码
        /// </summary>
        public string ReplenishPswd { get; set; }

        /// <summary>
        /// Extra:视觉柜自定义Extra文档
        /// </summary>
        public string Extra { get; set; }

        /// <summary>
        /// VersionNameApp
        /// </summary>
        public string VersionNameApp { get; set; }
    }
    #region 应答类

    /// <summary>
    /// 接收
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
    }

    #endregion 应答类
}