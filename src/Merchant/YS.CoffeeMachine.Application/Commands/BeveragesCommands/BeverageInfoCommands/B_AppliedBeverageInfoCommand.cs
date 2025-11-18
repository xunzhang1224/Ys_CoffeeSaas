using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    public record B_AppliedBeverageInfoCommand(long beverageId, List<AppliedBeverageInput> appliedBeverageInputs) : ICommand<DrinkCommandDownSends>;
}