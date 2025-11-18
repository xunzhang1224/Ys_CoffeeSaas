using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.EnterpriseDevicesCommand;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.EnterpriseDevicesCommandHandler
{
    /// <summary>
    /// 设备分配
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_user"></param>
    public class CreateEnterpriseDevicesCommandHandler(CoffeeMachineDbContext context, UserHttpContext _user, IClearDeviceRelationshipService clearDeviceRelationshipService) : ICommandHandler<CreateEnterpriseDevicesCommand, bool>
    {
        /// <summary>
        /// 设备分配
        /// </summary>
        public async Task<bool> Handle(CreateEnterpriseDevicesCommand request, CancellationToken cancellationToken)
        {

            if (request.enterpriseId == _user.TenantId)
                throw ExceptionHelper.AppFriendly("目标企业不能是自己");

            if (request.allocationEnum == DeviceAllocationEnum.Lease && request.recyclingTime == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0066)]);

            // 新增的设备分配
            var List = new List<EnterpriseDevices>();

            // 如果该设备已存在，则需即使修改当前所属企业
            var enterpriseDeviceInfos = await context.EnterpriseDevices.AsQueryable().IgnoreQueryFilters().Where(a => request.deviceIds.Contains(a.DeviceId) && !a.IsDelete).ToListAsync();
            foreach (var enterpriseDeviceInfo in enterpriseDeviceInfos)
            {
                if (enterpriseDeviceInfo.DeviceAllocationType == DeviceAllocationEnum.Lease && request.allocationEnum == DeviceAllocationEnum.Lease)
                {
                    throw ExceptionHelper.AppFriendly("存在租赁中的设备，不能再次租赁");
                }

                if (enterpriseDeviceInfo.DeviceAllocationType == DeviceAllocationEnum.Lease && request.allocationEnum == DeviceAllocationEnum.Sale)
                {
                    throw ExceptionHelper.AppFriendly("存在租赁中的设备，不能进行售卖");
                }

                enterpriseDeviceInfo.UpdateEnterpriseId(request.enterpriseId);
            }

            var deviceInfos = await context.DeviceInfo
                .Include(i => i.BeverageInfos)
                .Include(i => i.SettingInfo)
                .Include(i => i.GroupDevices)
                .Include(i => i.DeviceUserAssociations)
                .Where(w => request.deviceIds.Contains(w.Id)).ToListAsync();

            // 所属企业为当前设备企业
            foreach (var deviceInfo in deviceInfos)
            {
                //var deviceInfo = await context.DeviceInfo.Include(i => i.GroupDevices).Include(i => i.DeviceUserAssociations).FirstOrDefaultAsync(w => w.Id == deviceId);
                if (deviceInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
                if (deviceInfo.EnterpriseinfoId != _user.TenantId)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0065)]);

                // 清除设备分组
                deviceInfo.RemoveGroups();

                // 清除设备用户
                deviceInfo.RemoveUsers();

                // 更新设备所属企业
                deviceInfo.EnterpriseinfoId = request.enterpriseId;

                // 设备分配
                List.Add(new EnterpriseDevices(request.enterpriseId, deviceInfo.Id, request.enterpriseId, request.allocationEnum, request.recyclingTime));
            }

            await context.EnterpriseDevices.AddRangeAsync(List);

            // 清除设备关系
            if (request.isClearRelationship)
            {
                foreach (var deviceInfo in deviceInfos)
                {
                    // 清除设备基础信息
                    //deviceInfo.ClearBaseInfo();

                    // 清除服务商关系
                    if (deviceInfo.DeviceServiceProviders != null && deviceInfo.DeviceServiceProviders.Count > 0)
                        deviceInfo.BindServiceProviders(new List<long>());

                    // 清除饮品信息
                    deviceInfo.ClearBeverageInfos();

                    // 重置设置信息
                    if (deviceInfo.SettingInfo != null)
                        deviceInfo.ReSetSettingInfo();
                }

                context.DeviceInfo.UpdateRange(deviceInfos);

                var baseIds = deviceInfos.Select(s => s.DeviceBaseId).Distinct().ToList();

                // 获取设备基础信息，清除相关日志记录
                var deviceBaseInfos = await context.DeviceBaseInfo.IgnoreQueryFilters()
                    .Where(d => baseIds.Contains(d.Id)).ToListAsync();

                var mids = deviceBaseInfos.Select(s => s.Mid).Distinct().ToList();

                if (deviceBaseInfos != null && deviceBaseInfos.Count != 0)
                {
                    // 清除设备操作记录
                    var operationLog = await context.OperationLog.IgnoreQueryFilters()
                        .Where(o => mids.Contains(o.Mid))
                        .ToListAsync();
                    if (operationLog != null && operationLog.Count > 0)
                    {
                        operationLog.ForEach(o => o.IsDelete = true);
                        context.OperationLog.UpdateRange(operationLog);
                    }

                    // 清除设备异常记录
                    var deviceAbnormal = await context.DeviceAbnormal.IgnoreQueryFilters()
                        .Where(d => baseIds.Contains(d.DeviceBaseId))
                        .ToListAsync();
                    if (deviceAbnormal != null && deviceAbnormal.Count > 0)
                    {
                        deviceAbnormal.ForEach(d => d.IsDelete = true);
                        context.DeviceAbnormal.UpdateRange(deviceAbnormal);
                    }

                    // 清除设备订单及制作记录
                    var orders = await context.OrderInfo.IgnoreQueryFilters()
                        .Where(o => baseIds.Contains(o.DeviceBaseId))
                        .ToListAsync();
                    if (orders != null && orders.Count > 0)
                    {
                        orders.ForEach(o => o.IsDelete = true);
                        context.OrderInfo.UpdateRange(orders);
                    }
                }
            }

            return true;
            //return await context.SaveChangesAsync() > 0;
        }
    }
}