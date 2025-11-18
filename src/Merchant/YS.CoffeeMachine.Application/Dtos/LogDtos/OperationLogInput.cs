using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.LogDtos
{
    /// <summary>
    /// 操作日志查询参数
    /// </summary>
    public class OperationLogInput: QueryRequest
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备SN
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public OperationResultEnum? OperationResult { get; set; }

        /// <summary>
        /// 操作名称（暂时无定义，等设备对接后整理，确认是用枚举还是字典）
        /// </summary>
        public string OperationName { get; set; }
    }
}
