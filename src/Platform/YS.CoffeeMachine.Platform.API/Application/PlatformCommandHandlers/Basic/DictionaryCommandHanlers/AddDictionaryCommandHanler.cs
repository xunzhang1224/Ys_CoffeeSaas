using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.DictionaryCommand;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Dictionary;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.Basic.Dictionary
{
    /// <summary>
    /// 新增字典
    /// </summary>
    /// <param name="coffeeMachineDb"></param>
    public class AddDictionaryCommandHanler(CoffeeMachinePlatformDbContext coffeeMachineDb) : ICommandHandler<AddDictionaryCommand>
    {
        /// <summary>
        /// 新增字典
        /// </summary>
        public async Task Handle(AddDictionaryCommand request, CancellationToken cancellationToken)
        {
            var info = await coffeeMachineDb.DictionaryEntity.AsQueryable().FirstOrDefaultAsync(a => a.Key == request.key);
            if (info != null)
            {
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0012)]);
            }
            var dic = new DictionaryEntity(request.key, request.value, request.enabled, request.parentKey);
            await coffeeMachineDb.AddAsync(dic);
        }
    }
}