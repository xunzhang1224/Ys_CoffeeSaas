using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands
{
    public record AppliedBeverageInfoCommand(long templateId, List<AppliedBeverageInput> appliedBeverageInputs) : ICommand<DrinkCommandDownSends>;
}
