using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.PlatformDto.CurrencyDtos
{
    /// <summary>
    /// 币种dto
    /// </summary>
    public class CurrencyDto
    {
        /// <summary>
        /// 币种id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 货币代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 货币名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 货币符号
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// 默认显示格式
        /// </summary>
        public CurrencyShowFormatEnum CurrencyShowFormat { get; set; }

        /// <summary>
        /// 金额精度
        /// </summary>
        public int Accuracy { get; set; }

        /// <summary>
        /// 舍入类型
        /// </summary>
        public RoundingTypeEnum RoundingType { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum Enabled { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
