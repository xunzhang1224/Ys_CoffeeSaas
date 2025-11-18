using MessagePack;

using System.Collections.Generic;

namespace YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO;

/// <summary>
/// 出货订单。
/// </summary>
public class Downlink3201
{
    /// <summary>
    /// 下发出货订单。
    /// </summary>
    [MessagePackObject(true)]
    public class Request
    {
        /// <summary>
        /// 机器编号。
        /// </summary>
        public string VendCode { get; set; }

        /// <summary>
        /// 事务号。
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 订单号。
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 出货订单明细。
        /// </summary>
        public List<Detail> Orders { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 出货订单明细。
        /// </summary>
        [MessagePackObject(true)]
        public class Detail
        {
            /// <summary>
            /// 货道编号。
            /// </summary>
            public int SlotNo { get; set; }

            /// <summary>
            /// 商品编码。
            /// </summary>
            public string SKU { get; set; }

            /// <summary>
            /// 支付金额。
            /// </summary>
            public decimal Amount { get; set; }

            /// <summary>
            /// 支付类型。
            /// </summary>
            public string PayType { get; set; }

            /// <summary>
            /// 子订单号（订单明细标识）。
            /// </summary>
            public string SubOrderNo { get; set; }

            /// <summary>
            /// 出货记录编号。
            /// </summary>
            public string DeliveryId { get; set; }

            /// <summary>
            /// 扩展字段Json字符串。
            /// </summary>
            public string Extra { get; set; }

            /// <summary>
            /// 货柜编号。
            /// </summary>
            public int CounterNo { get; set; }
        }
        #endregion
    }
}
