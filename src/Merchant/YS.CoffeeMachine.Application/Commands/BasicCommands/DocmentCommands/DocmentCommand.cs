using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BasicCommands.DocmentCommands
{
    public record CreateFileCenterCommand(string fileName, DocmentTypeEnum fileCenterType, string url, FileStateEnum state, long tenantId, string? faildMessage = null) : ICommand;
    public record CreateFileNewCenterCommand(string fileName, DocmentTypeEnum fileCenterType,long tenantId,string code) : ICommand;
    public record UpdateFileCommand(string code, FileStateEnum state, string? faildMessage = null, string? url = null) : ICommand;
    public record DeleteFileCenterCommand(long id) : ICommand<bool>;
}
