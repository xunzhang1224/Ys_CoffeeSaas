using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 优惠券使用状态枚举
    /// </summary>
    public enum CouponUsageStatus
    {
        /// <summary>
        /// 已使用
        /// - 优惠券已在订单中成功使用
        /// - 订单处于待支付或已支付状态
        /// </summary>
        Used = 1,

        /// <summary>
        /// 已取消
        /// - 订单取消导致优惠券使用被取消
        /// - 优惠券恢复为未使用状态
        /// </summary>
        Canceled = 2,

        /// <summary>
        /// 已退款
        /// - 订单退款导致优惠券使用记录标记为退款
        /// - 优惠券通常不会恢复（根据业务规则）
        /// </summary>
        Refunded = 3,

        /// <summary>
        /// 已过期
        /// - 使用记录因超时等原因过期
        /// - 通常用于预占用的优惠券
        /// </summary>
        Expired = 4
    }
}
