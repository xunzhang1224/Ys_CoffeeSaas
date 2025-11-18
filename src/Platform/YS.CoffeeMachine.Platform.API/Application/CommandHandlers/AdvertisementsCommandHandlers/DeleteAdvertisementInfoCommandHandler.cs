using YS.CoffeeMachine.Application.Commands.AdvertisementsCommands.AdvertisementsInfoCommands;
using YS.CoffeeMachine.Domain.IRepositories.AdvertisementsIRepositories;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.AdvertisementsCommandHandlers
{
    /// <summary>
    /// 删除广告
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteAdvertisementInfoCommandHandler(IAdvertisementsRepository repository) : ICommandHandler<DeleteAdvertisementInfoCommand, bool>
    {
        /// <summary>
        /// 删除广告
        /// </summary>
        public Task<bool> Handle(DeleteAdvertisementInfoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //if (request.id <= 0)
            //    throw ExceptionHelper.AppFriendly("Id不能为空{0}", request.id);
            //var res = await repository.FakeDeleteByIdAsync(request.id);
            //return res > 0;
        }
    }
}
