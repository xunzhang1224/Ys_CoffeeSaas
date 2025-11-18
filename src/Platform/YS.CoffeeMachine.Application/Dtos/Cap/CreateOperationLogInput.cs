using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.Cap
{
    /// <summary>
    /// CreateOperationLogInput
    /// </summary>
    public class CreateOperationLogInput
    {
        /// <summary>
        /// 批次号
        /// 目前先用主键id代替
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 具体操作的设备编号
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public OperationResultEnum OperationResult { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// 请求方式类型
        /// </summary>
        public string RequestWayType { get; set; }
        /// <summary>
        /// 请求类型
        /// </summary>
        public RequestTypeEnum RequestType { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestMsg { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LastModifyTime { get; set; }

        /// <summary>
        /// 创建用户id
        /// </summary>
        public long? CreateUserId { get; set; }

        /// <summary>
        /// 创建用户名
        /// </summary>
        public string? CreateUserName { get; set; }

        /// <summary>
        /// 企业id
        /// </summary>
        public long EnterpriseinfoId { get; set; }

        /// <summary>
        /// 操作日志附表
        /// </summary>
        public List<OperationSubLog> OperationSubLogs { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateOperationLogInput() { }

        /// <summary>
        /// 新增日志
        /// </summary>
        /// <param name="code"></param>
        /// <param name="mid"></param>
        /// <param name="operationName"></param>
        /// <param name="requestUrl"></param>
        /// <param name="requestMsg"></param>
        /// <param name="requestType"></param>
        /// <param name="requestWayType"></param>
        /// <param name="ipAddress"></param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        /// <param name="enterpriseinfoId"></param>
        /// <param name="operationSubLogs"></param>
        public CreateOperationLogInput(string code, string mid, string operationName, string requestUrl, RequestTypeEnum requestType, string requestWayType, string ipAddress,
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
    }
}
