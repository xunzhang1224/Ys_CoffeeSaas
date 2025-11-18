using System.ComponentModel.DataAnnotations;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Order
{
    /// <summary>
    /// 订单物料使用
    /// </summary>
    public class OrderDetaliMaterial : BaseEntity
    {
        /// <summary>
        /// 订单详情id
        /// </summary>
        [Required]
        public long OrderDetailsId { get; private set; }

        /// <summary>
        /// 物料表id
        /// </summary>
        [Required]
        public long DeviceMaterialInfoId { get; private set; }

        /// <summary>
        /// 使用量
        /// </summary>
        public int Value { get; private set; } = 0;

        /// <summary>
        /// a
        /// </summary>
        public OrderDetails OrderDetails { get; private set; }

        /// <summary>
        /// a
        /// </summary>
        protected OrderDetaliMaterial() { }

        /// <summary>
        /// a1
        /// </summary>
        /// <param name="orderDetailsId"></param>
        /// <param name="deviceMaterialInfoId"></param>
        /// <param name="value"></param>
        public OrderDetaliMaterial(long deviceMaterialInfoId, int value)
        {
            DeviceMaterialInfoId = deviceMaterialInfoId;
            Value = value;
        }
    }
}