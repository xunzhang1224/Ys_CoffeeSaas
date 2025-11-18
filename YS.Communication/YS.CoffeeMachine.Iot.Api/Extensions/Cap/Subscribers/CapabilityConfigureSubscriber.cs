namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Subscribers
{
    using DotNetCore.CAP;

    using YS.CoffeeMachine.Domain.Shared.Const;
    using YS.CoffeeMachine.Iot.Api.Services;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities;

    /// <summary>
    /// 能力配置消息订阅器，用于处理设备能力配置更新的消息
    /// </summary>
    public class CapabilityConfigureSubscriber : ICapSubscribe
    {
        private readonly DeviceService _deviceService;

        /// <summary>
        /// 构造函数，注入所需服务
        /// </summary>
        /// <param name="deviceService">设备服务</param>
        public CapabilityConfigureSubscriber(DeviceService _deviceService)
        {
            this._deviceService = _deviceService;
        }

        /// <summary>
        /// 处理能力配置上报消息
        /// </summary>
        /// <param name="input">上行数据请求实体（1010）</param>
        /// <returns>异步任务</returns>
        [CapSubscribe(CapConst.CapabilityConfigure)]
        public async Task Handle(UplinkEntity1010.Request input)
        {
            await _deviceService.SetCapabilityConfigure(input);
        }
    }
}