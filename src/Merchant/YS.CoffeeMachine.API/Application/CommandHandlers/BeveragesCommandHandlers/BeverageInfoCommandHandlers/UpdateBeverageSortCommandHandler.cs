using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Exceptions.Extensions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 修改设备下饮品排序
    /// </summary>
    public class UpdateBeverageSortCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<UpdateBeverageSortCommand, bool>
    {
        /// <summary>
        /// 修改设备下饮品排序
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateBeverageSortCommand request, CancellationToken cancellationToken)
        {
            var infos = await context.BeverageInfo.AsQueryable()
               .Where(a => a.DeviceId == request.deviceId && !a.IsDefault).ToListAsync();

            foreach (var item in infos)
            {
                var info = request.beverageInfoSortList.FirstOrDefault(a => a.Id == item.Id);
                if (info != null)
                {
                    item.UpdateSort(info.Sort);
                }
            }

            bool isSuccess = await context.SaveChangesAsync() > 0;
            if (!isSuccess)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0085)]).StatusCode(200).WithData(false);
            return isSuccess;
        }
    }
}
