namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 7213.获取支付平台相关配置信息
/// </summary>
public class UplinkEntity7213
{

    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 7213;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// 付款方案：AlipayK12、WechatK12、Card
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 支付宝/微信:刷脸uid；刷卡：物理卡号
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 可选备注 如微信的离线刷脸Offline,K12
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 响应
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal? Balance { get; set; }

        /// <summary>
        /// 赠送金额
        /// </summary>
        public decimal? GiveBalance { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 卡状态
        /// </summary>
        public int? CardStatus { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
