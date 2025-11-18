namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Subscribers
{
    using DotNetCore.CAP;

    using YS.CoffeeMachine.Cap.IServices;
    using YS.CoffeeMachine.Domain.Shared.Const;
    using YS.CoffeeMachine.Iot.Api.Services;
    using YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities;
    using YSCore.CoffeeMachine.SignalR.Services;

    /// <summary>
    /// 指标上报消息订阅器，用于处理设备指标数据并通知前端
    /// </summary>
    public class MetricSubscriber : ICapSubscribe
    {
        private readonly DeviceService _deviceService;
        private readonly IPublishService _capPublisher;

        /// <summary>
        /// 构造函数，注入所需服务
        /// </summary>
        /// <param name="deviceService">设备服务</param>
        /// <param name="capPublisher">CAP 消息发布服务</param>
        public MetricSubscriber(DeviceService _deviceService, IPublishService _capPublisher)
        {
            this._deviceService = _deviceService;
            this._capPublisher = _capPublisher;
        }

        /// <summary>
        /// 处理指标上报消息（UplinkEntity1012.Request）
        /// </summary>
        /// <param name="input">上行数据请求实体（1012）</param>
        /// <returns>异步任务</returns>
        [CapSubscribe(CapConst.MetricCap)]
        public async Task Handle(UplinkEntity1012.Request input)
        {
            await _deviceService.SetDeviceMetric(input);
            // 通过 CAP 通知前端更新
            await _capPublisher.SendMessage(CapConst.MetricNotice, input.Mid);
        }
    }
}