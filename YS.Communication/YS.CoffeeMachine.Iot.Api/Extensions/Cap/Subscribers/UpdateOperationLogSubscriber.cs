using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Iot.Api.Iot.Interfaces;
using YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto
{
    /// <summary>
    /// UpdateOperationLogSubscriber
    /// </summary>
    /// <param name="_coffeeMachinePlatformDb"></param>
    public class UpdateOperationLogSubscriber(CoffeeMachinePlatformDbContext _coffeeMachinePlatformDb) : ICapSubscribe
    {
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.UpdateOperationLog)]
        public async Task Handle(UpdateOperationLogInput input)
        {
            var log = await _coffeeMachinePlatformDb.OperationLog.Include(x => x.OperationSubLogs).Where(x => x.Code == input.Code).FirstOrDefaultAsync();
            if (log != null)
            {
                foreach (var sub in log.OperationSubLogs)
                {
                    sub.Update(input.OperationResult, input.ErrorMsg);
                }
                if (log.OperationSubLogs.All(x => x.OperationResult == OperationResultEnum.CommandIssued))
                {
                    log.UpdateReslut(OperationResultEnum.CommandIssued);
                }
                else if (log.OperationSubLogs.All(x => x.OperationResult == OperationResultEnum.CommandExecuted))
                {
                    log.UpdateReslut(OperationResultEnum.CommandExecuted);
                }
                else
                {
                    log.UpdateReslut(OperationResultEnum.CommandUnexecuted);
                }
                _coffeeMachinePlatformDb.Update(log);
                await _coffeeMachinePlatformDb.SaveChangesAsync();
            }
        }
    }
}
