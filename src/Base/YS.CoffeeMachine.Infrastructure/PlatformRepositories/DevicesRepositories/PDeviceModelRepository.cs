using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IPlatformRepositories.DevicesIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.DevicesRepositories
{
    /// <summary>
    /// 设备型号
    /// </summary>
    /// <param name="context"></param>
    public class PDeviceModelRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<DeviceModel, long, CoffeeMachinePlatformDbContext>(context), IPDeviceModelRepository
    {
        /// <summary>
        /// 获取所有设备型号
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeviceModel>> GetDeviceModelsAsync()
        {
            return await context.DeviceModel.AsNoTracking().Where(w => !w.IsDelete).ToListAsync();
        }
    }
}
