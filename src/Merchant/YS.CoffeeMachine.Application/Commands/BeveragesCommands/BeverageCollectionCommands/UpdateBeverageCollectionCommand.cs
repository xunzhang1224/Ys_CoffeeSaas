using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands
{
    public record UpdateBeverageCollectionCommand(long id, string name) : ICommand<bool>;
}
