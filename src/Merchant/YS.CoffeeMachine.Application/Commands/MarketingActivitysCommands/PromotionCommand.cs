using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.MarketingActivitysCommands
{
    public record AddPromotionCommand(string name, int number, DateTime startTime, DateTime endTime, PromotionType couponType, int totalLimit,
            CouponValue value, UsageRules usageRules, int limitedCount = 0, int sort = 0, List<long>? ParticipatingUsers = null) : ICommand<bool>;

    public record UpdatePromotionCommand(long id, string name, int number, DateTime startTime, DateTime endTime, PromotionType couponType, int totalLimit,
            CouponValue value, UsageRules usageRules, int limitedCount = 0, int sort = 0, List<long>? ParticipatingUsers = null) : ICommand<bool>;
}
