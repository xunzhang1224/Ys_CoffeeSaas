namespace YS.CoffeeMachine.Application.Dtos.BasicDtos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
    using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
    using YS.CoffeeMachine.Domain.Shared.Enum;

    /// <summary>
    /// 更新操作日志 DTO，用于接收更新批次日志的请求数据
    /// </summary>
    public class UpdateOperationLogDto
    {
        /// <summary>
        /// 批次号，标识一次批量操作的唯一编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 子操作日志列表，用于更新多个子项的状态或错误信息
        /// </summary>
        public List<UpdateOperationSubLogDto> UpdateSubDtos { get; set; }
    }

    /// <summary>
    /// 操作日志查询返回 DTO，用于展示操作日志详情及执行状态统计
    /// </summary>
    public class OperationLogQueriesDto
    {
        /// <summary>
        /// 具体操作的设备编号（Mid）
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 批次号，标识本次操作的唯一编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 操作名称，描述本次操作类型或用途
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// 请求方式类型，如 HTTP 方法（GET/POST）
        /// </summary>
        public string RequestWayType { get; set; }

        /// <summary>
        /// 请求类型，例如：配置下发、状态查询等
        /// </summary>
        public RequestTypeEnum RequestType { get; set; }

        /// <summary>
        /// 请求参数，记录请求体内容
        /// </summary>
        public string RequestMsg { get; set; }

        /// <summary>
        /// 请求地址，记录 API 地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 当前批次操作的总数量
        /// </summary>
        public int SubCount => OperationSubLogs?.Count() ?? 0;

        /// <summary>
        /// 已下发指令的数量
        /// </summary>
        public int SubCommandIssuedCount => OperationSubLogs?.Count(x => x.OperationResult == OperationResultEnum.CommandIssued) ?? 0;

        /// <summary>
        /// 设备未执行的数量
        /// </summary>
        public int SubCommandUnexecuted => OperationSubLogs?.Count(x => x.OperationResult == OperationResultEnum.CommandUnexecuted) ?? 0;

        /// <summary>
        /// 设备已执行的数量
        /// </summary>
        public int SubCommandExecuted => OperationSubLogs?.Count(x => x.OperationResult == OperationResultEnum.CommandExecuted) ?? 0;

        /// <summary>
        /// 整体操作结果，根据子操作结果综合判断
        /// </summary>
        public OperationResultEnum OperationResult
        {
            get
            {
                if (SubCommandUnexecuted > 0)
                {
                    return OperationResultEnum.CommandUnexecuted;
                }
                else if (SubCommandUnexecuted == 0 && SubCommandIssuedCount == SubCommandExecuted)
                {
                    return OperationResultEnum.CommandExecuted;
                }
                else
                {
                    return OperationResultEnum.CommandIssued;
                }
            }
        }

        /// <summary>
        /// 第一个未执行子项的错误原因
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                if (SubCommandUnexecuted > 0 && OperationSubLogs != null)
                {
                    return OperationSubLogs.First(x => x.OperationResult == OperationResultEnum.CommandUnexecuted).ErrorMsg;
                }
                return null;
            }
        }

        /// <summary>
        /// 子操作日志集合
        /// </summary>
        public List<OperationSubLogDto> OperationSubLogs { get; set; }
    }

    /// <summary>
    /// 新增操作日志 DTO，用于创建新的操作日志记录
    /// </summary>
    public class OperationLogDto
    {
        /// <summary>
        /// 具体操作的设备编号（Mid）
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 批次号，标识本次操作的唯一编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 操作名称，描述本次操作类型或用途
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// 请求方式类型，如 HTTP 方法（GET/POST）
        /// </summary>
        public string RequestWayType { get; set; }

        /// <summary>
        /// 请求类型枚举，如：配置下发、状态查询等
        /// </summary>
        public RequestTypeEnum RequestType { get; set; }

        /// <summary>
        /// 请求参数，记录请求体内容
        /// </summary>
        public string RequestMsg { get; set; }

        /// <summary>
        /// 客户端 IP 地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 租户 ID，用于多租户系统区分
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// 请求地址，记录 API 地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 子操作日志集合
        /// </summary>
        public List<OperationSubLogDto> OperationSubLogs { get; set; }
    }

    /// <summary>
    /// 更新子操作日志 DTO，用于部分更新子操作状态
    /// </summary>
    public class UpdateOperationSubLogDto
    {
        /// <summary>
        /// 设备编号（Mid）
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 操作结果，如：已下发、已执行、未执行等
        /// </summary>
        public OperationResultEnum OperationResult { get; set; }

        /// <summary>
        /// 未执行的原因描述（可为空）
        /// </summary>
        public string ErrorMsg { get; set; }
    }

    /// <summary>
    /// 子操作日志 DTO，用于表示单个设备的操作详情
    /// </summary>
    public class OperationSubLogDto
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备编号（Mid）
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 饮品应用方式（可为空）
        /// </summary>
        public BeverageAppliedType? AppliedType { get; set; }

        /// <summary>
        /// 替换内容方式（可为空）
        /// </summary>
        public ReplaceContentType? ContentType { get; set; }

        /// <summary>
        /// 替换目标（可为空）
        /// </summary>
        public string? ReplaceTarget { get; set; }

        /// <summary>
        /// 操作结果，如：已下发、已执行、未执行等
        /// </summary>
        public OperationResultEnum OperationResult { get; set; }

        /// <summary>
        /// 未执行原因（可为空）
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}