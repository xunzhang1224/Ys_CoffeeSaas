using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.PlatformDto.BasicDtos
{
    /// <summary>
    /// 故障码查询入参
    /// </summary>
    public class FaultCodeInput : QueryRequest
    {
        /// <summary>
        /// 故障码标识
        /// </summary>
        public string? Code { get; set; } = null;

        /// <summary>
        /// 多语言标识
        /// </summary>
        public string? LanCode { get; set; } = null;

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// 类型
        /// </summary>
        public FaultCodeTypeEnum? Type { get; set; }
    }
}
