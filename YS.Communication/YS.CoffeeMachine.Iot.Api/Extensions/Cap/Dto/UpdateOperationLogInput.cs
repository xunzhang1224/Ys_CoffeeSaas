namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto
{
    using YS.CoffeeMachine.Domain.Shared.Enum;

    /// <summary>
    /// 更新操作日志输入 DTO，用于接收更新操作日志状态的请求参数
    /// </summary>
    public class UpdateOperationLogInput
    {
        /// <summary>
        /// 批次号，标识本次操作的唯一编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备编号（Mid），表示该操作对应的设备
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 操作结果枚举值，如：已下发、已执行、未执行等
        /// </summary>
        public OperationResultEnum OperationResult { get; set; }

        /// <summary>
        /// 错误信息，当操作失败时记录原因
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}