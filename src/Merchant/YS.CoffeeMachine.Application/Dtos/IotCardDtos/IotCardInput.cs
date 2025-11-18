using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Dtos.IotCardDtos
{
    /// <summary>
    /// 物联网卡分页列表查询输入参数
    /// </summary>
    public class IotCardQueryInput : QueryRequest
    {
        /// <summary>
        /// 卡号ICCID
        /// </summary>
        public string? iccid { get; set; } = null;

        /// <summary>
        /// 设备Id
        /// </summary>
        public string? code { get; set; } = null;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? deviceName { get; set; } = null;
    }

    /// <summary>
    /// 流量卡账号登录输入参数
    /// </summary>
    public class LotCardAccountLoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; } = "16666666666";

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = "123456";
    }

    /// <summary>
    /// 流量卡输入参数
    /// </summary>
    public class IotCardInput
    {
        /// <summary>
        /// 卡号ICCID
        /// </summary>
        public string Iccid { get; set; }

        /// <summary>
        /// 策略Id
        /// </summary>
        public string PolicyID { get; set; }
    }

    /// <summary>
    /// 基础套餐输入参数
    /// </summary>
    public class BestPackageInput
    {
        /// <summary>
        /// 卡号ICCID
        /// </summary>
        public string Iccid { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string account { get; set; } = "";

        /// <summary>
        /// accountFrom
        /// </summary>
        public string accountFrom { get; set; } = "";
    }

    /// <summary>
    /// 用户充值续订
    /// </summary>
    public class UserRechargeRenewal
    {
        /// <summary>
        /// 卡号ICCID
        /// </summary>
        public string ICCID { get; set; }

        /// <summary>
        /// 续订类型
        /// </summary>
        public string RenewalType { get; set; }

        /// <summary>
        /// 续订价格
        /// </summary>
        public decimal RenewalPrice { get; set; }

        /// <summary>
        /// 续订流量
        /// </summary>
        public int Flow { get; set; }

        /// <summary>
        /// 续订流量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 源头
        /// </summary>
        public int Origin { get; set; }

        /// <summary>
        /// 充值用户账号
        /// </summary>
        public string RechargeUserAccount { get; set; }

        /// <summary>
        /// 充值用户名称
        /// </summary>
        public string RechargeUserName { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 机器名称
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Mid
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNO { get; set; }

        /// <summary>
        /// 设备SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime OrderCreateTime { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 充值类型
        /// </summary>
        public int RechargeType { get; set; }
        /// <summary>
        /// 策略
        /// </summary>
        public string PolicyCode { get; set; }
    }
}