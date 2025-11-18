namespace YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO
{
    using System;
    using MessagePack;

    /// <summary>
    /// 微信支付模拟数据传输对象。
    /// 用于在IoT模块中模拟微信支付完成后的回调信息。
    /// </summary>
    [MessagePackObject(true)]
    public class MockWechatPaymentDto
    {
        /// <summary>
        /// 获取或设置售货机编号（设备唯一标识）。
        /// </summary>
        public string VendCode { get; set; }

        /// <summary>
        /// 获取或设置交易流水号（订单编号）。
        /// </summary>
        public string Tradeno { get; set; }

        /// <summary>
        /// 获取或设置支付时间。
        /// </summary>
        public DateTime Paytime { get; set; }

        /// <summary>
        /// 获取或设置微信支付系统中的交易ID（用于对账）。
        /// </summary>
        public string Transactionid { get; set; }

        /// <summary>
        /// 获取或设置售货机ID（业务系统内唯一标识）。
        /// </summary>
        public string VendId { get; set; }
    }
}