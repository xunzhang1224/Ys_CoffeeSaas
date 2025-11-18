using YS.CoffeeMachine.Domain.AggregatesModel.Basics.EnterpriseDeviceBaseEntity;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Order
{
    /// <summary>
    /// 订单退款
    /// </summary>
    public class OrderRefund : EDBaseEntity, IAggregateRoot
    {

        /// <summary>
        /// 主订单号（Order表的OrderId字段）
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 子订单号（OrderDetailId表的Id字段）
        /// </summary>
        public string OrderDetailId { get; set; }

        /// <summary>
        /// 退款的交易订单号（支付平台OrderRefund表的Id）
        /// </summary>
        public string RefundOrderNo { get; set; }

        /// <summary>
        /// 饮品SKU
        /// </summary>
        public string? ItemCode { get; set; } = null;

        /// <summary>
        /// 商品表的Id（Product表的Id）
        /// </summary>
        public long? ProductId { get; set; } = null;

        /// <summary>
        /// 商品条码（Product表的BarCode）
        /// </summary>
        public string? BarCode { get; set; } = null;

        /// <summary>
        /// 商品名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 商品主图（Product表的MainImage）
        /// </summary>
        public string? MainImage { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; } = 0;

        /// <summary>
        /// Desc:退款原因
        /// Default:
        /// Nullable:True
        /// </summary>
        public string? RefundReason { get; set; }

        /// <summary>
        /// 退款状态（枚举类型：RefundStatusEnum）
        /// </summary>
        public RefundStatusEnum RefundStatus { get; set; } = RefundStatusEnum.Success;

        /// <summary>
        /// 处理方式（枚举类型：HandlingMethodEnum）
        /// </summary>
        public HandlingMethodEnum HandlingMethod { get; set; } = HandlingMethodEnum.FullRefund;

        /// <summary>
        /// 订单创建时间（Order表的OrderCreatedOnUtc，一定要保持一致）
        /// </summary>
        public DateTime OrderCreatedOnUtc { get; set; }

        /// <summary>
        /// 退款发起时间
        /// </summary>
        public DateTime InitiationTime { get; set; }

        /// <summary>
        /// 退款成功时间
        /// </summary>
        public DateTime? SuccessTime { get; set; }
    }
}
