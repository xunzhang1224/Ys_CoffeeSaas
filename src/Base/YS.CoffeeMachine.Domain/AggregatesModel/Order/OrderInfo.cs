using System.ComponentModel.DataAnnotations;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.EnterpriseDeviceBaseEntity;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Order
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderInfo : EDBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 机器
        /// </summary>
        [Required]
        public long DeviceBaseId { get; private set; }

        /// <summary>
        /// 系统订单编号
        /// </summary>
        [Required]
        public string Code { get; private set; }

        /// <summary>
        /// 机器本地订单编号
        /// </summary>
        [Required]
        public string BizNo { get; private set; }

        /// <summary>
        /// 第三方订单ID
        /// </summary>
        public string? ThirdOrderId { get; private set; } = null;

        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string? ThirdOrderNo { get; private set; } = null;

        /// <summary>
        /// 订单自定义内容
        /// </summary>
        public string? CustomContent { get; private set; } = null;

        /// <summary>
        /// 消费金额(元)
        /// </summary>
        public decimal Amount { get; private set; } = 0;

        /// <summary>
        /// 货币代码
        /// </summary>
        [Required]
        public string CurrencyCode { get; private set; } = "CNY";

        /// <summary>
        /// 货币符号
        /// </summary>
        public string CurrencySymbol { get; private set; }

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
        /// 支付成功时间
        /// </summary>
        public DateTime? PayDateTime { get; private set; }

        /// <summary>
        /// 出货结果
        /// </summary>
        public OrderShipmentResult ShipmentResult { get; private set; } = OrderShipmentResult.NotShipped;

        /// <summary>
        /// 支付结果
        /// </summary>
        public OrderSaleResult SaleResult { get; private set; } = OrderSaleResult.NotPay;

        /// <summary>
        /// 错误码
        /// </summary>
        public string? ErrCode { get; private set; } = null;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string? PayErrMsg { get; private set; } = null;

        /// <summary>
        /// 支付时间Utc
        /// </summary>
        public DateTime PayTime => PayTimeSp == 0 ? DateTime.MinValue : DateTimeOffset.FromUnixTimeMilliseconds(PayTimeSp).UtcDateTime;

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal ReturnAmount { get; private set; } = 0;

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderTypeEnum? OrderType { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        public List<OrderDetails> OrderDetails { get; private set; }

        #region 微信支付宝支付相关字段

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatusEnum? OrderStatus { get; private set; } = null;

        /// <summary>
        /// 微信/支付宝支付的商户Id（M_PaymentMethod表的MerchantId）
        /// </summary>
        /// <remarks>
        /// <para>微信支付：特约商户的商户号sub_mchid</para>
        /// <para>支付宝支付：二级商户Id smid</para>
        /// </remarks>
        public string? PaymentMerchantId { get; private set; } = null;

        /// <summary>
        /// 系统支付方式表的Id（SystemPaymentMethod表的Id）
        /// </summary>
        public long? SystemPaymentMethodId { get; private set; } = null;

        /// <summary>
        /// 支付的服务商表（SystemPaymentServiceProvider表的Id）
        /// </summary>
        public long? SystemPaymentServiceProviderId { get; private set; } = null;

        /// <summary>
        /// 订单支付方式
        /// </summary>
        public OrderPaymentTypeEnum? OrderPaymentType { get; private set; } = null;
        #endregion

        /// <summary>
        /// 保护构造
        /// </summary>
        protected OrderInfo() { }

        /// <summary>
        /// 订单上报
        /// </summary>
        /// <param name="orderType"></param>
        /// <param name="deviceBaseId"></param>
        /// <param name="bizNo"></param>
        /// <param name="amount"></param>
        /// <param name="provider"></param>
        /// <param name="payTimeSp"></param>
        /// <param name="currencyCode"></param>
        /// <param name="enterpriseinfoId"></param>
        /// <param name="code"></param>
        /// <param name="orderDetails"></param>
        public OrderInfo(OrderTypeEnum? orderType, long deviceBaseId, string bizNo, decimal amount, string provider, long payTimeSp, string currencyCode, string currencySymbol, long enterpriseinfoId, string code, List<OrderDetails> orderDetails)
        {
            OrderType = orderType;
            DeviceBaseId = deviceBaseId;
            BizNo = bizNo;
            Amount = amount;
            Code = code;
            Provider = provider;
            PayTimeSp = payTimeSp;
            EnterpriseinfoId = enterpriseinfoId;
            OrderDetails = orderDetails;
            CurrencyCode = currencyCode;
            CurrencySymbol = currencySymbol;
        }

        /// <summary>
        /// 设置租户Id
        /// </summary>
        /// <param name="id"></param>
        public void SetEnterpriseinfoId(long id)
        {
            EnterpriseinfoId = id;
        }

        /// <summary>
        /// 设置订单支付方式
        /// </summary>
        /// <param name="orderPaymentType"></param>
        public void SetOrderPaymentType(OrderPaymentTypeEnum? orderPaymentType)
        {
            OrderPaymentType = orderPaymentType;
        }

        /// <summary>
        /// 设置出货结果
        /// </summary>
        /// <param name="shipmentResult"></param>
        public void SetShipmentResult(OrderShipmentResult shipmentResult)
        {
            ShipmentResult = shipmentResult;
        }

        /// <summary>
        /// 设置支付结果
        /// </summary>
        /// <param name="saleResult"></param>
        public void SetSaleResult(OrderSaleResult saleResult)
        {
            SaleResult = saleResult;
        }

        /// <summary>
        /// 设置第三方订单号
        /// </summary>
        /// <param name="thirdOrderId"></param>
        public void SetThirdOrderId(string thirdOrderId)
        {
            ThirdOrderId = thirdOrderId;
            ThirdOrderNo = thirdOrderId;
        }

        /// <summary>
        /// 设置订单支付状态
        /// </summary>
        /// <param name="orderStatus"></param>
        public void SetOrderStatus(OrderStatusEnum orderStatus)
        {
            OrderStatus = orderStatus;
        }

        /// <summary>
        /// 设置支付时间
        /// </summary>
        /// <param name="payTimeSp"></param>
        /// <param name="dateTime"></param>
        public void SetPayTimeSp(long payTimeSp, DateTime? dateTime)
        {
            PayTimeSp = payTimeSp;
            PayDateTime = dateTime;
        }

        /// <summary>
        /// 设置支付错误信息
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="payErrMsg"></param>
        public void SetPayErrorInfo(string? errCode, string? payErrMsg)
        {
            ErrCode = errCode;
            PayErrMsg = payErrMsg;
        }

        /// <summary>
        /// 设置退款金额
        /// </summary>
        /// <param name="returnAmount"></param>
        public void SetReturnAmount(decimal returnAmount)
        {
            ReturnAmount = returnAmount;
        }

        /// <summary>
        /// 设置出货结果
        /// </summary>
        public void SetShipmentResults()
        {
            if (this.OrderDetails == null || !this.OrderDetails.Any())
            {
                ShipmentResult = OrderShipmentResult.Fail;
                return;
            }

            var totalCount = this.OrderDetails.Count;
            var successCount = this.OrderDetails.Count(d => d.Result == 1); // 出货成功
            var failCount = this.OrderDetails.Count(d => d.Result == 0); // 出货失败

            if (successCount == totalCount)
            {
                // 所有订单详情都出货成功
                ShipmentResult = OrderShipmentResult.Success;
            }
            else if (failCount == totalCount)
            {
                // 所有订单详情都出货失败
                ShipmentResult = OrderShipmentResult.Fail;
            }
            else
            {
                // 部分成功部分失败
                ShipmentResult = OrderShipmentResult.Part;
            }
        }

        /// <summary>
        /// 设置线上支付信息
        /// </summary>
        /// <param name="paymentMerchantId"></param>
        /// <param name="systemPaymentMethodId"></param>
        /// <param name="systemPaymentServiceProviderId"></param>
        public void SetPaymentInfo(string paymentMerchantId, long? systemPaymentMethodId = 0, long? systemPaymentServiceProviderId = 0)
        {
            PaymentMerchantId = paymentMerchantId;
            SystemPaymentMethodId = systemPaymentMethodId;
            SystemPaymentServiceProviderId = systemPaymentServiceProviderId;
        }
    }
}