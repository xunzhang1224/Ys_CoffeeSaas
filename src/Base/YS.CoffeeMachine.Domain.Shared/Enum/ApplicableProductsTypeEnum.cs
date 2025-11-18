using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 适用商品类型
    /// </summary>
    public enum ApplicableProductsTypeEnum
    {
        /// <summary>
        /// 全部商品
        /// </summary>
        All,

        /// <summary>
        /// 指定商品可用
        /// </summary>
        Available,

        /// <summary>
        /// 指定商品不可用
        /// </summary>
        NotAvailable
    }
}
