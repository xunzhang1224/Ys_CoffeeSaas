namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Subscribers
{
    using DotNetCore.CAP;

    using YS.CoffeeMachine.Domain.Shared.Const;
    using YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto;
    using YS.CoffeeMachine.Iot.Api.Iot.Interfaces;
    using YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

    /// <summary>
    /// 设备绑定消息订阅器，用于处理来自 CAP 的设备绑定指令
    /// </summary>
    public class DeviceBindSubscriber : ICapSubscribe
    {
        private readonly ICommandSenderService _commandSender;

        /// <summary>
        /// 构造函数，注入所需服务
        /// </summary>
        /// <param name="commandSender">命令发送服务</param>
        public DeviceBindSubscriber(ICommandSenderService _commandSender)
        {
            this._commandSender = _commandSender;
        }

        /// <summary>
        /// 处理设备绑定消息（DownlinkEntity1100）
        /// </summary>
        /// <param name="input">下行数据实体 1100，表示设备绑定操作</param>
        /// <returns>异步任务</returns>
        [CapSubscribe(CapConst.DeviceBind)]
        public async Task Handle(DownlinkEntity1100 input)
        {
            await _commandSender.Downlink1100SendAsync(new DownSeedRequestBase<DownlinkEntity1100>(input, input.Mid));
        }
    }
}