using Aop.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PaymentDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.MarketingActivitysCommands
{
    /// <summary>
    /// 领取优惠劵
    /// </summary>
    /// <param name="campaignId">营销活动id</param>
    /// <param name="userId"></param>
    /// <param name="useType"></param>
    /// <param name="validFrom"></param>
    /// <param name="validTo"></param>
    /// <param name="useDay"></param>
    public record AddCouponCommand(long campaignId, long userId, int useType, DateTime? validFrom, DateTime? validTo, int? useDay) : ICommand<bool>;
    public record UseCouponCommand(long couponId, CreateOrderBaseInput order) : ICommand<decimal>;
    public record ReturnCouponCommand(long couponId) : ICommand<bool>;
    public record EndCouponCommand(List<long> couponIds) : ICommand<bool>;
}
