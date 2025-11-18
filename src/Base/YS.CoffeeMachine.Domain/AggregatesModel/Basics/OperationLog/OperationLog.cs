namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog
{
    using YS.CoffeeMachine.Domain.AggregatesModel.Basics.EnterpriseDeviceBaseEntity;
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 操作日志聚合根
    /// </summary>
    public class OperationLog : EDBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 批次号
        /// 目前先用主键id代替
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 设备生成编号
        /// </summary>
        public string Mid { get; private set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public OperationResultEnum OperationResult { get; private set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string OperationName { get; private set; }

        /// <summary>
        /// 请求方式类型
        /// </summary>
        public string RequestWayType { get; private set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public RequestTypeEnum RequestType { get; private set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; private set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; private set; }

        /// <summary>
        /// OperationSubLogs
        /// </summary>
        public List<OperationSubLog> OperationSubLogs { get; private set; }

        ///// <summary>
        ///// 本批次操作的数量
        ///// </summary>
        //[NotMapped]
        //public int SubCount { get { return OperationSubLogs.Count(); } }
        ///// <summary>
        ///// 本批次指令已下发数量
        ///// </summary>
        //[NotMapped]
        //public int SubCommandIssuedCount { get { return OperationSubLogs.Count(x => x.OperationResult == OperationResultEnum.CommandIssued); } }
        ///// <summary>
        ///// 本批次设备未执行数量
        ///// </summary>
        //[NotMapped]
        //public int SubCommandUnexecuted { get { return OperationSubLogs.Count(x => x.OperationResult == OperationResultEnum.CommandUnexecuted); } }
        ///// <summary>
        ///// 本批次设备已执行数量
        ///// </summary>
        //[NotMapped]
        //public int SubCommandExecuted { get { return OperationSubLogs.Count(x => x.OperationResult == OperationResultEnum.CommandExecuted); } }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected OperationLog()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">批次号</param>
        /// <param name="mid">设备生成编号</param>
        /// <param name="operationName">操作名称</param>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="requestType">请求类型</param>
        /// <param name="requestWayType">请求方式类型</param>
        /// <param name="ipAddress">ip地址</param>
        /// <param name="createUserId">创建用户Id</param>
        /// <param name="createUserName">创建用户名</param>
        /// <param name="enterpriseinfoId">企业信息Id</param>
        /// <param name="operationSubLogs">操作子日志</param>
        public OperationLog(string code, string mid, string operationName, string requestUrl,
            RequestTypeEnum requestType, string requestWayType, string ipAddress,
            long createUserId, string createUserName, long enterpriseinfoId, List<OperationSubLog> operationSubLogs)
        {
            Code = code;
            Mid = mid;
            OperationName = operationName;
            RequestUrl = requestUrl;
            RequestWayType = requestWayType;
            RequestType = requestType;
            IpAddress = ipAddress;
            CreateUserId = createUserId;
            CreateUserName = createUserName;
            EnterpriseinfoId = enterpriseinfoId;
            OperationSubLogs = operationSubLogs;
        }

        /// <summary>
        /// 修改主表操作状态
        /// </summary>
        /// <param name="operationResult"></param>
        public void UpdateReslut(OperationResultEnum operationResult)
        {
            OperationResult = operationResult;
        }
    }
}
