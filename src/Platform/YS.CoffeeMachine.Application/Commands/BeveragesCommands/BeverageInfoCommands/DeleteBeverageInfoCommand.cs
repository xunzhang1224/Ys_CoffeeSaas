using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    public record DeleteBeverageInfoCommand(List<long> ids) : ICommand<bool>;
}
