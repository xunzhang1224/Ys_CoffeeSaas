using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.ProductCategoryCommands;
using YS.CoffeeMachine.Application.Commands.MarketingActivitysCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.MarketingActivitysCommandHandlers
{
    /// <summary>
    /// 新建营销活动
    /// </summary>
    /// <param name="_db"></param>
    public class AddPromotionCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<AddPromotionCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(AddPromotionCommand request, CancellationToken cancellationToken)
        {
            var model = new Promotion(request.name, request.startTime, request.endTime, request.couponType, request.totalLimit, request.value, request.usageRules, request.limitedCount, request.sort, request.ParticipatingUsers);
            await _db.AddAsync(model);
            return true;
        }
    }

    /// <summary>
    /// 修改营销活动
    /// </summary>
    /// <param name="_db"></param>
    public class UpdatePromotionCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UpdatePromotionCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
        {
            var model = await _db.Promotion.FirstOrDefaultAsync(x => x.Id == request.id);
            if (model != null)
            {
                model.Update(request.name, request.startTime, request.endTime, request.couponType, request.totalLimit, request.value, request.usageRules, request.limitedCount, request.sort, request.ParticipatingUsers);
                _db.Update(model);
                return true;
            }
            return false;
        }
    }
}
