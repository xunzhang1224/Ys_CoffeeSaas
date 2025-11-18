using Aop.Api.Domain;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using YS.CoffeeMachine.Application.Commands.MarketingActivitysCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.Pay.SDK.Top;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.MarketingActivitysCommandHandlers
{
    /// <summary>
    /// 领取优惠劵
    /// </summary>
    /// <param name="_db"></param>
    public class AddCouponCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<AddCouponCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(AddCouponCommand request, CancellationToken cancellationToken)
        {
            var promotion = await _db.Promotion.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == request.campaignId);
            if (promotion == null) return false;
            promotion.GenerateCoupon(request.userId, request.useType, request.validFrom, request.validTo, request.useDay);
            return true;
        }
    }

    /// <summary>
    /// 使用优惠劵
    /// </summary>
    /// <param name="_db"></param>
    public class UseCouponCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UseCouponCommand, decimal>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<decimal> Handle(UseCouponCommand request, CancellationToken cancellationToken)
        {
            decimal disprice = 0;
            var models = await _db.Coupon.Include(x => x.Promotion).FirstOrDefaultAsync(x => x.Id == request.couponId);
            var promotion = models.Promotion;
            if (promotion.ValidateCouponUsage(request.couponId, request.order.PayAmount, request.order.OrderDetails.Select(x => x.GoodId).ToList()))
            {
                disprice = promotion.ApplyCoupon(request.couponId, request.order.PayAmount, request.order.OrderId);
            }
            return disprice;
        }
    }

    /// <summary>
    /// 撤回优惠劵
    /// </summary>
    /// <param name="_db"></param>
    public class ReturnCouponCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<ReturnCouponCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(ReturnCouponCommand request, CancellationToken cancellationToken)
        {
            var model = await _db.Coupon.FirstOrDefaultAsync(x => x.Id == request.couponId);
            model.ReturnOrderCoupon();
            return true;
        }
    }

    /// <summary>
    /// 优惠劵过期
    /// </summary>
    /// <param name="_db"></param>
    public class EndCouponCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<EndCouponCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(EndCouponCommand request, CancellationToken cancellationToken)
        {
            var models = await _db.Coupon.Where(x => request.couponIds.Contains(x.Id)).ToListAsync();
            models?.ForEach(x => x.Invalidate());
            return true;
        }
    }
}
