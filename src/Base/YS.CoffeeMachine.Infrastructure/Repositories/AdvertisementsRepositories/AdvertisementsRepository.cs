using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
using YS.CoffeeMachine.Domain.IRepositories.AdvertisementsIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories.AdvertisementsRepositories
{
    /// <summary>
    /// 广告
    /// </summary>
    /// <param name="context"></param>
    public class AdvertisementsRepository(CoffeeMachineDbContext context) : YsRepositoryBase<AdvertisementInfo, long, CoffeeMachineDbContext>(context), IAdvertisementsRepository
    {
    }
}
