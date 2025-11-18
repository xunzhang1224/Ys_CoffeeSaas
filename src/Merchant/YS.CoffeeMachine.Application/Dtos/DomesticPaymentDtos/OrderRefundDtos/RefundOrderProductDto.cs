namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos
{
    /// <summary>
    /// 订单退款
    /// </summary>
    public class RefundOrderProductDto
    {
        /// <summary>
        /// 子订单号
        /// </summary>
        public string OrderProductId { get; set; }

        /// <summary>
        /// 货柜编号
        /// </summary>
        public int? CounterNo { get; set; }

        /// <summary>
        /// 货道编号
        /// </summary>
        public int? SlotNo { get; set; }

        /// <summary>
        /// 饮品SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// 商品表的Id（Product表的Id）
        /// </summary>
        public long? ProductId { get; set; } = null;

        /// <summary>
        /// 商品条码（Product表的BarCode）
        /// </summary>
        public string? BarCode { get; set; } = null;

        /// <summary>
        /// 商品名称（Product表的Name）
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 商品主图（Product表的MainImage）
        /// </summary>
        public string? MainImage { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal PayAmount { get; set; } = 0;

        /// <summary>
        /// 购买数量
        /// </summary>
        public int PurchaseQuantity { get; set; } = 0;

        /// <summary>
        /// 实际出货的商品数量
        /// </summary>
        public int ShipmentQuantity { get; set; } = 0;

        /// <summary>
        /// 可退金额
        /// </summary>
        public decimal RefundableAmount { get; set; } = 0;

        /// <summary>
        /// 单价
        /// </summary>
        public decimal ItemPricing { get; set; } = 0;
    }
}