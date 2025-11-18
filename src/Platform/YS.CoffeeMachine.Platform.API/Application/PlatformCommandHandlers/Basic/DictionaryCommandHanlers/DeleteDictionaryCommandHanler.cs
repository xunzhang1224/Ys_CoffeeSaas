using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.DictionaryCommand;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.Basic.DictionaryCommandHanlers
{
    /// <summary>
    /// 删除字典
    /// </summary>
    public class DeleteDictionaryCommandHanler(CoffeeMachinePlatformDbContext coffeeMachineDb) : ICommandHandler<DeleteDictionaryCommand>
    {
        /// <summary>
        /// 删除字典（根据key）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(DeleteDictionaryCommand request, CancellationToken cancellationToken)
        {
            await coffeeMachineDb.DictionaryEntity.AsQueryable().Where(a => a.Key == request.key || a.ParentKey == request.key).ExecuteDeleteAsync();
        }
    }
}
