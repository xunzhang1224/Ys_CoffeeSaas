using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 优惠券分发方式枚举
    /// </summary>
    public enum DistributionMethod
    {
        /// <summary>手动领取 - 用户主动领取</summary>
        ManualClaim = 1,

        /// <summary>自动发放 - 系统自动发放给符合条件的用户</summary>
        AutoDistribution = 2,

        /// <summary>兑换码 - 通过兑换码领取</summary>
        RedemptionCode = 3,

        /// <summary>活动赠送 - 参与活动后赠送</summary>
        EventReward = 4,

        /// <summary>邀请奖励 - 邀请好友获得</summary>
        InvitationReward = 5,

        /// <summary>注册赠送 - 新用户注册赠送</summary>
        RegistrationReward = 6,

        /// <summary>消费返券 - 消费后返券</summary>
        PurchaseReward = 7
    }
}
