using System.ComponentModel.DataAnnotations;
using YS.CoffeeMachine.Application.Dtos.PaymentDtos;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayPaymentDtos
{
    /// <summary>
    /// 微信扫码支付-创建订单入参
    /// </summary>
    public class CreateNativeOrderResponse
    {
        /// <summary>
        /// 设备MID
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 设备本地订单编号
        /// </summary>
        public string BizNo { get; set; }

        /// <summary>
        /// 付款方案
        /// </summary>
        [Required]
        public string Provider { get; private set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        [Required]
        public long PayTimeSp { get; private set; }

        /// <summary>
        /// 支付平台商户进件表的唯一标识（Merchant表的Id）
        /// </summary>
        public long MerchantId { get; set; }

        /// <summary>
        /// 实际支付金额（单位:元）
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 订单自定义内容(业务平台定义，不能超过200个字)
        /// </summary>
        public string CustomContent { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public OrderPaymentTypeEnum OrderPaymentType { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }

    /// <summary>
    /// 微信扫码支付-创建订单出参
    /// </summary>
    public class CreateNativeOrderOutput
    {
        /// <summary>
        /// 系统订单号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 交易订单号
        /// PaymentMode字段等于“在线支付”
        ///    1 .支付宝jsapi支付：OrderNo为支付宝交易号
        ///    2.微信jsapi支付：OrderNo为微信支付订单号
        /// PaymentMode字段等于“离线支付”：OrderNo就为（设备的生产编号+“_”+安卓订单号）
        /// </summary>
        public string ThirdOrderNo { get; set; }

        /// <summary>
        /// 动态对象，具体返回字段，去看文档
        /// </summary>
        public dynamic Content { get; set; }

        /// <summary>
        /// 二维码信息
        /// </summary>
        public string QrCodeData { get; set; }
    }
}