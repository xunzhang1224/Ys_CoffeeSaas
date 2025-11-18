using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.DevicesQueries
{
    /// <summary>
    /// 设备支付设置查询
    /// </summary>
    public class DevicePaymentSettingQueries(CoffeeMachineDbContext context) : IDevicePaymentQueries
    {
        /// <summary>
        /// 获取指定支付方式下绑定的设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DevicePaymentDto>> GetBildDeviceList(DevicePaymentInput input)
        {
            // 关联表分页
            var devicePyamentSettingInfo = await context.DevicePaymentConfig.AsQueryable()
                .WhereIf(input.GroupIds.Count > 0, a => context.GroupDevices.Any(b => a.DeviceInfoId == b.DeviceInfoId && input.GroupIds.Contains(b.GroupsId)))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceName), a => context.DeviceInfo.Any(b => b.Id == a.Id && b.Name.Contains(input.DeviceName)))
                .Where(a => a.PaymentConfigId == input.PaymentConfigId)
                .Select(a => new DevicePaymentDto
                {
                    DeviceId = a.DeviceInfoId,
                    PaymentConfigId = a.PaymentConfigId,
                }).ToPagedListAsync(input);
            // 获取设备ids
            var deviceIds = devicePyamentSettingInfo.Items.Select(a => a.DeviceId).ToList();
            // 获取设备名字字典
            var deviceDic = await context.DeviceInfo.AsQueryable().Where(a => deviceIds.Contains(a.Id)).ToDictionaryAsync(a => a.Id, a => a.Name);
            // 获取设备分组信息字典
            var groupData = await (
                    from a in context.GroupDevices
                    join b in context.Groups on a.GroupsId equals b.Id into bJoin
                    from b in bJoin.DefaultIfEmpty()
                    where deviceIds.Contains(a.DeviceInfoId)
                    select new { a.DeviceInfoId, b }
                ).ToListAsync();
            var groupDict = groupData
            .GroupBy(x => x.DeviceInfoId)
            .ToDictionary(
                g => g.Key,
                g => new
                {
                    GroupIds = g.Where(x => x.b != null).Select(x => x.b.Id).ToList(),
                    GroupNames = string.Join(",", g.Where(x => x.b != null).Select(x => x.b.Name))
                }
            );

            // 组装数据
            foreach (var item in devicePyamentSettingInfo.Items)
            {
                if (deviceDic != null && deviceDic.ContainsKey(item.DeviceId ?? 0))
                {
                    item.DeviceName = deviceDic[item.DeviceId ?? 0];
                }
                if (groupDict != null && groupDict.ContainsKey(item.DeviceId ?? 0))
                {
                    item.GroupIds = groupDict[item.DeviceId ?? 0].GroupIds;
                    item.GroupNames = groupDict[item.DeviceId ?? 0].GroupNames;
                }
            }

            return devicePyamentSettingInfo;
        }

        /// <summary>
        /// 获取指定支付方式下未绑定的设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DevicePaymentDto>> GetNotBildDeviceList(DevicePaymentInput input)
        {
            var devices = await context.DeviceInfo
                    .WhereIf(input.GroupIds.Count > 0, a => context.GroupDevices.Any(b => a.Id == b.DeviceInfoId && input.GroupIds.Contains(b.GroupsId)))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceName), a => a.Name.Contains(input.DeviceName))
                    .Where(di => !context.DevicePaymentConfig
                        .Any(dps => dps.DeviceInfoId == di.Id && dps.PaymentConfigId == input.PaymentConfigId))
                     .Select(a => new DevicePaymentDto
                     {
                         DeviceId = a.Id,
                         PaymentConfigId = input.PaymentConfigId,
                         DeviceName = a.Name
                     }).ToPagedListAsync(input);

            // 获取设备ids
            var deviceIds = devices.Items.Select(a => a.DeviceId).ToList();

            // 获取设备分组信息字典
            var groupData = await (
                    from a in context.GroupDevices
                    join b in context.Groups on a.GroupsId equals b.Id into bJoin
                    from b in bJoin.DefaultIfEmpty()
                    where deviceIds.Contains(a.DeviceInfoId)
                    select new { a.DeviceInfoId, b }
                ).ToListAsync();
            var groupDict = groupData
            .GroupBy(x => x.DeviceInfoId)
            .ToDictionary(
                g => g.Key,
                g => new
                {
                    GroupIds = g.Where(x => x.b != null).Select(x => x.b.Id).ToList(),
                    GroupNames = string.Join(",", g.Where(x => x.b != null).Select(x => x.b.Name))
                }
            );
            // 组装数据
            foreach (var item in devices.Items)
            {
                if (groupDict != null && groupDict.ContainsKey(item.DeviceId ?? 0))
                {
                    item.GroupIds = groupDict[item.DeviceId ?? 0].GroupIds;
                    item.GroupNames = groupDict[item.DeviceId ?? 0].GroupNames;
                }
            }
            return devices;
        }
    }
}
