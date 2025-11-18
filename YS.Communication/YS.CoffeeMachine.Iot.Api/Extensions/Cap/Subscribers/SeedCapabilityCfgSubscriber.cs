using DotNetCore.CAP;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Iot.Api.Iot.Interfaces;
using YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Subscribers
{
    /// <summary>
    /// SeedCapabilityCfgSubscriber
    /// </summary>
    /// <param name="_commandSender"></param>
    public class SeedCapabilityCfgSubscriber(ICommandSenderService _commandSender) : ICapSubscribe
    {
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.SeedCapabilityCfg)]
        public async Task Handle(DownlinkEntity1100 input)
        {
            await _commandSender.Downlink1100SendAsync(new DownSeedRequestBase<DownlinkEntity1100>(input, input.Mid));
        }
    }
}
