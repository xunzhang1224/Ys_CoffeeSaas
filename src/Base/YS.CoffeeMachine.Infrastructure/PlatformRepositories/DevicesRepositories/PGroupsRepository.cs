using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IPlatformRepositories.DevicesIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.DevicesRepositories
{
    /// <summary>
    /// 设备型号仓储
    /// </summary>
    public class PGroupsRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<Groups, long, CoffeeMachinePlatformDbContext>(context), IPGroupsRepository
    {
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Groups> GetByIdAsync(long id)
        {
            return await context.Groups.Include(i => i.Users).ThenInclude(i => i.ApplicationUser).Include(i => i.Devices).ThenInclude(i => i.DeviceInfo).FirstAsync(w => w.Id == id);
        }
    }
    }
