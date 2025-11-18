using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

namespace YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos
{
    /// <summary>
    /// 饮品信息入参
    /// </summary>
    public class BeverageInfoInput: QueryRequest
    {
        /// <summary>
        /// 饮品名称
        /// </summary>
        public string BevergeInfoName { get; set; }

        /// <summary>
        /// 饮品元素
        /// </summary>
        public FormulaTypeEnum? FormulaType { get; set; }
    }
}
