using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers
{
    /// <summary>
    /// 创建操作日志
    /// </summary>
    /// <param name="_coffeeMachineDb"></param>
    public class CreateOperationLogSubscriber(CoffeeMachineDbContext _coffeeMachineDb) : ICapSubscribe
    {
        /// <summary>
        /// 创建操作日志
        /// </summary>
        [CapSubscribe(CapConst.CreateOperationLog)]
        public async Task Handle(CreateOperationLogInput input)
        {
            var info = new OperationLog(input.Code, input.Mid, input.OperationName, input.RequestUrl, input.RequestType, input.RequestWayType, input.IpAddress,
                input.CreateUserId ?? 0, input.CreateUserName ?? "", input.EnterpriseinfoId, input.OperationSubLogs);

            // 设置当时的设备及企业信息
            var deviceBaseInfo = await _coffeeMachineDb.DeviceBaseInfo.FirstOrDefaultAsync(w => w.MachineStickerCode == input.Mid);
            if (deviceBaseInfo != null)
            {
                var deviceInfo = await _coffeeMachineDb.DeviceInfo.IgnoreQueryFilters().AsNoTracking().Include(i => i.DeviceModel).FirstOrDefaultAsync(w => w.DeviceBaseId == deviceBaseInfo.Id);
                if (deviceInfo != null)
                {
                    var enterpriseInfo = await _coffeeMachineDb.EnterpriseInfo.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == deviceInfo.EnterpriseinfoId);
                    info.SetEDBaseInfo(enterpriseInfo.Id, enterpriseInfo.Name, deviceInfo.Id, deviceInfo.Name, deviceInfo.DeviceModel == null ? 0 : deviceInfo.DeviceModel.Id, deviceInfo.DeviceModel == null ? "" : deviceInfo.DeviceModel.Name);
                }
            }

            await _coffeeMachineDb.OperationLog.AddAsync(info);
            await _coffeeMachineDb.SaveChangesAsync();
        }
    }
}