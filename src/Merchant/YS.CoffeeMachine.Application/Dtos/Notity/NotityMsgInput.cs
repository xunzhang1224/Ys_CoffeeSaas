using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.Notity
{
    /// <summary>
    /// 消息分页查询
    /// </summary>
    public class NotityMsgInput : QueryRequest
    {
        /// <summary>
        /// 时间
        /// </summary>
        public List<DateTime>? Times { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string? contactAddress { get; set; }
    }
}
