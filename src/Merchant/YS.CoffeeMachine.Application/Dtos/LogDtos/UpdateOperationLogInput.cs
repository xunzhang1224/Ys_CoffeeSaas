using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.LogDtos
{
    /// <summary>
    /// 更新操作日志的输入参数
    /// </summary>
   public class UpdateOperationLogInput
{
    /// <summary>
    /// 操作日志编号，用于唯一标识一条操作记录
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 设备编号或唯一标识符（Mid - Machine ID）
    /// </summary>
    public string Mid { get; set; }

    /// <summary>
    /// 操作结果枚举，表示本次操作的成功或失败状态
    /// </summary>
    public OperationResultEnum OperationResult { get; set; }

    /// <summary>
    /// 错误信息描述，当操作结果为失败时记录具体错误信息
    /// </summary>
    public string ErrorMsg { get; set; }
}
}
