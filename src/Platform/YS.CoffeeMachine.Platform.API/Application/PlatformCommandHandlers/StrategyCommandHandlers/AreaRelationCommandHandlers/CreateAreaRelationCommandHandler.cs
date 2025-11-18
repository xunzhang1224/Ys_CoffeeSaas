using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.StrategyCommands.AreaRelationCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Strategy;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.StrategyCommandHandlers.AreaRelationCommandHandlers
{
    /// <summary>
    /// 创建地区关联
    /// </summary>
    /// <param name="context"></param>
    public class CreateAreaRelationCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateAreaRelationCommand, bool>
    {
        /// <summary>
        /// 创建地区关联
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateAreaRelationCommand request, CancellationToken cancellationToken)
        {

            var countryList = await context.AreaRelation.AsQueryable()
               .Where(a => a.Country == request.country).ToListAsync();

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

            var info = new AreaRelation(request.area, request.country, request.areaCode, request.language, request.currencyId, request.timeZone, request.termServiceUrl, request.enabled);
            await context.AddAsync(info);
            return true;
        }
    }
}
