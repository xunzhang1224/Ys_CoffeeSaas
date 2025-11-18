using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BasicCommands.FaultCodeCommands
{
    public record FaultCodeCommand(string code, string lanCode, FaultCodeTypeEnum type, string name, string description, string remark) : ICommand<bool>;

    public record UpdateFaultCodeCommand(string code, string lanCode, string name, string description, string remark) : ICommand<bool>;

    public record DeleteFaultCodeCommand(string code) : ICommand<bool>;
}