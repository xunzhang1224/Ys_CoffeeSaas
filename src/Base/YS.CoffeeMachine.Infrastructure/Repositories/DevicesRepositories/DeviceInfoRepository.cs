using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Provider.EntityFrameworkCore;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Infrastructure.Repositories.DevicesRepositories
{
    /// <summary>
    /// 设备信息
    /// </summary>
    /// <param name="context"></param>
    public class DeviceInfoRepository(CoffeeMachineDbContext context) : YsRepositoryBase<DeviceInfo, long, CoffeeMachineDbContext>(context), IDeviceInfoRepository
    {
        /// <summary>
        /// 根据Id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeviceInfo> GetByIdAsync(long id)
        {
            return await context.DeviceInfo.Include(i => i.GroupDevices).FirstAsync(w => w.Id == id);
        }

        /// <summary>
        /// 根据国家地区Id获取国家地区名称
        /// </summary>
        /// <param name="countryRegionId"></param>
        /// <returns></returns>
        public async Task<(bool, string, long)> GetCountryRegionTextByCountryRegionId(long countryRegionId)
        {
            var path = new List<string>();
            long countryId = 0;
            while (countryRegionId > 0)
            {
                var region = await context.CountryRegion
                    .AsNoTracking()
                    .Where(r => r.Id == countryRegionId)
                    .Select(r => new { r.RegionName, r.ParentID, r.CountryID })
                    .FirstOrDefaultAsync();

                if (region == null) break;
                countryId = region.CountryID;
                path.Insert(0, region.RegionName); // 插入到路径的开头
                countryRegionId = region.ParentID ?? 0; // 更新为父级Id
            }
            if (path.Count == 0)
                return (false, L.Text[nameof(ErrorCodeEnum.D0014)], countryId);
            return (true, string.Join("/", path), countryId);
        }

        /// <summary>
        /// 批量添加设备信息
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        public async Task<bool> BatchAddAsync(List<DeviceInfo> devices)
        {
            await context.DeviceInfo.AddRangeAsync(devices);
            var res = await context.SaveChangesAsync();
            return res > 0;
        }

        /// <summary>
        /// 根据Mid获取设备信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task<DeviceInfo> GetByMidAsync(string mid)
        {
            //return await context.DeviceInfo.Include(x=>x.DeviceMetrics).FirstOrDefaultAsync(w => w.Mid == mid);
            return null;
        }
    }
}
