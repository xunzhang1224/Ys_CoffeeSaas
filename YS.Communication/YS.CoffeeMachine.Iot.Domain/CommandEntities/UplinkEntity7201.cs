namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 7201.获取支付平台相关配置信息
/// </summary>
public class UplinkEntity7201
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 7201;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {

        /// <summary>
        /// 支付提供商
        /// </summary>
        public string Provider { get; set; }

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
        /// 返回结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }

    #region 嵌套结构

    /// <summary>
    /// 嵌套
    /// </summary>
    public class BasePaymentConfig
    {
        /// <summary>
        /// Appid
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 商户ID
        /// </summary>
        public string MchId { get; set; }

        /// <summary>
        /// 服务商ID
        /// </summary>
        public string SpMchId { get; set; }

        /// <summary>
        /// 门店编码
        /// </summary>
        public string StoreCode { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MchName { get; set; }

        /// <summary>
        /// 服务商名称
        /// </summary>
        public string SpMchName { get; set; }

        /// <summary>
        /// 学校库ID
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 支付PID
        /// </summary>
        public string PayPid { get; set; }
    }
    #endregion
}
