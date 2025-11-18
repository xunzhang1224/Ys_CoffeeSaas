using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Dtos
{
    /// <summary>
    /// 入参
    /// </summary>
    public class GoodsDto : QueryRequest
    {
        /// <summary>
        /// 商品名称/sku
        /// </summary>
        public string str { get; set; }
    }
}
