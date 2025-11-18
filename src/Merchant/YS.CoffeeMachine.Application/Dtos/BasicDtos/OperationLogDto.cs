using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.BasicDtos
{
    /// <summary>
    /// 修改实体
    /// </summary>
    public class UpdateOperationLogDto
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 修改的子实体
        /// </summary>
        public List<UpdateOperationSubLogDto> UpdateSubDtos { get; set; }
    }

    /// <summary>
    /// 查询实体
    /// </summary>
    public class OperationLogQueriesDto()
    {
        /// <summary>
        /// 具体操作的设备编号
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string Code { get; set; }
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
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// 本批次操作的数量
        /// </summary>
        public int SubCount { get { return OperationSubLogs.Count(); } }
        /// <summary>
        /// 本批次指令已下发数量
        /// </summary>
        public int SubCommandIssuedCount { get { return OperationSubLogs.Count(x => x.OperationResult == OperationResultEnum.CommandIssued); } }
        /// <summary>
        /// 本批次设备未执行数量
        /// </summary>
        public int SubCommandUnexecuted { get { return OperationSubLogs.Count(x => x.OperationResult == OperationResultEnum.CommandUnexecuted); } }
        /// <summary>
        /// 本批次设备已执行数量
        /// </summary>
        public int SubCommandExecuted { get { return OperationSubLogs.Count(x => x.OperationResult == OperationResultEnum.CommandExecuted); } }

        /// <summary>
        /// 操作结果
        /// </summary>
        public OperationResultEnum OperationResult { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 未执行原因
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                if (SubCommandUnexecuted > 0)
                {
                    return OperationSubLogs.First(x => x.OperationResult == OperationResultEnum.CommandUnexecuted).ErrorMsg;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// 子实体
        /// </summary>
        public List<OperationSubLogDto> OperationSubLogs { get; set; }
    }
    /// <summary>
    /// 新增实体
    /// </summary>
    public class OperationLogDto()
    {
        /// <summary>
        /// 具体操作的设备编号
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string Code { get; set; }
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
        /// 租户id
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// 子实体
        /// </summary>

        public List<OperationSubLogDto> OperationSubLogs { get; set; }
    }

    /// <summary>
    /// 修改子实体
    /// </summary>
    public class UpdateOperationSubLogDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public OperationResultEnum OperationResult { get; set; }
        /// <summary>
        /// 未执行原因
        /// </summary>
        public string ErrorMsg { get; set; }
    }

    /// <summary>
    /// 子实体
    /// </summary>
    public class OperationSubLogDto
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string Mid { get; set; }
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
        /// 请求参数
        /// </summary>
        public string RequestMsg { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public OperationResultEnum OperationResult { get; set; }
        /// <summary>
        /// 未执行原因
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
