using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands
{
    public record MultipleGroupsBindDevicesCommand(List<long> groupIds, List<long> deviceIds) : ICommand<bool>;
}
