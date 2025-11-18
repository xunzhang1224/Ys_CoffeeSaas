using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands
{
    public record BindDevicesCommand(long groupId, List<long> deviceIds) : ICommand<bool>;
}
