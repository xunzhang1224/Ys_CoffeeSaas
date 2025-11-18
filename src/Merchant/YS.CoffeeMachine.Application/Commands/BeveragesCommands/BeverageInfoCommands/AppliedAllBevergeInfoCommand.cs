using YS.CoffeeMachine.Application.Dtos.Cap;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    public record AppliedAllBevergeInfoCommand(long deviceId, List<long> targetDeviceIds) : ICommand<CommandDownSends>;
}
