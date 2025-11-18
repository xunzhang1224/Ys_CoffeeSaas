using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands
{
    public record UpdateEnterPriseMenuIdsCommand(long id, List<long> ids) : ICommand<bool>;
}