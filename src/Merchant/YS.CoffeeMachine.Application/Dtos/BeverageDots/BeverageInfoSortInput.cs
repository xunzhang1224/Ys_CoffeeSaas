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
    public class BeverageInfoSortInput
    {
        /// <summary>
        /// 饮品id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int Sort { get; set; }
    }
}
