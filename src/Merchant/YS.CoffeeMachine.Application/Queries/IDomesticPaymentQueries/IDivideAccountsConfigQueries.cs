using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.DivideAccountsConfigDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IDomesticPaymentQueries
{
    /// <summary>
    /// 分账相关查询
    /// </summary>
    public interface IDivideAccountsConfigQueries
    {
        /// <summary>
        /// 获取当前企业所有可用支付方式
        /// </summary>
        /// <returns></returns>
        public Task<List<SystemPaymentMethodSelect>> GetSystemPaymentMethodSelects();

        /// <summary>
        /// 获取当前系统支付方式下所有支付列表
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        Task<List<M_PaymentMethodSelect>> GetmPaymentMethodSelects(long paymentMethodId);

        /// <summary>
        /// 获取当前支付方式绑定的设备
        /// </summary>
        /// <returns></returns>
        public Task<List<PaymentMethodBindDeviceSelect>> GetPaymentMethodBindDeviceSelects(long paymentMethodId);

        /// <summary>
        /// 查询支付分账配置信息分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<DivideAccountsConfigOutput>> GetDivideAccountsConfigPageList(DivideAccountsConfigInput input);
    }
}