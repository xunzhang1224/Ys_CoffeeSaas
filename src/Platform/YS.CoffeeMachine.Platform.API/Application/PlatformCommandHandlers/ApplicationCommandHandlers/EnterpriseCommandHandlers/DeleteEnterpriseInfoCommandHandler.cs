using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Infrastructure.PlatformRepositories.ApplicationUsersRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 删除企业信息
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class DeleteEnterpriseInfoCommandHandler(PEnterpriseInfoRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<DeleteEnterpriseInfoCommand, bool>
    {
        /// <summary>
        /// 删除企业信息
        /// </summary>
        public async Task<bool> Handle(DeleteEnterpriseInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            // 删除企业
            var res = await repository.FakeDeleteByIdAsync(request.id);
            // 删除企业下的默认用户
            var allUser = await context.ApplicationUser.Where(w => w.EnterpriseId == request.id).ToListAsync();
            if (allUser != null)
            {
                allUser.ForEach(x => x.IsDelete = true);
                await context.SaveChangesAsync();
            }
            return res > 0;
        }
    }
}