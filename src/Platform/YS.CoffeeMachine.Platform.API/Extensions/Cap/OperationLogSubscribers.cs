using DotNetCore.CAP;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.Platform.API.Extensions.Cap
{
    /// <summary>
    /// 创建操作日志
    /// </summary>
    /// <param name="_coffeeMachineDb"></param>
    public class OperationLogSubscriber(CoffeeMachineTimescaleDBContext _coffeeMachineDb) : ICapSubscribe
    {
        /// <summary>
        /// 创建操作日志
        /// </summary>
        [CapSubscribe(CapConst.PlatformOperationLog)]
        public async Task Handle(PlatformOperationLogDto input)
        {
            var info = new PlatformOperationLog(input.OperationUserId, input.OperationUserName, input.TrailType, input.Describe, input.Result, input.Ip);
            await _coffeeMachineDb.PlatformOperationLog.AddAsync(info);
            await _coffeeMachineDb.SaveChangesAsync();
        }
    }
}
