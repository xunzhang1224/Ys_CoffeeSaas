using AutoMapper;
using DotNetCore.CAP;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers
{
    /// <summary>
    /// 记录消息通知
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_map"></param>
    public class CreateNotityMsgSubscriber(CoffeeMachinePlatformDbContext _db, IMapper _map) : ICapSubscribe
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.CreateNotityMsg)]
        public async Task Handle(List<CreateNotityMsgDto> input)
        {
            var models = _map.Map<List<NotityMsg>>(input);
            await _db.AddRangeAsync(models);
            await _db.SaveChangesAsync();
        }
    }
}
