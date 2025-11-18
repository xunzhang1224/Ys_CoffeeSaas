using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.DictionaryCommand;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Dictionary;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.Basic.DictionaryCommandHanlers
{

    /// <summary>
    /// 更新字典
    /// </summary>
    /// <param name="coffeeMachineDb"></param>
    public class UpdateDictionaryCommandHanler(CoffeeMachinePlatformDbContext coffeeMachineDb) : ICommandHandler<UpdateDictionaryCommand>
    {
        /// <summary>
        /// 更新字典
        /// </summary>
        public async Task Handle(UpdateDictionaryCommand request, CancellationToken cancellationToken)
        {
            var dic = await coffeeMachineDb.DictionaryEntity.Where(x => x.Key == request.key).FirstAsync();
            dic.Update(request.value, request.enabled);
            coffeeMachineDb.Update(dic);
        }
    }
}
