using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.PlatformDto.PaymentDtos;

namespace YS.CoffeeMachine.Application.PlatformQueries.IPaymentQueries
{
    /// <summary>
    /// 支付配置查询
    /// </summary>
    public interface IPaymentConfigQueries
    {
        /// <summary>
        /// 平台端获取支付配置列表
        /// </summary>
        /// <returns></returns>
        Task<List<P_PaymetConfigDto>> GetPaymentConfigList();
    }
}
