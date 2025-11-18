using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IRepositories.AdvertisementsIRepositories
{
    /// <summary>
    /// 广告
    /// </summary>
    public interface IAdvertisementsRepository : IYsRepository<AdvertisementInfo, long>
    {
    }
}
