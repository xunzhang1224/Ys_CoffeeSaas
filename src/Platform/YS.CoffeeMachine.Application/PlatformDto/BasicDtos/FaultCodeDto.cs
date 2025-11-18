using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.PlatformDto.BasicDtos
{
    /// <summary>
    /// 故障码Dto
    /// </summary>
    public class FaultCodeDto
    {
        /// <summary>
        /// 故障码标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 多语言标识
        /// </summary>
        public string LanCode { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public FaultCodeTypeEnum? Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
