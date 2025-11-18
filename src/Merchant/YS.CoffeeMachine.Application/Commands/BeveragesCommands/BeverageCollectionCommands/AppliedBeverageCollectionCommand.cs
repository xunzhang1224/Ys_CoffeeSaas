using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands
{
    public record AppliedBeverageCollectionCommand(AppliedBeverageCollectionInput beverageCollectionInput) : ICommand<CommandDownSends>;
}
