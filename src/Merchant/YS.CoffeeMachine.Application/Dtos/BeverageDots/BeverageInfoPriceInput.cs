using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.BeverageDots
{
    /// <summary>
    /// 饮品价格信息
    /// </summary>
    public class BeverageInfoPriceInput
    {
        /// <summary>
        /// 饮品id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 折扣价
        /// </summary>
        public decimal DiscountedPrice { get; set; }
    }
}
