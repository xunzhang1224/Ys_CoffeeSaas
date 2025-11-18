namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog
{
    using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 操作日志子项
    /// </summary>
    public class OperationSubLog : Entity
    {
        /// <summary>
        /// GetKeys
        /// </summary>
        /// <returns></returns>
        public override object[] GetKeys() => new object[] { Mid, OperationLogId };

        /// <summary>
        /// 操作日志id
        /// </summary>
        public long OperationLogId { get; set; }

        /// <summary>
        /// 操作日志
        /// </summary>
        public OperationLog OperationLog { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 型号名
        /// </summary>
        public string? DeviceModelName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string? RequestMsg { get; set; }

        /// <summary>
        /// 饮品应用方式
        /// </summary>
        public BeverageAppliedType? AppliedType { get; set; }

        /// <summary>
        /// 替换内容方式
        /// </summary>
        public ReplaceContentType? ContentType { get; set; }

        /// <summary>
        /// 替换目标
        /// </summary>
        public string? ReplaceTarget { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public OperationResultEnum OperationResult { get; set; }

        /// <summary>
        /// 未执行原因
        /// </summary>
        public string? ErrorMsg { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public OperationSubLog() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <param name="mid"> 设备编号</param>
        /// <param name="appliedType"> 饮品应用方式</param>
        /// <param name="contentType"> 替换内容方式</param>
        /// <param name="replaceTarget"> 替换目标</param>
        /// <param name="operationResult"> 操作结果</param>
        /// <param name="errorMsg"> 未执行原因</param>
        public OperationSubLog(string deviceModelName, string deviceName, string mid, BeverageAppliedType? appliedType,
            ReplaceContentType? contentType, string? replaceTarget, OperationResultEnum operationResult, string? errorMsg, string requestMsg)
        {
            //OperationLogId = operationLogId;
            DeviceModelName = deviceModelName;
            DeviceName = deviceName;
            Mid = mid;
            RequestMsg = requestMsg;
            AppliedType = appliedType;
            ContentType = contentType;
            ReplaceTarget = replaceTarget;
            OperationResult = operationResult;
            ErrorMsg = errorMsg;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="operationResult"></param>
        /// <param name="errorMsg"></param>
        public void Update(OperationResultEnum operationResult, string errorMsg)
        {
            OperationResult = operationResult;
            ErrorMsg = errorMsg;
        }
    }
}
