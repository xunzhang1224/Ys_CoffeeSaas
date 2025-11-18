using AutoMapper;
using DotNetCore.CAP;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto;

namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Subscribers
{
    /// <summary>
    /// 记录在线状态日志
    /// </summary>
    /// <param name="_map"></param>
    /// <param name="_timedb"></param>
    public class CreateDeviceOnlineLogSubscriber(IMapper _map, CoffeeMachineTimescaleDBContext _timedb) : ICapSubscribe
    {
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.CreateDeviceOnlineLog)]
        public async Task Handle(DeviceOnlineLog input)
        {
            //var log = _map.Map<DeviceOnlineLog>(input);
            await _timedb.AddAsync(input);
            await _timedb.SaveChangesAsync();
        }
    }
}
