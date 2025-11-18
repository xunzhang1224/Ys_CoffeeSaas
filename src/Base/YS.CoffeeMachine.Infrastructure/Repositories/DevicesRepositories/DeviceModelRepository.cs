using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories.DevicesRepositories
{
    /// <summary>
    /// 设备型号
    /// </summary>
    /// <param name="context"></param>
    public class DeviceModelRepository(CoffeeMachineDbContext context) : YsRepositoryBase<DeviceModel, long, CoffeeMachineDbContext>(context), IDeviceModelRepository
    {
        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeviceModel>> GetDeviceModelsAsync()
        {
            return await context.DeviceModel.AsNoTracking().Where(w => !w.IsDelete).ToListAsync();
        }
    }
}
