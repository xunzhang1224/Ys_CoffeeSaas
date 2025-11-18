using YS.CoffeeMachine.Application.Commands.SettingsCommands.InterfaceStylesCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.IRepositories.SettingsIRepositories;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.SettingsCommandHandlers.InterfaceStylesCommandHandlers
{
    /// <summary>
    /// 新增界面风格
    /// </summary>
    /// <param name="repository"></param>
    public class CreateInterfaceStylesCommandHandler(IInterfaceStylesRepository repository) : ICommandHandler<CreateInterfaceStylesCommand, bool>
    {
        /// <summary>
        /// 新增界面风格
        /// </summary>
        public async Task<bool> Handle(CreateInterfaceStylesCommand request, CancellationToken cancellationToken)
        {
            var info = new InterfaceStyles(request.name, request.code, request.preview);
            var res = repository.AddAsync(info);
            return res != null;
        }
    }
}
