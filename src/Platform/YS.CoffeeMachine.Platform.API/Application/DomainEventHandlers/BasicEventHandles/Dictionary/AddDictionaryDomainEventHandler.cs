using MediatR;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.Basic.LanguageCommandHandlers;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.LanguageCommands;
using YS.CoffeeMachine.Domain.Events.BasicDomainEvents;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.DomainEvent;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace YS.CoffeeMachine.Platform.API.Application.DomainEventHandlers.BasicEventHandles.Dictionary
{
    /// <summary>
    /// 新增字典
    /// </summary>
    /// <param name="coffeeMachineDb"></param>
    /// <param name="mediator"></param>
    public class AddDictionaryDomainEventHandler(CoffeeMachinePlatformDbContext coffeeMachineDb, IMediator mediator) : IDomainEventHandler<AddDictionaryDomainEvent>
    {
        /// <summary>
        /// 新增字典
        /// </summary>
        public async Task Handle(AddDictionaryDomainEvent notification, CancellationToken cancellationToken)
        {
            var languageInfo = await coffeeMachineDb.LanguageInfo.Include(x => x.LanguageTextEntitys).FirstAsync(y => y.Code == LanguageConst.ChineseKey);
            if (languageInfo == null)
            {
                await mediator.Send(new CreateLanguageInfoCommand("中文", LanguageConst.ChineseKey));
            }
            else
            {
                if (!languageInfo.LanguageTextEntitys.Any(x => x.Code == notification.key))
                    await mediator.Send(new CreateLanguageTextCommand(notification.key, new Dictionary<string, string> { { LanguageConst.ChineseKey, notification.value } }));
            }
        }
    }
}
