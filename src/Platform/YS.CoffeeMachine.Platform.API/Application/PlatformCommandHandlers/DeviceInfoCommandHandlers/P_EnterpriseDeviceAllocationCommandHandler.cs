using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 设备分配
    /// </summary>
    /// <param name="context"></param>
    public class P_EnterpriseDeviceAllocationCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<P_EnterpriseDeviceAllocationCommand, bool>
    {
        /// <summary>
        /// 设备分配
        /// </summary>
        public async Task<bool> Handle(P_EnterpriseDeviceAllocationCommand request, CancellationToken cancellationToken)
        {
            // 商户端不存在设备信息，需要创建
            if (request.deviceId == null)
            {
                var deviceBaseInfo = await context.DeviceBaseInfo.AsQueryable().SingleOrDefaultAsync(a => a.Id == request.deviceBaseId);
                var info = new Domain.AggregatesModel.Devices.DeviceInfo(request.enterpriseId, request.deviceBaseId, deviceBaseInfo.DeviceModelId ?? 0);

                var time = DateTime.UtcNow;
                //获取默认风格Id
                var interfaceStyles = await context.InterfaceStyles.FirstOrDefaultAsync();
                if (interfaceStyles == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0058)]);
                //获取当前设备型号
                var deviceModel = await context.DeviceModel.FirstOrDefaultAsync(w => w.Id == deviceBaseInfo.DeviceModelId);
                if (deviceModel == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0059)]);
                //根据设备型号，组装默认料盒
                List<MaterialBox> materialBoxes = new List<MaterialBox>();
                for (int i = 1; i <= deviceModel.MaxCassetteCount; i++)
                    materialBoxes.Add(new MaterialBox(0, "", i, true));
                //创建设备默认设置信息
                info.CreateSettingInfo(true, interfaceStyles.Id, WashEnum.Automatic, "00:00:00", null, string.Empty, "00:00:00", 50, 50, "", "", "00:00:00", 0, "00:00:00", 0, string.Empty, string.Empty, materialBoxes);
                //创建设备默认预警信息
                info.CreateDefaultEarlyWarningConfig(info.Id, false, time, false, time, false, time, false, time, false, time, false, 10, false, 0, 0);
                await context.AddAsync(info);
            }
            else
            {
                var deviceInfo = await context.DeviceInfo.Include(i => i.GroupDevices).Include(i => i.DeviceUserAssociations).FirstAsync(w => w.Id == request.deviceId);
                if (deviceInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
                var enterpriseInfo = await context.EnterpriseInfo.FirstAsync(w => w.Id == request.enterpriseId);
                if (enterpriseInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0068)]);

                //删除设备分组
                deviceInfo.RemoveGroups();
                //组装设备分配信息
                //var enterpriseDeviceInfo = new EnterpriseDevices(request.enterpriseId, request.deviceId, request.enterpriseId, DeviceAllocationEnum.Sale, null);//平台端分配的设备，不需要有分配记录
                //获取当前设备原分配信息
                var curDeviceAllocationList = await context.EnterpriseDevices.Where(w => w.DeviceId == request.deviceId).ToListAsync();
                //清除当前设备原分配信息
                if (curDeviceAllocationList.Count > 0)
                    context.EnterpriseDevices.RemoveRange(curDeviceAllocationList);
                //清除绑定的用户
                deviceInfo.RemoveUsers();

                //设备绑定企业
                //deviceInfo.ChangebindEnterprise(request.enterpriseId);
                deviceInfo.UnbindEnterprise();
                var info = new Domain.AggregatesModel.Devices.DeviceInfo(request.enterpriseId, request.deviceBaseId, deviceInfo.DeviceModelId ?? 0);
                await context.AddAsync(info);

                // 清除设备关系
                if (request.isClearRelationship)
                {
                    // 清除服务商关系
                    if (deviceInfo.DeviceServiceProviders != null && deviceInfo.DeviceServiceProviders.Count > 0)
                        deviceInfo.BindServiceProviders(new List<long>());

                    // 清除饮品信息
                    deviceInfo.ClearBeverageInfos();

                    // 重置设置信息
                    if (deviceInfo.SettingInfo != null)
                        deviceInfo.ReSetSettingInfo();

                    // 获取设备基础信息，清除相关日志记录
                    var deviceBaseInfo = await context.DeviceBaseInfo.IgnoreQueryFilters()
                        .Where(d => deviceInfo.DeviceBaseId == d.Id).FirstOrDefaultAsync();

                    if (deviceBaseInfo != null)
                    {
                        // 清除设备操作记录
                        var operationLog = await context.OperationLog.IgnoreQueryFilters()
                            .Where(o => deviceBaseInfo.Mid == o.Mid)
                            .ToListAsync();
                        if (operationLog != null && operationLog.Count > 0)
                        {
                            operationLog.ForEach(o => o.IsDelete = true);
                            context.OperationLog.UpdateRange(operationLog);
                        }

                        // 清除设备异常记录
                        var deviceAbnormal = await context.DeviceAbnormal.IgnoreQueryFilters()
                            .Where(d => deviceBaseInfo.Id == d.DeviceBaseId)
                            .ToListAsync();
                        if (deviceAbnormal != null && deviceAbnormal.Count > 0)
                        {
                            deviceAbnormal.ForEach(d => d.IsDelete = true);
                            context.DeviceAbnormal.UpdateRange(deviceAbnormal);
                        }

                        // 清除设备订单及制作记录
                        var orders = await context.OrderInfo.IgnoreQueryFilters()
                            .Where(o => deviceBaseInfo.Id == o.DeviceBaseId)
                            .ToListAsync();
                        if (orders != null && orders.Count > 0)
                        {
                            orders.ForEach(o => o.IsDelete = true);
                            context.OrderInfo.UpdateRange(orders);
                        }
                    }
                }
            }

            //保存设备分配信息
            //await context.EnterpriseDevices.AddAsync(enterpriseDeviceInfo);
            return true;
        }
    }

    /// <summary>
    /// 设备解绑企业
    /// </summary>
    /// <param name="context"></param>
    public class DeviceUnbindEnterpriseCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<DeviceUnbindEnterpriseCommand>
    {
        /// <summary>
        /// 设备解绑企业
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(DeviceUnbindEnterpriseCommand request, CancellationToken cancellationToken)
        {
            var deviceInfo = await context.DeviceInfo.Include(i => i.GroupDevices).Include(i => i.DeviceUserAssociations).FirstAsync(w => w.Id == request.deviceId);
            if (deviceInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            var enterpriseInfo = await context.EnterpriseInfo.FirstAsync(w => w.Id == request.enterpriseId);
            if (enterpriseInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0068)]);

            //删除设备分组
            deviceInfo.RemoveGroups();
            //组装设备分配信息
            //var enterpriseDeviceInfo = new EnterpriseDevices(request.enterpriseId, request.deviceId, request.enterpriseId, DeviceAllocationEnum.Sale, null);//平台端分配的设备，不需要有分配记录
            //获取当前设备原分配信息
            var curDeviceAllocationList = await context.EnterpriseDevices.Where(w => w.DeviceId == request.deviceId).ToListAsync();
            //清除当前设备原分配信息
            if (curDeviceAllocationList.Count > 0)
                context.EnterpriseDevices.RemoveRange(curDeviceAllocationList);
            //清除绑定的用户
            deviceInfo.RemoveUsers();
            // 设备解绑企业
            deviceInfo.UnbindEnterprise();

            // 清除设备关系
            if (request.isClearRelationship)
            {
                // 清除服务商关系
                if (deviceInfo.DeviceServiceProviders != null && deviceInfo.DeviceServiceProviders.Count > 0)
                    deviceInfo.BindServiceProviders(new List<long>());

                // 清除饮品信息
                deviceInfo.ClearBeverageInfos();

                // 重置设置信息
                if (deviceInfo.SettingInfo != null)
                    deviceInfo.ReSetSettingInfo();

                // 获取设备基础信息，清除相关日志记录
                var deviceBaseInfo = await context.DeviceBaseInfo.IgnoreQueryFilters()
                    .Where(d => deviceInfo.DeviceBaseId == d.Id).FirstOrDefaultAsync();

                if (deviceBaseInfo != null)
                {
                    // 清除设备操作记录
                    var operationLog = await context.OperationLog.IgnoreQueryFilters()
                        .Where(o => deviceBaseInfo.Mid == o.Mid)
                        .ToListAsync();
                    if (operationLog != null && operationLog.Count > 0)
                    {
                        operationLog.ForEach(o => o.IsDelete = true);
                        context.OperationLog.UpdateRange(operationLog);
                    }

                    // 清除设备异常记录
                    var deviceAbnormal = await context.DeviceAbnormal.IgnoreQueryFilters()
                        .Where(d => deviceBaseInfo.Id == d.DeviceBaseId)
                        .ToListAsync();
                    if (deviceAbnormal != null && deviceAbnormal.Count > 0)
                    {
                        deviceAbnormal.ForEach(d => d.IsDelete = true);
                        context.DeviceAbnormal.UpdateRange(deviceAbnormal);
                    }

                    // 清除设备订单及制作记录
                    var orders = await context.OrderInfo.IgnoreQueryFilters()
                        .Where(o => deviceBaseInfo.Id == o.DeviceBaseId)
                        .ToListAsync();
                    if (orders != null && orders.Count > 0)
                    {
                        orders.ForEach(o => o.IsDelete = true);
                        context.OrderInfo.UpdateRange(orders);
                    }
                }
            }
        }
    }
}
