using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    public record B_AppliedBeverageInfoCommand(long beverageId, List<AppliedBeverageInput> appliedBeverageInputs) : ICommand<bool>;
}
