using YS.CoffeeMachine.Application.Commands.SettingsCommands.SettingInfoCommands;
using YS.CoffeeMachine.Domain.IRepositories.SettingsIRepositories;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.SettingsCommandHandlers.SettingInfoCommandHandlers
{
    /// <summary>
    /// 删除设置信息
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteSettingInfoCommandHandler(ISettingInfoRepository repository) : ICommandHandler<DeleteSettingInfoCommand, bool>
    {
        /// <summary>
        /// 删除设置信息
        /// </summary>
        public async Task<bool> Handle(DeleteSettingInfoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //if (request.id <= 0)
            //    throw ExceptionHelper.AppFriendly("Id不能为空{0}", request.id);
            //var res = await repository.FakeDeleteByIdAsync(request.id);
            //return res > 0;
        }
    }
}
