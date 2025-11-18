using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands
{
    public record AppliedBeverageInfoCommand(long templateId, List<AppliedBeverageInput> appliedBeverageInputs) : ICommand<bool>;
}
