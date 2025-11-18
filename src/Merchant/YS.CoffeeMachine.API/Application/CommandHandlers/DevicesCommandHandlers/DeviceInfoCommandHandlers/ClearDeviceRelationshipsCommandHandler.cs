using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.deviceInfosCommandHandlers
{
    /// <summary>
    /// 清除设备关系命令处理器
    /// </summary>
    public class ClearDeviceRelationshipsCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<ClearDeviceRelationshipsCommand, bool>
    {
        /// <summary>
        /// 执行清除设备关系命令
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ClearDeviceRelationshipsCommand request, CancellationToken cancellationToken)
        {
            // 该接口已禁用
            throw ExceptionHelper.AppFriendly("该接口已禁用");

            var deviceInfos = await context.DeviceInfo
                .Include(i => i.BeverageInfos)
                .Include(i => i.SettingInfo)
                .Include(i => i.GroupDevices)
                .Include(i => i.DeviceUserAssociations)
                .Where(w => request.deviceId.Contains(w.Id))
                .ToListAsync();
            if (deviceInfos == null || deviceInfos.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);

            foreach (var deviceInfo in deviceInfos)
            {
                // 清除设备基础信息
                deviceInfo.ClearBaseInfo();

                // 清除分组和用户关系
                deviceInfo.RemoveGroups();
                deviceInfo.RemoveUsers();

                // 清除服务商关系
                deviceInfo.BindServiceProviders(new List<long>());

                // 清除饮品信息
                deviceInfo.ClearBeverageInfos();

                // 重置设置信息
                deviceInfo.ReSetSettingInfo();
            }

            var baseIds = deviceInfos.Select(s => s.DeviceBaseId).Distinct().ToList();

            // 获取设备基础信息，清除相关日志记录
            var deviceBaseInfos = await context.DeviceBaseInfo
                .Where(d => baseIds.Contains(d.Id)).ToListAsync();

            var mids = deviceBaseInfos.Select(s => s.Mid).Distinct().ToList();

            if (deviceBaseInfos != null && deviceBaseInfos.Count != 0)
            {
                // 清除设备操作记录
                var operationLog = await context.OperationLog
                    .Where(o => mids.Contains(o.Mid))
                    .ToListAsync();
                if (operationLog != null && operationLog.Count > 0)
                {
                    operationLog.ForEach(o => o.IsDelete = true);
                    context.OperationLog.UpdateRange(operationLog);
                }

                // 清除设备异常记录
                var deviceAbnormal = await context.DeviceAbnormal
                    .Where(d => baseIds.Contains(d.DeviceBaseId))
                    .ToListAsync();
                if (deviceAbnormal != null && deviceAbnormal.Count > 0)
                {
                    deviceAbnormal.ForEach(d => d.IsDelete = true);
                    context.DeviceAbnormal.UpdateRange(deviceAbnormal);
                }

                // 清除设备订单及制作记录
                var orders = await context.OrderInfo
                    .Where(o => baseIds.Contains(o.DeviceBaseId))
                    .ToListAsync();
                if (orders != null && orders.Count > 0)
                {
                    orders.ForEach(o => o.IsDelete = true);
                    context.OrderInfo.UpdateRange(orders);
                }
            }

            return true;
        }
    }
}