using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IPlatformRepositories.DevicesIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.DevicesRepositories
{
    /// <summary>
    /// 企业设备
    /// </summary>
    /// <param name="context"></param>
    public class PEnterpriseDevicesRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<EnterpriseDevices, long, CoffeeMachinePlatformDbContext>(context), IPEnterpriseDevicesRepository
    {
    }
}
