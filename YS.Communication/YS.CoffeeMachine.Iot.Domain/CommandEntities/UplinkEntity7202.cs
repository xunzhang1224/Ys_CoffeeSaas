namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 7202.用于获取支付用户系统信息
/// </summary>
public class UplinkEntity7202
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 7202;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// 付款方案：Card、Wechat、Alipay
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 付款账户Id
        /// </summary>
        public string AccountId { get; set; }
    }

    /// <summary>
    /// 响应
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// 付款方案：Card、Wechat、Alipay
        /// </summary>
        public string Provider { get; set; }

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
