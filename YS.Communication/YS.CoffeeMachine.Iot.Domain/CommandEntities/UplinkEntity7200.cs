namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 7200.初始化VMC的刷脸识别器
/// </summary>
public class UplinkEntity7200
{

    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 7200;

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
        /// 刷脸SDK提供者
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 可选备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Parameter { get; set; }
    }

    /// <summary>
    /// 响应
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 状态 0.成功 1.失败
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
