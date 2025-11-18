using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands
{
    public record DeleteBeverageCollectionCommand(List<long> ids) : ICommand<bool>;
}
