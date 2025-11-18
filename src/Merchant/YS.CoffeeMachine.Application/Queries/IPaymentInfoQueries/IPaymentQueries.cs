using YS.CoffeeMachine.Application.Dtos.PaymentDtos;

namespace YS.CoffeeMachine.Application.Queries.IPaymentInfoQueries
{
    /// <summary>
    /// 支付查询接口
    /// </summary>
    public interface IPaymentQueries
    {
        /// <summary>
        /// 获取符合条件的平台端支付配置
        /// </summary>
        /// <returns></returns>
        Task<List<P_PaymentConfigDto>> GetPpaymentConfig();

        /// <summary>
        /// 获取支付配置列表
        /// </summary>
        /// <returns></returns>
        Task<List<PaymentConfigDto>> GetPaymentConfig();
    }
}