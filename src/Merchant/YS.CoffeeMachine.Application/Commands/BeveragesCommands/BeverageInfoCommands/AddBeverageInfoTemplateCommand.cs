using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    public record AddBeverageInfoTemplateCommand(long id, long enterpriseInfoId) : ICommand<bool>;
}
