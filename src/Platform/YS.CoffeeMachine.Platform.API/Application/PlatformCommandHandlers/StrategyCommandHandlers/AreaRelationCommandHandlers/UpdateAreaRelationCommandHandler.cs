using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.StrategyCommands.AreaRelationCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.StrategyCommandHandlers.AreaRelationCommandHandlers
{
    /// <summary>
    /// 更新地区关联
    /// </summary>
    /// <param name="context"></param>
    public class UpdateAreaRelationCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateAreaRelationCommand, bool>
    {
        /// <summary>
        /// 更新地区关联
        /// </summary>
        public async Task<bool> Handle(UpdateAreaRelationCommand request, CancellationToken cancellationToken)
        {
            var countryList = await context.AreaRelation.AsQueryable()
                .Where(a => a.Country == request.country && a.Id != request.id).ToListAsync();

            if (countryList.Count > 0)
            {
                foreach (var country in countryList)
                {
                    if (country.Area != request.area)
                    {
                        throw ExceptionHelper.AppFriendly("同国家的地区需要一致");
                    }
                    if (country.AreaCode != request.areaCode)
                    {
                        throw ExceptionHelper.AppFriendly("同国家的区号需要一致");
                    }
                    if (country.Language != request.language)
                    {
                        throw ExceptionHelper.AppFriendly("同国家的语言需要一致");
                    }
                    if (country.CurrencyId != request.currencyId)
                    {
                        throw ExceptionHelper.AppFriendly("同国家的币种需要一致");
                    }

                    if (country.TimeZone == request.timeZone)
                    {
                        throw ExceptionHelper.AppFriendly("该国家已存在相同时区");
                    }
                }
            }

            var info = await context.AreaRelation.AsQueryable()
                .Where(a => a.Id == request.id).FirstOrDefaultAsync();
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            info.UpdateAreaRelation(request.area, request.country, request.areaCode, request.language, request.currencyId, request.timeZone, request.termServiceUrl, request.enabled);
            return true;
        }
    }
}