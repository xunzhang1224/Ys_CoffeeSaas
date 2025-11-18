using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands
{
    public record CreateBeverageCollectionCommand(long enterpriseInfoId, long deviceId, string name) : ICommand<bool>;
}
