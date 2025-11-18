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
    /// 设备
    /// </summary>
    /// <param name="context"></param>
    public class EnterpriseDevicesRepository(CoffeeMachineDbContext context) : YsRepositoryBase<EnterpriseDevices, long, CoffeeMachineDbContext>(context), IEnterpriseDevicesRepository
    {
    }
}
